using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Extension
{
    public static class ObjectExtension
    {
        public static int ToInt(this object obj)
        {
            if (obj == null)
                throw new Exception("不能为null");
            return int.Parse(obj.ToString());
        }
        public static string String(this object obj)
        {
            if (obj == null)
                throw new Exception("不能为null");
            return obj.ToString();
        }
        public static string StringOrNull(this object obj)
        {
            if (obj == null)
                return string.Empty;
            return obj.ToString();
        }
        public static int? ToIntOrNull(this object obj)
        {
            int? result = null;
            if (obj == null)
                return result;
            if (obj.ToString().IsNullOrEmpty())
                return result;
            try
            {
                result = int.Parse(obj.ToString());
            }
            catch
            {
            }
            return result;
        }
        public static decimal ToDecimal(this object obj)
        {
            if (obj == null)
                throw new Exception("不能为null");
            return decimal.Parse(obj.ToString());
        }
        public static decimal? ToDecimalOrNull(this object obj)
        {
            decimal? result = null;
            if (obj == null)
                return result;
            try
            {
                result = decimal.Parse(obj.ToString());
            }
            catch
            {
            }
            return result;
        }
        public static double ToDouble(this object obj)
        {
            if (obj == null)
                throw new Exception("不能为null");
            return double.Parse(obj.ToString());
        }
        public static double? ToDoubleOrNull(this object obj)
        {
            double? result = null;
            if (obj == null)
                return result;
            try
            {
                result = double.Parse(obj.ToString());
            }
            catch
            {
            }
            return result;
        }

     

    }
}
