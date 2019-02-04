using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace UserInterface.Converters
{
    internal class BoolToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof (bool)) return null;
            var obj = (bool)value;
            return obj? Brushes.Red: Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}