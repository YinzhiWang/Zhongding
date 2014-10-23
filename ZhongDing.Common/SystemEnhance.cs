using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System
{

    /// <summary>
    /// Class SystemEnhance
    /// </summary>
    public static class SystemEnhance
    {

        /// <summary>
        /// Gets the paged list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src">The SRC.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <returns>IEnumerable{``0}.</returns>
        public static IEnumerable<T> GetPagedList<T>(this IEnumerable<T> src, int pageIndex, int pageSize, ref int totalRecords)
        {
            totalRecords = src.Count();

            return src.Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="format">The format.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public static string ToString(this Nullable<DateTime> source, string format)
        {
            if (source.HasValue)
                return source.Value.ToString(format);
            else
                return string.Empty;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="format">The format.</param>
        /// <param name="nullstring">The nullstring.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public static string ToString(this Nullable<DateTime> source, string format, string nullstring)
        {
            if (!source.HasValue)
                return nullstring;
            else
                return source.Value.ToString(format);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="format">The format.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public static string ToString(this Nullable<decimal> source, string format)
        {
            return source.GetValueOrDefault().ToString(format);
        }

        /// <summary>
        /// To the date time string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>System.String.</returns>
        public static string ToDateTimeString(this Nullable<DateTime> source)
        {
            if (source.HasValue)
                return source.Value.ToString("MMMM dd, yyyy h:mm tt");
            else
                return string.Empty;
        }

        /// <summary>
        /// To the date string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>System.String.</returns>
        public static string ToDateString(this Nullable<DateTime> source)
        {
            if (source.HasValue)
                return source.Value.ToString("MMMM dd, yyyy");
            else
                return string.Empty;
        }

        /// <summary>
        /// To the date string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>System.String.</returns>
        public static string ToDateString(this DateTime source)
        {
            return source.ToString("MMMM dd, yyyy");
        }

        /// <summary>
        /// To the time string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>System.String.</returns>
        public static string ToTimeString(this Nullable<DateTime> source)
        {
            if (source.HasValue)
                return source.Value.ToShortTimeString();
            else
                return string.Empty;
        }

        /// <summary>
        /// To the local date time.
        /// </summary>
        /// <param name="srcDate">The SRC date.</param>
        /// <param name="timeZoneMinutes">The time zone minutes.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ToLocalDateTime(this DateTime srcDate, int? timeZoneMinutes = null)
        {
            if (!timeZoneMinutes.HasValue)
            {
                if ((timeZoneMinutes ?? 0) != 0)
                    srcDate = srcDate.AddMinutes(timeZoneMinutes.Value);
                else
                    srcDate = srcDate.ToLocalTime();
            }
            else
            {
                srcDate = srcDate.AddMinutes(timeZoneMinutes.Value);
            }

            return srcDate;

        }

    }
}
