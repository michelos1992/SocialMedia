using System;
using System.Globalization;
using System.Windows.Data;

namespace UserInterface.Converters
{
    internal class BoolToDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof (bool)) return null;
            var obj = (bool)value;
            return obj ? 1.0 : 0.2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}