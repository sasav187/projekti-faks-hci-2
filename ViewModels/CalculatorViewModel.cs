using hci_projekat2.Commands;
using hci_projekat2.Models;
using System.ComponentModel;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace hci_projekat2.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private readonly CalculatorModel _model = new();
        private string _currentInput = "0";
        private string _history = "";
        private double _firstNumber;
        private string _pendingOperation;
        private bool _isNewInput = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public enum CalculatorMode { Normal, Scientific, Programmer }
        public CalculatorMode Mode { get; set; } = CalculatorMode.Normal;

        public enum Theme { Light, Dark, Blue, }
        public List<Theme> Themes { get; } = new() { Theme.Light, Theme.Dark, Theme.Blue };

        private Theme? _selectedTheme = null;
        public Theme? SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                _selectedTheme = value;
                ApplyTheme(value.Value);
                OnPropertyChanged(nameof(SelectedTheme));
            }
        }

        public RelayCommand NumberCommand { get; }
        public RelayCommand OperationCommand { get; }
        public RelayCommand ScientificCommand { get; }
        public RelayCommand BitwiseCommand { get; }
        public RelayCommand BaseCommand { get; }
        public RelayCommand ThemeCommand { get; }
        public RelayCommand ThemeRefreshCommand { get; }

        public CalculatorViewModel()
        {
            SelectedTheme = Theme.Dark;
            NumberCommand = new RelayCommand(AddNumber);
            OperationCommand = new RelayCommand(PerformOperation);
            ScientificCommand = new RelayCommand(PerformScientific);
            BitwiseCommand = new RelayCommand(PerformBitwise);
            BaseCommand = new RelayCommand(ChangeBase);
            ThemeCommand = new RelayCommand(ChangeTheme);
            ThemeRefreshCommand = new RelayCommand((_) => ApplyTheme(theme: (Theme)SelectedTheme));
        }

        private void AddNumber(object parameter)
        {
            var number = parameter.ToString();

            if (number == ".")
            {
                if (_currentInput.Contains(".") || _currentBase != 10)
                    return;

                if (string.IsNullOrEmpty(_currentInput) || _isNewInput)
                {
                    CurrentInput = "0.";
                    _isNewInput = false;
                    return;
                }

                else
                {
                    CurrentInput += ".";
                }

                _isNewInput = false;
                return;
            }

            if (_currentBase == 2 && !(number.Equals("0") || number.Equals("1")))
            {
                return;
            }

            if (_currentBase == 16 && !Uri.IsHexDigit(number[0]))
            {
                return;
            }

            if (_isNewInput)
            {
                CurrentInput = number;
                _isNewInput = false;
            }
            else
                CurrentInput += number;
        }

        private void PerformOperation(object parameter)
        {
            var operation = parameter.ToString();

            if (operation == "=" && _pendingBitwiseOperation != null)
            {
                var secondOperand = Convert.ToInt32(CurrentInput, _currentBase);
                var result = _model.BitwiseCalculate(_firstBitwiseOperand, secondOperand, _pendingBitwiseOperation);
                CurrentInput = _model.ConvertToBase(result, _currentBase);
                _pendingBitwiseOperation = null;
                return;
            }

            if (operation == "C")
            {
                CurrentInput = "0";
                _history = "";
                _isNewInput = true;
                _pendingOperation = null;
                return;
            }
            else if (operation == "=")
            {
                if (!string.IsNullOrEmpty(_pendingOperation))
                {
                    if (double.TryParse(CurrentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double secondNumber))
                    {
                        double result = _model.Calculate(_firstNumber, secondNumber, _pendingOperation);
                        CurrentInput = FormatNumber(result);
                    }
                    _pendingOperation = null;
                    _isNewInput = true;
                    return;
                }
            }
            else if (operation == "%")
            {
                if (!string.IsNullOrEmpty(_pendingOperation))
                {
                    double secondNumber = double.Parse(CurrentInput);
                    double percentageValue;

                    if (_pendingOperation == "+" || _pendingOperation == "-")
                    {
                        percentageValue = (secondNumber / 100) * _firstNumber;
                    }
                    else if (_pendingOperation == "×" || _pendingOperation == "÷")
                    {
                        percentageValue = secondNumber / 100;
                    }
                    else
                    {
                        throw new InvalidOperationException("Nepodržana operacija za procent");
                    }

                    double result = _model.Calculate(_firstNumber, percentageValue, _pendingOperation);
                    CurrentInput = result.ToString();
                    _pendingOperation = null;
                    _isNewInput = true;
                }
                else
                {
                    double current = double.Parse(CurrentInput);
                    CurrentInput = (current / 100).ToString();
                    _isNewInput = true;
                }
                return;
            }
            else
            {
                if (double.TryParse(CurrentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out _firstNumber))
                {
                    _pendingOperation = operation;
                    _isNewInput = true;
                }
            }
        }

        private string FormatNumber(double number)
        {
            string result = number.ToString(CultureInfo.InvariantCulture);

            if (result.Contains("."))
            {
                result = result.TrimEnd('0').TrimEnd('.');
            }

            if (string.IsNullOrEmpty(result))
            {
                return "0";
            }

            return result;
        }

        private void PerformScientific(object parameter)
        {
            var function = parameter.ToString();

            if (double.TryParse(CurrentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                var result = _model.ScientificCalculate(value, function);
                CurrentInput = FormatNumber(result);
                _history += $"{function}({value}) = {result}\n";
            }
        }

        private int _firstBitwiseOperand;
        private string _pendingBitwiseOperation;
        private int _currentBase = 10;

        private void PerformBitwise(object parameter)
        {
            var operation = parameter.ToString();

            if (operation == "NOT")
            {
                var currentValue = Convert.ToInt32(CurrentInput, _currentBase);
                var result = _model.BitwiseCalculate(currentValue, 0, operation);
                CurrentInput = _model.ConvertToBase(result, _currentBase);
            }
            else if (operation == "<<" || operation == ">>")
            {
                _firstBitwiseOperand = Convert.ToInt32(CurrentInput, _currentBase);
                _pendingBitwiseOperation = operation;
                _isNewInput = true;
            }
            else
            {
                _firstBitwiseOperand = Convert.ToInt32(CurrentInput, _currentBase);
                _pendingBitwiseOperation = operation;
                _isNewInput = true;
            }
        }


        private void ChangeBase(object parameter)
        {
            var newBase = int.Parse(parameter.ToString());
            var currentNumber = Convert.ToInt32(CurrentInput, _currentBase);
            CurrentInput = _model.ConvertToBase(currentNumber, newBase);
            _currentBase = newBase;
        }

        private void ChangeTheme(object parameter)
        {
            SelectedTheme = (Theme)int.Parse(parameter.ToString());
        }

        private void ApplyTheme(Theme theme)
        {


            var dict = new ResourceDictionary();
            dict.Source = theme switch
            {
                Theme.Light => new Uri("Styles/LightTheme.xaml", UriKind.Relative),
                Theme.Dark => new Uri("Styles/DarkTheme.xaml", UriKind.Relative),
                Theme.Blue => new Uri("Styles/BlueTheme.xaml", UriKind.Relative),
                _ => new Uri("Themes/LightTheme.xaml", UriKind.Relative)
            };

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        public string CurrentInput
        {
            get => _currentInput;
            set
            {
                _currentInput = value;
                OnPropertyChanged(nameof(CurrentInput));
            }
        }

        public string History
        {
            get => _history;
            set
            {
                _history = value;
                OnPropertyChanged(nameof(History));
            }
        }

        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}