using Newtonsoft.Json.Converters;

namespace OnDijon.Common.Utils.Converters
{
    public class DateTimeJsonConverter : IsoDateTimeConverter
    {
        public DateTimeJsonConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
