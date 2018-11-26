using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeSheetsCoreApp.DataTypes;

namespace TimeSheetsASPApp.Tests.ModelTests
{
    struct TestDataTime
    {
        public int hour;
        public int minutes;
        public WorkRecordAmOrPm amORpm;

        public TestDataTime(int hour, int minutes, WorkRecordAmOrPm ampm)
        {
            this.hour = hour;
            this.minutes = minutes;
            this.amORpm = ampm;
        }
    }

    [TestClass]
    public class TestWorkRecordWeek
    {
        public TestWorkRecordWeek()
        {
        }

        [TestMethod]
        public void NoBoundariesMiddleWeek()
        {
            var expected = new int[7,3] {{2018, 6, 4}, {2018, 6, 5}, {2018, 6, 6}, {2018, 6, 7}, {2018, 6, 8}, {2018, 6, 9}, {2018, 6, 10}};
            WorkRecordWeek workWeek = CreateWorkRecordWeek(2018, WorkRecordMonths.June, 6);
            EvaluateWorkRecordDaysOfWeek(workWeek, expected);
        }

        [TestMethod]
        public void MonthBoundarySameYear()
        {
            var expected = new int[7, 3] { { 2018, 5, 28 }, { 2018, 5, 29 }, { 2018, 5, 30 }, { 2018, 5, 31 }, { 2018, 6, 1 }, { 2018, 6, 2 }, { 2018, 6, 3 } };
            WorkRecordWeek workWeek = CreateWorkRecordWeek(2018, WorkRecordMonths.June, 1);
            EvaluateWorkRecordDaysOfWeek(workWeek, expected);
        }

        [TestMethod]
        public void YearBoundaryMonthBoundary()
        {
            var expected = new int[7, 3] { { 2018, 12, 31 }, { 2019, 1, 1 }, { 2019, 1, 2 }, { 2019, 1, 3 }, { 2019, 1, 4 }, { 2019, 1, 5 }, { 2019, 1, 6 } };
            WorkRecordWeek workWeek = CreateWorkRecordWeek(2019, WorkRecordMonths.January, 1);
            EvaluateWorkRecordDaysOfWeek(workWeek, expected);
        }

        [TestMethod]
        public void CalculateBillableHoursForWeek()
        {
            object[] tests = new object[15];

            tests[0] = new TestDataTime(7, 15, WorkRecordAmOrPm.AM);
            tests[1] = new TestDataTime(4, 15, WorkRecordAmOrPm.PM);
            tests[2] = new ushort[1] { 0 };
            tests[3] = new TestDataTime(7, 15, WorkRecordAmOrPm.AM);
            tests[4] = new TestDataTime(4, 15, WorkRecordAmOrPm.PM);
            tests[5] = new ushort[1] { 0 };
            tests[6] = new TestDataTime(7, 15, WorkRecordAmOrPm.AM);
            tests[7] = new TestDataTime(4, 15, WorkRecordAmOrPm.PM);
            tests[8] = new ushort[1] { 0 };
            tests[9] = new TestDataTime(7, 15, WorkRecordAmOrPm.AM);
            tests[10] = new TestDataTime(4, 15, WorkRecordAmOrPm.PM);
            tests[11] = new ushort[1] { 0 };
            tests[12] = new TestDataTime(7, 15, WorkRecordAmOrPm.AM);
            tests[13] = new TestDataTime(4, 15, WorkRecordAmOrPm.PM);
            tests[14] = new ushort[1] { 0 };

            WorkRecordWeek workWeek = CreateWorkRecordWeek(2018, WorkRecordMonths.October, 8);

            DefineHoursForWeek(workWeek, tests);

            double billableHours = workWeek.CalculateBillableHoursForWeek();

            double testValue = (double)(45.0);
            double difference = Math.Abs(billableHours - testValue);

            bool testResult = (difference < 9.0E-15);
            Assert.IsTrue(testResult);
        }

        private void DefineHoursForWeek(WorkRecordWeek workWeek, object[] testData)
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
                WorkRecordTime workStartTime = new WorkRecordTime(testInfo.hour , testInfo.minutes, testInfo.amORpm);

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
            var testDate  = new WorkRecordDate(year, month, day);
            var mondayDate = WorkRecordMondayDate.FindMonday(testDate);
            return new WorkRecordWeek(mondayDate);
        }

        private bool EvaluateWorkRecordDaysOfWeek(WorkRecordWeek workWeek, int[,] expected)
        {
            int expectedIndex = 0;

            foreach(WorkRecordForDay workRecord in workWeek)
            {
                WorkRecordDate date = workRecord.Date;

                Assert.IsTrue((date.Year == expected[expectedIndex, 0]));
                Assert.IsTrue((date.Month == expected[expectedIndex, 1]));
                Assert.IsTrue((date.Day == expected[expectedIndex, 2]));

                expectedIndex++;
            }

            return false;
        }
    }
}
