using System;

namespace TimeSheetsCoreApp.DataTypes
{
    public class WorkRecordDate
    {
        public WorkRecordDate(int year, int month, int day)
        {
            if ((month < 1) || (month > 12))
            {
                throw new ArgumentOutOfRangeException(nameof(month));
            }

            if ((day < 1) || (day > 31))
            {
                throw new ArgumentOutOfRangeException(nameof(day));
            }

            Year = year;
            Month = month;
            Day = day;
        }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        override public string ToString()
        {
            return String.Format(@"{0,2:D2}/{1,2:D2}/{2,4:D4}", Month, Day, Year);
        }
    }
}
