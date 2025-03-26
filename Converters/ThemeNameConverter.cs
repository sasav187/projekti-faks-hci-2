using System;
using System.Globalization;
using System.Windows.Data;
using hci_projekat2.ViewModels;

namespace hci_projekat2.Converters
{
    public class ThemeNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                CalculatorViewModel.Theme.Light => "Svijetlo",
                CalculatorViewModel.Theme.Dark => "Tamno",
                CalculatorViewModel.Theme.Blue => "Plavo",
                _ => value.ToString()
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
