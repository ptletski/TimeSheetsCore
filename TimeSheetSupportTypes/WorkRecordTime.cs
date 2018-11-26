using System;

namespace TimeSheetsCoreApp.DataTypes
{
    public class WorkRecordTime
    {
        // t1 is earlier than t2: < 0
        // t1 is later than t2: > 0
        // t1 is equal to t2: 0
        public static int Compare(WorkRecordTime t1, WorkRecordTime t2)
        {
            int result = 0;
            int t1_24 = t1.HourAs24HourConvention();
            int t2_24 = t2.HourAs24HourConvention();
            
            if (t1_24 < t2_24)
            {
                return -1;
            }

            if (t1_24 > t2_24)
            {
                return 1;
            }

            if (t1.Minutes < t2.Minutes)
            {
                return -1;
            }

            if (t1.Minutes > t2.Minutes)
            {
                return 1;
            }

            return result;
        }

        private int _hour = 0;
        private int _minutes = 0;
        readonly private WorkRecordAmOrPm _amORpm = WorkRecordAmOrPm.AM;

        public WorkRecordTime(int hour, int minutes, WorkRecordAmOrPm amORpm)
        {
            if ((hour < 1) || (hour > 12))
            {
                throw new ArgumentOutOfRangeException(); // hour parameter is of range
            }

            if ((minutes < 1) || (minutes > 60))
            {
                throw new ArgumentOutOfRangeException(); // minutes parameter is our of range
            }

            _hour = hour;
            _minutes = minutes;
            _amORpm = amORpm;
        }

        public int Hour => _hour;
        public int Minutes => _minutes;
        public WorkRecordAmOrPm amORpm => _amORpm;

        override public string ToString()
        {
            string ampmString = (_amORpm == WorkRecordAmOrPm.AM) ? "AM" : "PM";

            return $"{Hour}:{Minutes} {ampmString}";
        }

        public int HourAs24HourConvention()
        {
            int convertedHour = 0;

            if (_amORpm == WorkRecordAmOrPm.AM)
            {
                convertedHour = (_hour == 12) ? 0 : _hour;
            }
            else
            {
                convertedHour = (_hour == 12) ? 12 : _hour + 12;
            }

            return convertedHour;
        }
    }
}
