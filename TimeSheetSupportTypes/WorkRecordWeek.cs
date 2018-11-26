using System;
using System.Collections;
using System.Collections.Generic;

namespace TimeSheetsCoreApp.DataTypes
{
    public class WorkRecordWeek : IEnumerable<WorkRecordForDay>
    {
        private List<WorkRecordForDay> _daysWorked = null;

        public WorkRecordWeek(WorkRecordDate mondayDate)
        {
            _daysWorked = new List<WorkRecordForDay>();

            List<WorkRecordDate> datesOfWeek = DefineWeek(mondayDate);
            _daysWorked = CreateWorkRecordForDayObjects(datesOfWeek);
        }

        public void SetHoursForDay(WorkRecordDaysOfWeek.Days dayToSet, WorkRecordTime startTime, WorkRecordTime endTime, ushort[] breaks)
        {
            WorkRecordForDay workDay = _daysWorked[WorkRecordDaysOfWeek.AsIndex(dayToSet)];
            WorkRecordSegment segment = new WorkRecordSegment()
            {
                StartTime = startTime,
                EndTime = endTime
            };

            foreach(ushort breakTime in breaks)
            {
                segment.AddBreak(breakTime);
            }

            workDay.Add(segment);
        }

        public WorkRecordForDay Day(WorkRecordDaysOfWeek.Days day)
        {
            int dayIndex = (int)day;
            return _daysWorked[dayIndex];
        }

        public string[] GetDateStrings()
        {
            string[] dateStrings = new string[7];
            int index = 0;

            foreach(WorkRecordForDay workDay in _daysWorked)
            {
                dateStrings[index] = workDay.ToString();
                index++;
            }

            return dateStrings;
        }

        private List<WorkRecordForDay> CreateWorkRecordForDayObjects(List<WorkRecordDate> datesOfWeek)
        {
            List<WorkRecordForDay> week = new List<WorkRecordForDay>();

            foreach(WorkRecordDate workedDay in datesOfWeek)
            {
                week.Add(new WorkRecordForDay(workedDay));
            }

            return week;
        }

        private List<WorkRecordDate> DefineWeek(WorkRecordDate mondayDate)
        {
            DateTime dtMonday = new DateTime(mondayDate.Year, mondayDate.Month, mondayDate.Day);
            DateTime[] dateResults = DateTimeExtensions.DaysInWeek(dtMonday);
            List<WorkRecordDate> week = new List<WorkRecordDate>();

            foreach(DateTime dt in dateResults)
            {
                week.Add(new WorkRecordDate(dt.Year, dt.Month, dt.Day));
            }

            return week;
        }

        public double CalculateBillableHoursForWeek()
        {
            double billableHours = 0;

            foreach (WorkRecordForDay workDay in _daysWorked)
            {
                billableHours += workDay.CalculateBillableHours();
            }

            return billableHours;
        }

        // ======================================================
        //
        // IEnumerator, IEnumerable Section
        //
        // ======================================================
        public IEnumerator<WorkRecordForDay> GetEnumerator()
        {
            return _daysWorked.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _daysWorked.GetEnumerator();
        }
    }
}
