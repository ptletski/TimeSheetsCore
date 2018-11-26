using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeSheetsCoreApp.DataTypes;

namespace TimeSheetsASPApp.Tests.ModelTests
{
    [TestClass]
    public class TestWorkRecordDate
    {
        public TestWorkRecordDate()
        {
        }

        [TestMethod]
        public void TestWorkRecordDateConstructionSuccess()
        {
            WorkRecordDate workDate = new WorkRecordDate(2018, WorkRecordMonths.September, 24);

            bool testResult = ((workDate.Year == 2018) && (workDate.Month == WorkRecordMonths.September) && (workDate.Day == 24));
            Assert.IsTrue(testResult); // Test WorRecordDate Construction Failure
        }

        [TestMethod]
        public void TestWorkRecordDateConstructionMonthFailure()
        {
            bool isException = false;

            try
            {
                WorkRecordDate workDate = new WorkRecordDate(2018, 1234, 24);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
                isException = true;
            }

            Assert.IsTrue(isException); // Test WorRecordDate Range Failure
        }
    }
}
