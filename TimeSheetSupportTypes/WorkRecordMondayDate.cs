using System;

namespace TimeSheetsCoreApp.DataTypes
{
    public static class WorkRecordMondayDate
    {
        public static WorkRecordDate FindMonday(WorkRecordDate startDate)
        {
            DateTime startDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            DateTime beginDateTime = DateTimeExtensions.StartOfWeek(startDateTime, DayOfWeek.Monday);
            WorkRecordDate result = new WorkRecordDate(beginDateTime.Year, beginDateTime.Month, beginDateTime.Day);

            return result;
        }
    }
}
