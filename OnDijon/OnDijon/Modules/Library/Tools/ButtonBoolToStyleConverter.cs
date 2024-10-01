using System;
using System.Globalization;
using Xamarin.Forms;

namespace OnDijon.Modules.Library.Tools
{
    public class ButtonBoolToStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return Application.Current.Resources["ButtonRegular"] as Style;
            }
            else
            {
                return Application.Current.Resources["ButtonConfirm"] as Style;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if ((Style)value == (Style) Application.Current.Resources["ButtonRegular"])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
