using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OnDijon.Common.Utils.Converters
{
    public class ValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int nombrePlacesLibres = (int)value;

            if (nombrePlacesLibres < 5)
                return Color.Red;
            else
                return Color.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
