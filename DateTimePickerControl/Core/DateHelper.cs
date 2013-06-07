using System;
using System.Collections.Generic;
using AC.AvalonControlsLibrary.Controls;
using AC.AvalonControlsLibrary.Exception;


namespace AC.AvalonControlsLibrary.Core
{
    /// <summary>
    /// Helper class for dates
    /// </summary>
    public class DateHelper
    {
        //the list of months in a year
        public static readonly string[] Months = new string[]
        {
            "1月",
            "2月",
            "3月",
            "4月",
            "5月",
            "6月",
            "7月",
            "8月",
            "9月",
            "10月",
            "11月",
            "12月"
        };

        //caches the days in a dict where the key is the number of days
        //the key is a hash of month, year
        readonly Dictionary<KeyValuePair<int, int>, DayCell[]> daysArrays = new Dictionary<KeyValuePair<int, int>, DayCell[]>();

        /// <summary>
        /// Generates an array of days in a month
        /// </summary>
        /// <param name="month">The month to get the days for </param>
        /// <param name="year">The year to get the days for </param>
        /// <param name="maxDate">The max date to generate enabled cells</param>
        /// <param name="minDate">The min date to generate enabled cells</param>
        /// <returns>Returns an int array full of days in a month</returns>
        public DayCell[] GetDaysOfMonth(int month, int year, 
            DateTime minDate, DateTime maxDate)
        {
            KeyValuePair<int, int> key = new KeyValuePair<int, int>(month, year);
            int daysCount = DateTime.DaysInMonth(year, month);

            DayCell[] days = null;

            if (daysArrays.ContainsKey(key))
            {
                days = daysArrays[key];
                foreach (DayCell item in days)
                    item.IsEnabled = IsDateWithinRange(minDate, maxDate, item);
            }
            else
            {
                days = new DayCell[daysCount];

                for (int i = 0; i < days.Length; i++)
                {
                    DayCell item = new DayCell(i + 1, month, year);
                    item.IsEnabled = IsDateWithinRange(minDate, maxDate, item);
                    days[i] = item;
                }

                daysArrays[key] = days;//cache the array
            }

            return days;
        }

        #region Helper Methods
        /// <summary>
        /// Checks if the specified date is greater than 
        /// </summary>
        /// <param name="minDate">The min date</param>
        /// <param name="maxDate">The max date</param>
        /// <param name="cell">The cell to check</param>
        /// <returns>Returns true if the cell is greater</returns>
        public static bool IsDateWithinRange(DateTime minDate, DateTime maxDate, DayCell cell)
        {
            long ticks = new DateTime(cell.YearNumber, cell.MonthNumber, cell.DayNumber).Ticks;
            return ticks >= minDate.Ticks && ticks <= maxDate.Ticks;
        }

        /// <summary>
        /// Gets the day of week for a specific date
        /// </summary>
        /// <param name="year">The year of the date</param>
        /// <param name="month">The month of the date</param>
        /// <param name="day">The day of the date</param>
        /// <returns>Returns the day of week</returns>
        public static DayOfWeek GetDayOfWeek(int year, int month, int day)
        {
            return new DateTime(year, month, day).DayOfWeek;
        }

        /// <summary>
        /// Gets a string that represents the string for the current month
        /// </summary>
        /// <param name="month">a number from 1 to 12</param>
        /// <returns>Returns the string that represents the month</returns>
        /// <exception cref="ArgumentException">Thrown if the month number is less than 1 or greater than 12</exception>
        public static string GetMonthDisplayName(int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException(ExceptionStrings.INVALID_MONTH_NUMBER, "month");

            return Months[month - 1];
        }

        /// <summary>
        /// Moves one month forward
        /// </summary>
        /// <param name="currentMonth">The current month</param>
        /// <param name="currentYear">The current year</param>
        /// <param name="monthToGetNext">The next month</param>
        /// <param name="yearTogetNext">The relative year for the new month</param>
        public static void MoveMonthForward(int currentMonth, int currentYear,
            out int monthToGetNext, out int yearTogetNext)
        {
            monthToGetNext = currentMonth;
            yearTogetNext = currentYear;
            if (monthToGetNext < 12)
                monthToGetNext++;
            else//move a year forward
            {
                yearTogetNext++;
                monthToGetNext = 1;
            }
        }
        /// <summary>
        /// Move one month back
        /// </summary>
        /// <param name="currentMonth">The current month</param>
        /// <param name="currentYear">The current year</param>
        /// <param name="monthToGetPrevious">The previous month</param>
        /// <param name="yearToGetPrevious">The relative year for the new month</param>
        public static void MoveMonthBack(int currentMonth, int currentYear,
            out int monthToGetPrevious, out int yearToGetPrevious)
        {
            monthToGetPrevious = currentMonth;
            yearToGetPrevious = currentYear;
            if (monthToGetPrevious > 1)
                monthToGetPrevious--;
            else // move one year down
            {
                yearToGetPrevious--;
                monthToGetPrevious = 12;
            }
        }

        #endregion
    }

    public class Month
    {
        public int Number;
        public string Name;
    }

    public static class MonthsDefines
    {
        public static List<Month> Months = new List<Month>()
        {
            new Month(){Number = 1, Name = "1月"},
            new Month(){Number = 2, Name = "2月"},
            new Month(){Number = 3, Name = "3月"},
            new Month(){Number = 4, Name = "4月"},
            new Month(){Number = 5, Name = "5月"},
            new Month(){Number = 6, Name = "6月"},
            new Month(){Number = 7, Name = "7月"},
            new Month(){Number = 8, Name = "8月"},
            new Month(){Number = 9, Name = "9月"},
            new Month(){Number = 10, Name = "10月"},
            new Month(){Number = 11, Name = "11月"},
            new Month(){Number = 12, Name = "12月"},
        };

        public static string GetMonthString(int number)
        {
            return Months.Find(item => item.Number == number).Name;
        }

        public static int GetMonthNumber(string str)
        {
            return Months.Find(item => item.Name == str).Number;
        }
    }
}
