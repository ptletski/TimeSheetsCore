using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeSheetsCoreApp.DataTypes;

namespace TimeSheetsASPApp.Tests.ModelTests
{
    [TestClass]
    public class TestOneDayHoursAndBreak
    {
        public TestOneDayHoursAndBreak()
        {
        }

        [TestMethod]
        public void TestConstruction()
        {
            WorkRecordDate workDate = new WorkRecordDate(2018, WorkRecordMonths.September, 24);
            WorkRecordForDay workDay = new WorkRecordForDay(workDate);
            WorkRecordTime workStartTime = new WorkRecordTime(7, 15, WorkRecordAmOrPm.AM);
            WorkRecordTime workEndTime = new WorkRecordTime(4, 15, WorkRecordAmOrPm.PM);

            WorkRecordSegment segment = new WorkRecordSegment()
            {
                StartTime = workStartTime,
                EndTime = workEndTime
            };

            segment.AddBreak(15);
            segment.AddBreak(65);

            // Next test...
            Assert.AreEqual(segment.StartTime.Hour, 7);
            Assert.AreEqual(segment.StartTime.Minutes, 15);
            Assert.AreEqual(segment.StartTime.amORpm, WorkRecordAmOrPm.AM);

            // Next test...
            Assert.AreEqual(segment.EndTime.Hour, 4);
            Assert.AreEqual(segment.EndTime.Minutes, 15);
            Assert.AreEqual(segment.EndTime.amORpm, WorkRecordAmOrPm.PM);

            // Next test...
            Assert.AreEqual(segment.Breaks.Count, 2);
            Assert.AreEqual(segment.Breaks[0], 15);
            Assert.AreEqual(segment.Breaks[1], 65);
        }

        [TestMethod]
        public void TestBillableHours()
        {
            WorkRecordDate workDate = new WorkRecordDate(2018, WorkRecordMonths.September, 24);
            WorkRecordForDay workDay = new WorkRecordForDay(workDate);
            WorkRecordTime workStartTime = new WorkRecordTime(7, 15, WorkRecordAmOrPm.AM);
            WorkRecordTime workEndTime = new WorkRecordTime(4, 15, WorkRecordAmOrPm.PM);

            WorkRecordSegment segment = new WorkRecordSegment()
            {
                StartTime = workStartTime,
                EndTime = workEndTime
            };

            segment.StartTime = workStartTime;
            segment.EndTime = workEndTime;
            segment.AddBreak(15);
            segment.AddBreak(65);

            workDay.Add(segment);

            try
            {
                double billableHours = workDay.CalculateBillableHours();
                double testValue = (double)((540.0 - 15.0 - 65.0) / 60.0);
                double difference = Math.Abs(billableHours - testValue);

                bool testResult = (difference < 9.0E-15);

                Assert.IsTrue(testResult);
            }
            catch (Exception exception)
            {
                Assert.IsFalse(false, exception.Message);
            }
        }
    }
}
