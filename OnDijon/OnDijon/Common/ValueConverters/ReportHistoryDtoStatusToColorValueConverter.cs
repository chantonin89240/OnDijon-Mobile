using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace OnDijon.Common.ValueConverters
{
    public class ReportHistoryDtoStatusToColorValueConverter : IValueConverter
    {
     

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string Status)
                switch (Status)
                {
                    case "Créé":
                        return (Color)App.Current.Resources["BadgeColorRed"];
                    case "En cours de traitement":
                        return (Color)App.Current.Resources["BadgeColorYellow"];
                    case "Clôture":
                        return (Color)App.Current.Resources["BadgeColorGreen"];
                    case "Annulation":
                        return (Color)App.Current.Resources["BadgeColorLightGray"];
                    default:
                        return (Color)App.Current.Resources["BadgeColorLightGray"];
                }

            return (Color)App.Current.Resources["BadgeColorLightGray"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}