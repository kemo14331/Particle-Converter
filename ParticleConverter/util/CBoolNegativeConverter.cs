using System;
using System.Globalization;
using System.Windows.Data;

namespace ParticleConverter.util
{
    /// <summary>
    /// GUIのバインディングで、bool値を反転してくれるコンバーター
    /// </summary>
    public class CBoolNegativeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is bool boolean && boolean);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is bool boolean && boolean);
        }

    }
}
