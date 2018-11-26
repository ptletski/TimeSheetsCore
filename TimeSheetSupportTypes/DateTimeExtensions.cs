using System;
using System.Collections.Generic;

namespace TimeSheetsCoreApp.DataTypes
{
    public static class DateTimeExtensions
    {
        // Get the DateTime for the start of the week.
        public static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        // Get 7 properly dated days from the start of the week (provided)
        public static DateTime[] DaysInWeek(DateTime startOfWeek)
        {
            DateTime[] week = new DateTime[7];

            week[0] = startOfWeek;

            for (var dayIndex = 1; dayIndex < 7; dayIndex++)
            {
                week[dayIndex] = startOfWeek.AddDays(dayIndex);
            }

            return week;
        }
    }
}
