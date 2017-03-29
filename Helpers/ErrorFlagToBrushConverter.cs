using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace NavigationAndDialogWpf.Helpers
{
    public class ErrorFlagToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (bool)value;
            var resourcePrefix = parameter.ToString();
            return Application.Current.Resources[resourcePrefix + (input ? "Error" : "")] as Brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}