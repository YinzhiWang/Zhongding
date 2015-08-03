using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Extension
{
    public static class DateTimeExtension
    {
        public static DateTime ToFirstDayOfMonthDate(this DateTime obj)
        {
            if (obj == null)
                throw new Exception("不能为null");
            return new DateTime(obj.Year, obj.Month, 1);
        }
        public static DateTime ToFirstDayOfYearDate(this DateTime obj)
        {
            if (obj == null)
                throw new Exception("不能为null");
            return new DateTime(obj.Year, 1, 1);
        }
        public static DateTime? ToFirstDayOfMonthDateOrNull(this DateTime? obj)
        {
            if (obj == null)
                return null;
            return new DateTime(obj.Value.Year, obj.Value.Month, 1);
        }
    }
}
