using System;
using System.Globalization;

namespace OnDijon.Common.Utils.Tools
{
    public static class DateTimeTool
    {

        public static int GetWeekNumber(DateTime now)
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            int weekNumber = ci.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNumber;
        }

        public static int GetDayOfWeekOffset(DateTime date, CultureInfo cultureInfo)
        {
            return ((int)(date.DayOfWeek - cultureInfo.DateTimeFormat.FirstDayOfWeek) + 7) % 7;

        }
    }
}
