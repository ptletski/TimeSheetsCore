using System;
using System.Collections.Generic;

namespace TimeSheetsCoreApp.DataTypes
{
    public class WorkRecordSegment
    {
        private WorkRecordDate _date;
        private WorkRecordRate _rate;
        private WorkRecordTime _startTime = null;
        private WorkRecordTime _endTime = null;
        private List<ushort> _breaks;

        // 1's based year, month, day.
        public WorkRecordSegment()
        {
            _breaks = new List<ushort>();
        }

        public WorkRecordTime StartTime
        {
            get => _startTime;
            set => _startTime = value;
        }

        public WorkRecordTime EndTime
        {
            get => _endTime;
            set => _endTime = value;
        }

        public WorkRecordRate Rate
        {
            get => _rate;
            set => _rate = value;
        }

        public WorkRecordDate Date
        {
            get => _date;
            set => _date = value;
        }

        public List<ushort> Breaks => _breaks;

        public void AddBreak(ushort durationInMinutes)
        {
            _breaks.Add(durationInMinutes);
        }

        public string BreaksToString()
        {
            string result = "";
            int count = _breaks.Count;
            int index = 0;

            foreach(var b in _breaks)
            {
                result += b.ToString();
                index++;

                if (index < count)
                {
                    result += ", ";
                }
            }


            return result;
        }

        public double CalculateBillableHours()
        {
            if (_startTime == null)
                return 0;

            if (_endTime == null)
            {
                throw new MissingMemberException("Incomplete time record");
            }

            // t1 is earlier than t2: < 0, that's OK
            // t1 is later than t2: > 0, that's BAD
            // t1 is equal to t2: 0, don't care
            //                                                            t1         t2
            bool isStartTimeLaterThanEndTime = WorkRecordTime.Compare(_startTime, _endTime) > 0;

            if (isStartTimeLaterThanEndTime == true)
            {
                throw new ArgumentException("End-time is earlier than Start-time");
            }

            DateTime endTime = new DateTime(
                _date.Year,
                _date.Month,
                _date.Day,
                _endTime.HourAs24HourConvention(),
                _endTime.Minutes,
                0);

            DateTime startTime = new DateTime(
                _date.Year,
                _date.Month,
                _date.Day,
                _startTime.HourAs24HourConvention(),
                _startTime.Minutes,
                0);

            // now add up break minutes and subtract from timespan.

            var totalBreakMinutes = 0;

            for (int index = 0; index < _breaks.Count; index++)
            {
                totalBreakMinutes += _breaks[index];
            }

            TimeSpan tsPresent = endTime - startTime;
            var workedMinutes = tsPresent.TotalMinutes - totalBreakMinutes;
            TimeSpan tsWorkedHours = TimeSpan.FromMinutes(workedMinutes);

            if (totalBreakMinutes > tsWorkedHours.TotalMinutes)
            {
                throw new Exception("More break time than work time");
            }

            return tsWorkedHours.TotalHours;
        }
    }
}
