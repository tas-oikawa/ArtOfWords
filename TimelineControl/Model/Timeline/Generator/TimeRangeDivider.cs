using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimelineControl.Model
{
    public enum TimeRangeDivideKind
    {
        Sec30,
        Min1,
        Min2,
        Min5,
        Min15,
        Min30,
        Hour1,
        Hour2,
        Hour4,
        Hour8,
        Day1,
        Day2,
        MonthHalf,
    }

    public class TimeRangeDivider
    {
        public static TimeRangeDivideKind GetKind(TimeRange range)
        {
            TimeSpan span = new TimeSpan(range.EndDateTime.Ticks - range.StartDateTime.Ticks);

            if (span.TotalDays > 62)
            {
                return TimeRangeDivideKind.MonthHalf;
            }

            if (span.TotalDays > 14)
            {
                return TimeRangeDivideKind.Day2;
            }

            if (span.TotalDays > 7)
            {
                return TimeRangeDivideKind.Day1;
            }

            if (span.TotalHours > 96)
            {
                return TimeRangeDivideKind.Hour8;
            }

            if (span.TotalHours > 48)
            {
                return TimeRangeDivideKind.Hour4;
            }

            if (span.TotalHours > 24)
            {
                return TimeRangeDivideKind.Hour2;
            }

            if (span.TotalHours > 12)
            {
                return TimeRangeDivideKind.Hour1;
            }

            if (span.TotalHours > 6)
            {
                return TimeRangeDivideKind.Min30;
            }

            if(span.TotalHours > 3)
            {
                return TimeRangeDivideKind.Min15;
            }

            if(span.TotalMinutes > 60)
            {
                return TimeRangeDivideKind.Min5;
            }

            if (span.TotalMinutes > 30)
            {
                return TimeRangeDivideKind.Min2;
            }
            if (span.TotalMinutes > 15)
            {
                return TimeRangeDivideKind.Min1;
            }

            return TimeRangeDivideKind.Sec30;
        }
        
        public static TimeSpan GetTimeSpan(TimeRangeDivideKind kind)
        {
            switch (kind)
            {
                case TimeRangeDivideKind.Sec30:
                    return new TimeSpan(0,0,30);
                case TimeRangeDivideKind.Min1:
                    return new TimeSpan(0,1,0);
                case TimeRangeDivideKind.Min2:
                    return new TimeSpan(0, 2, 0);
                case TimeRangeDivideKind.Min5:
                    return new TimeSpan(0, 5, 0);
                case TimeRangeDivideKind.Min15:
                    return new TimeSpan(0, 15, 0);
                case TimeRangeDivideKind.Min30:
                    return new TimeSpan(0, 30, 0);
                case TimeRangeDivideKind.Hour1:
                    return new TimeSpan(1, 0, 0);
                case TimeRangeDivideKind.Hour2:
                    return new TimeSpan(2, 0, 0);
                case TimeRangeDivideKind.Hour4:
                    return new TimeSpan(4, 0, 0);
                case TimeRangeDivideKind.Hour8:
                    return new TimeSpan(8,0,0);
                case TimeRangeDivideKind.Day1:
                    return new TimeSpan(1, 0, 0, 0);
                case TimeRangeDivideKind.Day2:
                    return new TimeSpan(2, 0, 0, 0);
            }

            return new TimeSpan();
        }

        public static bool IsDividableKind(TimeRangeDivideKind kind)
        {
            switch (kind)
            {
                case TimeRangeDivideKind.Sec30:
                case TimeRangeDivideKind.Min1:
                case TimeRangeDivideKind.Min2:
                case TimeRangeDivideKind.Min5:
                case TimeRangeDivideKind.Min15:
                case TimeRangeDivideKind.Min30:
                case TimeRangeDivideKind.Hour1:
                case TimeRangeDivideKind.Hour2:
                case TimeRangeDivideKind.Hour4:
                case TimeRangeDivideKind.Hour8:
                case TimeRangeDivideKind.Day1:
                case TimeRangeDivideKind.Day2:
                    return true;
            }
            return false;
        }
        public static DateTime GetRangeEndDateTime(TimeRangeDivideKind kind, DateTime date)
        {
            if (IsDividableKind(kind))
            {
                var span = GetTimeSpan(kind);
                long restTime = (date.Ticks) % span.Ticks;

                // 1分20秒など半端な数字の場合は半端な数字を消化しておく(1分-20秒=40秒を消化）
                if (restTime > 0)
                {
                    return date.AddTicks(span.Ticks - restTime);
                }
                return date.Add(span);
            }
            if (kind == TimeRangeDivideKind.MonthHalf)
            {
                DateTime currentMonth = new DateTime(date.Year, date.Month, 1);
                DateTime nextMonth = currentMonth.AddMonths(1);
                DateTime diff = new DateTime(nextMonth.Ticks - date.Ticks);

                long halfOfMonth = (long)Math.Floor((double)(nextMonth.Ticks - currentMonth.Ticks) / 2);
                if (halfOfMonth < diff.Ticks)
                {
                    var halfDay = currentMonth.AddTicks(halfOfMonth);
                    if (halfDay.Hour > 0 || halfDay.Minute > 0 || halfDay.Second > 0)
                    {
                        return new DateTime(halfDay.Year, halfDay.Month, halfDay.Day).AddDays(1);
                    }
                    return halfDay;
                }

                return date.AddTicks(diff.Ticks);
            }

            return date.AddDays(1);
        }

        public static TimeRangeCollection Divide(TimeRange range)
        {
            TimeRangeCollection list = new TimeRangeCollection();

            list.Kind = GetKind(range);

            var currentDateTime = range.StartDateTime;
            while (currentDateTime < range.EndDateTime)
            {
                var endDate = GetRangeEndDateTime(list.Kind, currentDateTime);
                if (endDate > range.EndDateTime)
                {
                    endDate = range.EndDateTime;
                }

                list.Add(new TimeRange()
                {
                    StartDateTime = currentDateTime,
                    EndDateTime = endDate
                });
                currentDateTime = endDate;
            }

            return list;
        }
    }
}
