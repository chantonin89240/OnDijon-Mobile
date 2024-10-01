using System;
using System.Globalization;
using Xamarin.Forms;

namespace OnDijon.Common.Utils.Converters
{
    public class ImageOrNotHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty((string)value) ? 70 : 225;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
