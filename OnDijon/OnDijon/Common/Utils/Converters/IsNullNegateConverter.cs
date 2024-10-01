using System;
using System.Globalization;
using Xamarin.Forms;

namespace OnDijon.Common.Utils.Converters
{
    public class IsNullNegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value == null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
