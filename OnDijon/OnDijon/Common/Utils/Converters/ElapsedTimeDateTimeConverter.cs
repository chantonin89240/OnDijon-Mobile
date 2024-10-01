using System;
using Xamarin.Forms;

namespace OnDijon.Common.Utils.Converters
{
    public class ElapsedTimeDateTimeConverter : IValueConverter
    {
        private static DateTime now = DateTime.Now;
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            int elapsedTime = int.Parse((now-(DateTime)value).ToString("%d"));
            //si + de 24h
            if (elapsedTime != 0)
            {
                return "il y a " + elapsedTime + " jour" + (elapsedTime == 1 ? "" : "s");

            }
            else
            {
                elapsedTime = int.Parse((now-(DateTime)value).ToString("%h"));
                //si - d'une heure
                if (elapsedTime == 0)
                {
                    elapsedTime = int.Parse((now-(DateTime)value).ToString("%m"));
                    if (elapsedTime <= 0)
                    {
                        return "il y a moins d'une minute";
                    }
                    else
                    {
                        return "il y a " + elapsedTime + " minute" + (elapsedTime == 1 ? "" : "s");
                    }
                }
                else
                {
                    return "il y a " + elapsedTime + " heure" + (elapsedTime == 1 ? "" : "s");
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
