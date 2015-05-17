using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
        public static string ToStringOrNull(this object obj)
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
        public static DateTime ToDateTime(this object obj)
        {
            if (obj == null)
                throw new Exception("不能为null");
            return DateTime.Parse(obj.ToString());
        }
        public static DateTime? ToDateTimeOrNull(this object obj)
        {
            DateTime result;
            if (obj == null)
                return null;
            try
            {
                if (DateTime.TryParse(obj.ToString(), out result))
                {
                    return result;
                }
            }
            catch
            {
            }
            return null;
        }
        public static string GetName<T>(this object obj, Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression != null)
            {
                MemberExpression body = propertyExpression.Body as MemberExpression;
                if (body != null)
                {
                    PropertyInfo property = body.Member as PropertyInfo;
                    if (property != null)
                    {
                        return property.Name;
                    }
                    else
                    {
                        throw new ArgumentException("Argument is not a property", "propertyExpression");
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid argument", "propertyExpression");
                }
            }
            else
            {
                throw new ArgumentNullException("propertyExpression");
            }
        }

        // We need to add this overload to cover scenarios 
        // when a method has a void return type.
        public static string GetName(this object obj, Expression<Action> e)
        {
            string typeName = obj.GetType().ToString();
            string memberName = string.Empty;

            memberName = obj.GetName(e.Body);
            return string.Format("{0}-{1}", typeName, memberName);
        }

        private static string GetName(this object obj, Expression body)
        {
            var member = body as MemberExpression;
            if (member != null) return member.Member.ToString();

            var method = body as MethodCallExpression;
            if (method != null) return method.Method.ToString();

            throw new ArgumentException(
                "'" + body + "': not a member access");
        }

    }
}
