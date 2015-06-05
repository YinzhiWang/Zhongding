using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Common.Enums;

namespace ZhongDing.Common
{
    /// <summary>
    /// 类：日期时间处理
    /// </summary>
    public class DateAndTime
    {
        /// <summary>
        /// Dates the diff.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <param name="dt1">The DT1.</param>
        /// <param name="dt2">The DT2.</param>
        /// <returns>System.Int64.</returns>
        public static long DateDiff(DateInterval interval, DateTime dt1, DateTime dt2)
        {
            return DateDiff(interval, dt1, dt2, System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek);
        }

        /// <summary>
        /// Gets the quarter.
        /// </summary>
        /// <param name="nMonth">The n month.</param>
        /// <returns>System.Int32.</returns>
        private static int GetQuarter(int nMonth)
        {
            if (nMonth <= 3)
                return 1;
            if (nMonth <= 6)
                return 2;
            if (nMonth <= 9)
                return 3;
            return 4;
        }

        /// <summary>
        /// Dates the diff.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <param name="dt1">The DT1.</param>
        /// <param name="dt2">The DT2.</param>
        /// <param name="eFirstDayOfWeek">The e first day of week.</param>
        /// <returns>System.Int64.</returns>
        public static long DateDiff(DateInterval interval, DateTime dt1, DateTime dt2, DayOfWeek eFirstDayOfWeek)
        {
            if (interval == DateInterval.Year)
                return dt2.Year - dt1.Year;

            if (interval == DateInterval.Month)
                return (dt2.Month - dt1.Month) + (12 * (dt2.Year - dt1.Year));

            TimeSpan ts = dt2 - dt1;

            if (interval == DateInterval.Day || interval == DateInterval.DayOfYear)
                return Round(ts.TotalDays);

            if (interval == DateInterval.Hour)
                return Round(ts.TotalHours);

            if (interval == DateInterval.Minute)
                return Round(ts.TotalMinutes);

            if (interval == DateInterval.Second)
                return Round(ts.TotalSeconds);

            if (interval == DateInterval.Weekday)
            {
                return Round(ts.TotalDays / 7.0);
            }

            if (interval == DateInterval.WeekOfYear)
            {
                while (dt2.DayOfWeek != eFirstDayOfWeek)
                    dt2 = dt2.AddDays(-1);
                while (dt1.DayOfWeek != eFirstDayOfWeek)
                    dt1 = dt1.AddDays(-1);
                ts = dt2 - dt1;
                return Round(ts.TotalDays / 7.0);
            }

            if (interval == DateInterval.Quarter)
            {
                double d1Quarter = GetQuarter(dt1.Month);
                double d2Quarter = GetQuarter(dt2.Month);
                double d1 = d2Quarter - d1Quarter;
                double d2 = (4 * (dt2.Year - dt1.Year));
                return Round(d1 + d2);
            }

            return 0;

        }

        /// <summary>
        /// Rounds the specified d val.
        /// </summary>
        /// <param name="dVal">The d val.</param>
        /// <returns>System.Int64.</returns>
        private static long Round(double dVal)
        {
            if (dVal >= 0)
                return (long)Math.Floor(dVal);
            return (long)Math.Ceiling(dVal);
        }

        public static long ConvertDateInterval(DateInterval dateInterval)
        {
            if (dateInterval == DateInterval.Day)
            {
                return 1000 * 60 * 60 * 24;
            }
            if (dateInterval == DateInterval.Hour)
            {
                return 1000 * 60 * 60;
            }
            if (dateInterval == DateInterval.Minute)
            {
                return 1000 * 60;
            }
            if (dateInterval == DateInterval.Month)
            {
                return 1000l * 60 * 60 * 24 * 30;
            }
            if (dateInterval == DateInterval.Second)
            {
                return 1000;
            }
            return 1000;
        }
    }
}
