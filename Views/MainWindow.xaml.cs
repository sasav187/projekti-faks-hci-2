using hci_projekat2.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace hci_projekat2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
                
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = (CalculatorViewModel)DataContext;

            switch (e.Key)
            {
               
                case Key.D0: case Key.NumPad0: viewModel.NumberCommand.Execute("0"); break;
                case Key.D1: case Key.NumPad1: viewModel.NumberCommand.Execute("1"); break;
                case Key.D2: case Key.NumPad2: viewModel.NumberCommand.Execute("2"); break;
                case Key.D3: case Key.NumPad3: viewModel.NumberCommand.Execute("3"); break;
                case Key.D4: case Key.NumPad4: viewModel.NumberCommand.Execute("4"); break;
                case Key.D5: case Key.NumPad5: viewModel.NumberCommand.Execute("5"); break;
                case Key.D6: case Key.NumPad6: viewModel.NumberCommand.Execute("6"); break;
                case Key.D7: case Key.NumPad7: viewModel.NumberCommand.Execute("7"); break;
                case Key.D8: case Key.NumPad8: viewModel.NumberCommand.Execute("8"); break;
                case Key.D9: case Key.NumPad9: viewModel.NumberCommand.Execute("9"); break;


                case Key.Add: viewModel.OperationCommand.Execute("+"); break;
                case Key.Subtract: viewModel.OperationCommand.Execute("-"); break;
                case Key.Multiply: viewModel.OperationCommand.Execute("×"); break;
                case Key.Divide: viewModel.OperationCommand.Execute("÷"); break;
                case Key.Enter: viewModel.OperationCommand.Execute("="); break;
                case Key.Escape: viewModel.OperationCommand.Execute("C"); break;
                case Key.Back: viewModel.OperationCommand.Execute("⌫"); break; 

                case Key.Decimal: viewModel.NumberCommand.Execute("."); break;
            }

            e.Handled = true; 
        }
    }
}