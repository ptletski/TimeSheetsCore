using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using TimeSheetsCoreApp.DataTypes;

namespace TimeSheetsCoreApp.ViewModels
{
    struct TestDataTime
    {
        public int hour;
        public int minutes;
        public WorkRecordAmOrPm amORpm;

        public TestDataTime(int hour, int minutes, WorkRecordAmOrPm a)
        {
            this.hour = hour;
            this.minutes = minutes;
            this.amORpm = a;
        }
    }

    public class TimeSheetViewModel
    { 
        private DateTime _startWeekDate;


        public TimeSheetViewModel(string dayOfDateInWeek, UserDetails userDetails)
        {
            _startWeekDate = FindMondayForWeekFromDate(dayOfDateInWeek);

            FirstName = userDetails.FirstName;
            LastName = userDetails.LastName;
            UserId = userDetails.UserId;

            TimeSheet = DefineTimeSheet();

            Rate = WorkRecordRate.Standard;
        }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName 
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public WorkRecordWeek TimeSheet { get; set; }

        public bool IsSubmitted { get; set; }

        public WorkRecordRate Rate { get; set; }

        public double BillableHours { get; set; }

        public string[] DateStrings()
        {
            return TimeSheet.GetDateStrings();
        }

        public string MondayDate
        {
            get
            {
                return _startWeekDate.ToShortDateString();
            }
        }

        private DateTime FindMondayForWeekFromDate(string date)
        {
            DateTime dateTime = ParseDateString(date);
            WorkRecordDate monday = WorkRecordMondayDate.FindMonday(new WorkRecordDate(dateTime.Year, dateTime.Month, dateTime.Day));

            return new DateTime(monday.Year, monday.Month, monday.Day);
        }

        private DateTime ParseDateString(string dateString)
        {
            string format = "d";
            DateTime result = new DateTime();
            CultureInfo provider = CultureInfo.InvariantCulture;

            try
            {
                result = DateTime.ParseExact(dateString, format, provider);
            }
            catch (FormatException)
            {
                Debug.Assert(false, $"dateString:{dateString} is not in the correct format.");
            }

            return result;
        }

        private WorkRecordWeek DefineTimeSheet()
        {
            object[] tests = new object[15];

            tests[0] = new TestDataTime(0, 0, WorkRecordAmOrPm.AM);
            tests[1] = new TestDataTime(0, 0, WorkRecordAmOrPm.AM);
            tests[2] = new ushort[1] { 0};
            tests[3] = new TestDataTime(0, 0, WorkRecordAmOrPm.AM);
            tests[4] = new TestDataTime(0, 0, WorkRecordAmOrPm.AM);
            tests[5] = new ushort[1] { 0 };
            tests[6] = new TestDataTime(0, 0, WorkRecordAmOrPm.AM);
            tests[7] = new TestDataTime(0, 0, WorkRecordAmOrPm.AM);
            tests[8] = new ushort[1] { 0 };
            tests[9] = new TestDataTime(0, 0, WorkRecordAmOrPm.AM);
            tests[10] = new TestDataTime(0, 0, WorkRecordAmOrPm.AM);
            tests[11] = new ushort[1] { 0 };
            tests[12] = new TestDataTime(0, 0, WorkRecordAmOrPm.AM);
            tests[13] = new TestDataTime(0, 0, WorkRecordAmOrPm.AM);
            tests[14] = new ushort[1] { 0 };

            WorkRecordWeek workWeek = CreateWorkRecordWeek(_startWeekDate.Year, _startWeekDate.Month, _startWeekDate.Day);

            //AddHoursForWeek(workWeek, tests);

            BillableHours = workWeek.CalculateBillableHoursForWeek();

            IsSubmitted = false;

            return workWeek;
        }

        private void AddHoursForWeek(WorkRecordWeek workWeek, object[] testData)
        {
            int index = 0;
            TestDataTime testInfo;
            WorkRecordDaysOfWeek dayIterator = new WorkRecordDaysOfWeek();

            foreach (WorkRecordDaysOfWeek.Days workDay in dayIterator)
            {
                if (index >= testData.Length)
                {
                    break;
                }

                testInfo = (TestDataTime)testData[index];
                WorkRecordTime workStartTime = new WorkRecordTime(testInfo.hour, testInfo.minutes, testInfo.amORpm);

                index++;

                testInfo = (TestDataTime)testData[index];
                WorkRecordTime workEndTime = new WorkRecordTime(testInfo.hour, testInfo.minutes, testInfo.amORpm);

                index++;

                workWeek.SetHoursForDay(workDay, workStartTime, workEndTime, (ushort[])testData[index]);

                index++;
            }
        }

        private WorkRecordWeek CreateWorkRecordWeek(int year, int month, int day)
        {
            var testDate = new WorkRecordDate(year, month, day);
            var mondayDate = WorkRecordMondayDate.FindMonday(testDate);
            return new WorkRecordWeek(mondayDate);
        }
    }
}
