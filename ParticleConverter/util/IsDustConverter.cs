using System;
using System.Globalization;
using System.Windows.Data;

namespace ParticleConverter.util
{
    class IsDustConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string @string && @string.Equals("dust");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string @string && @string.Equals("dust");
        }
    }
}
