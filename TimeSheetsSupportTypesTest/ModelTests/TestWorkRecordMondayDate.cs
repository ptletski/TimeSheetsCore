using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeSheetsCoreApp.DataTypes;

namespace TimeSheetsASPApp.Tests.ModelTests
{
    [TestClass]
    public class TestWorkRecordMondayDate
    {
        public TestWorkRecordMondayDate()
        {
        }

        [TestMethod]
        public void SameMonthStartInMiddleOfWeek()
        {
            WorkRecordDate testDate = new WorkRecordDate(2018, WorkRecordMonths.June, 6);
            WorkRecordDate mondayDate = WorkRecordMondayDate.FindMonday(testDate);
            bool testResult = (mondayDate.Year == 2018) && (mondayDate.Month == WorkRecordMonths.June) && (mondayDate.Day == 4);
            Assert.IsTrue(testResult);
        }

        [TestMethod]
        public void BorderMonthsSameYear()
        {
            WorkRecordDate testDate = new WorkRecordDate(2018, WorkRecordMonths.June, 1);
            WorkRecordDate mondayDate = WorkRecordMondayDate.FindMonday(testDate);
            bool testResult = (mondayDate.Year == 2018) && (mondayDate.Month == WorkRecordMonths.May) && (mondayDate.Day == 28);
            Assert.IsTrue(testResult);
        }

        [TestMethod]
        public void BorderMonthsDifferentYear()
        {
            WorkRecordDate testDate = new WorkRecordDate(2019, WorkRecordMonths.January, 1);
            WorkRecordDate mondayDate = WorkRecordMondayDate.FindMonday(testDate);
            bool testResult = (mondayDate.Year == 2018) && (mondayDate.Month == WorkRecordMonths.December) && (mondayDate.Day == 31);
            Assert.IsTrue(testResult);
        }
    }
}
