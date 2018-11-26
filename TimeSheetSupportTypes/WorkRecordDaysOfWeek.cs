using System;
using System.Collections;

namespace TimeSheetsCoreApp.DataTypes
{
    public class WorkRecordDaysOfWeek : IEnumerator, IEnumerable
    {
        public static int Monday = 0;
        public static int Tuesday = 1;
        public static int Wednesday = 2;
        public static int Thursday = 3;
        public static int Friday = 4;
        public static int Saturday = 5;
        public static int Sunday = 6;

        public enum Days { Monday = 0, Tuesday = 1, Wednesday = 2, Thursday = 3, Friday = 4, Saturday = 5, Sunday = 6 };

        static public int AsIndex(WorkRecordDaysOfWeek.Days days)
        {
            return (int)days;
        }

        private int[] _days;
        private int _index;

        public WorkRecordDaysOfWeek()
        {
            _days = new int[7];

            _days[0] = WorkRecordDaysOfWeek.Monday;
            _days[1] = WorkRecordDaysOfWeek.Tuesday;
            _days[2] = WorkRecordDaysOfWeek.Wednesday;
            _days[3] = WorkRecordDaysOfWeek.Thursday;
            _days[4] = WorkRecordDaysOfWeek.Friday;
            _days[5] = WorkRecordDaysOfWeek.Saturday;
            _days[6] = WorkRecordDaysOfWeek.Sunday;

            Reset();
        }

        public bool MoveNext()
        {
            _index++;
            return (_index < _days.Length);
        }

        public void Reset()
        {
            _index = -1;
        }

        public object Current
        {
            get
            {
                try
                {
                    return _days[_index];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }
    }
}
