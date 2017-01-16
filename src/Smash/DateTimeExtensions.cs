using System;
using System.Globalization;

namespace Smash
{
    public static class DateExtensions
    {
        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        public static int GetIso8601WeekOfYear(this DateTime time)
        {
            var cal = CultureInfo.InvariantCulture.Calendar;

            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it’ll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = cal.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return cal.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static string GetSeason(this DateTime date)
        {
            var weekNo = new GregorianCalendar(GregorianCalendarTypes.Localized).GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var year = date.Year;

            if (weekNo > 30)
            {
                return $"{year}/{year + 1}";
            }

            return $"{year - 1}/{year}";
        }
    }
}