using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Common.Enums;

namespace ZhongDing.Common
{
    public class PermissionManager
    {
        public static EPermissionOption ConverEnum(string per)
        {
            EPermissionOption perm = (EPermissionOption)Enum.Parse(typeof(EPermissionOption), per);
            return perm;
        }
        public static EPermissionOption ConverEnum(int per)
        {
            EPermissionOption perm = (EPermissionOption)Enum.Parse(typeof(EPermissionOption), per.ToString());
            return perm;
        }
        /// <summary>        
        /// 是否有T这个值        
        /// bool canRead = permissions.Has(PermissionTypes.Read);        
        /// </summary>        
        /// <typeparam name="T"></typeparam>        
        /// <param name="type"></param>        
        /// <param name="value"></param>        
        /// <returns></returns>        
        public static bool HasRight(EPermissionOption type, EPermissionOption value)
        {
            try
            {
                return (((int)type & (int)value) == (int)value);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 是否T完全相等        
        /// </summary>        
        /// <typeparam name="T"></typeparam>        
        /// <param name="type"></param>        
        /// <param name="value"></param>        
        /// <returns></returns>        
        public static bool IsEquals<T>(EPermissionOption type, EPermissionOption value)
        {
            try
            {
                return (int)type == (int)value;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>        
        /// 添加        
        /// </summary>        
        /// <typeparam name="T"></typeparam>        
        /// <param name="type"></param>        
        /// <param name="value"></param>        
        /// <returns></returns>        
        public static EPermissionOption AddRight<T>(EPermissionOption type, EPermissionOption value)
        {
            try
            {
                return (EPermissionOption)(((int)type | (int)value));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>        
        /// 移除        
        /// </summary>        
        /// <typeparam name="T"></typeparam>        
        /// <param name="type"></param>        
        /// <param name="value"></param>        
        /// <returns></returns>        
        public static EPermissionOption RemoveRight(EPermissionOption type, EPermissionOption value)
        {
            try
            {
                return (EPermissionOption)(((int)type & ~(int)value));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}