using System;
using System.Globalization;
using Xamarin.Forms;

namespace OnDijon.Common.Utils.Converters
{
    public class RadioButtonBoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter.Equals(value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? parameter : null;
        }

    }
}
