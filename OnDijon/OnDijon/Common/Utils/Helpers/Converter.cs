using System;

namespace OnDijon.Common.Utils.Helpers
{
    public class Converter
    {
        public class DateTimeConverterTool
        {
            public static DateTime GetDayOfWeek(DateTime date, DayOfWeek dayOfWeek)
            {
                return date.AddDays(dayOfWeek - date.DayOfWeek);
            }
        }
    }
}
