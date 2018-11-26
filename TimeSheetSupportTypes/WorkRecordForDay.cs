using System;
using System.Collections;
using System.Collections.Generic;

namespace TimeSheetsCoreApp.DataTypes
{
    public class WorkRecordForDay : IEnumerable<WorkRecordSegment>
    {
        private WorkRecordDate _date;
        private List<WorkRecordSegment> _segments;

        // 1's based year, month, day.
        public WorkRecordForDay(WorkRecordDate date)
        {
            _date = date;
            _segments = new List<WorkRecordSegment>();
        }

        public WorkRecordDate Date
        {
            get => _date;
            set => _date = value;
        }

        public void Add(WorkRecordSegment segment)
        {
            segment.Date = _date;

            _segments.Add(segment);
        }

        public double CalculateBillableHours()
        {
            double billableHours = 0;

            foreach(var segment in _segments)
            {
                billableHours += segment.CalculateBillableHours();
            }

            return billableHours;
        }


        override public string ToString()
        {
            return _date.ToString();
        }

        public IEnumerator<WorkRecordSegment> GetEnumerator()
        {
            return _segments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _segments.GetEnumerator();
        }
    }
}
