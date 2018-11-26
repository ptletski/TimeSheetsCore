using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeSheetsCoreApp.DataTypes;

namespace TimeSheetsASPApp.Tests.ModelTests
{
    [TestClass]
    public class TestDateTimeExtensions
    {
        public TestDateTimeExtensions()
        {
        }

        [TestMethod]
        public void CreateNoBoundaryWeek()
        {
            WorkRecordDate testDate = new WorkRecordDate(2018, WorkRecordMonths.June, 6);
            WorkRecordDate mondayDate = WorkRecordMondayDate.FindMonday(testDate);
            DateTime dtMonday = new DateTime(mondayDate.Year, mondayDate.Month, mondayDate.Day);
            DateTime[] dateResults = DateTimeExtensions.DaysInWeek(dtMonday);

            // Validate results.
            // Expect: 6/4, 6/5, 6/6, 6/7, 6/8, 6/9, 6/10 [2018]
            int month = 6; 
            int day = 4;

            for (var index = 0; index < 7; index++)
            {
                Assert.IsTrue((month == dateResults[index].Month));
                Assert.IsTrue((day == dateResults[index].Day));
                day++;
            }
        }

        [TestMethod]
        public void CreateMonthBoundarySameYear()
        {
            WorkRecordDate testDate = new WorkRecordDate(2018, WorkRecordMonths.June, 1);
            WorkRecordDate mondayDate = WorkRecordMondayDate.FindMonday(testDate);
            DateTime dtMonday = new DateTime(mondayDate.Year, mondayDate.Month, mondayDate.Day);
            DateTime[] dateResults = DateTimeExtensions.DaysInWeek(dtMonday);

            // Validate results.
            // Expect: 5/28, 5/29, 5/30, 5/31, 6/1, 6/2, 6/3 [2018]
            int month = 5;
            int day = 28;

            for (var index = 0; index < 4; index++)
            {
                Assert.IsTrue((month == dateResults[index].Month));
                Assert.IsTrue((day == dateResults[index].Day));
                day++;
            }

            month = 6;
            day = 1;

            for (var index = 4; index < 7; index++)
            {
                Assert.IsTrue((month == dateResults[index].Month));
                Assert.IsTrue((day == dateResults[index].Day));
                day++;
            }
        }

        [TestMethod]
        public void CreateYearBoundaryMonthBoundary()
        {
            WorkRecordDate testDate = new WorkRecordDate(2019, WorkRecordMonths.January, 1);
            WorkRecordDate mondayDate = WorkRecordMondayDate.FindMonday(testDate);
            DateTime dtMonday = new DateTime(mondayDate.Year, mondayDate.Month, mondayDate.Day);
            DateTime[] dateResults = DateTimeExtensions.DaysInWeek(dtMonday);

            // Validate results.
            // Expect: 12/31/2018, 1/1, 1/2, 1/3, 1/4, 1/5, 1/6 [2019]
            int month = 12;
            int day = 31;

            Assert.IsTrue((month == dateResults[0].Month));
            Assert.IsTrue((day == dateResults[0].Day));

            month = 1;
            day = 1;

            for (var index = 1; index < 7; index++)
            {
                Assert.IsTrue((month == dateResults[index].Month));
                Assert.IsTrue((day == dateResults[index].Day));
                day++;
            }
        }
    }
}
