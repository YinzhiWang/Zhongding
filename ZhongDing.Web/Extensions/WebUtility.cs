﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;
using ZhongDing.Common;
using ZhongDing.Common.Enums;

namespace ZhongDing.Web.Extensions
{
    public class WebUtility
    {
        public static void WriteExceptionLog(Exception exp)
        {
            List<string> categories = new List<string>();
            categories.Add(LogCategory.Exception.ToString());

            var currentUser = ZDMembership.GetUser();

            IDictionary<string, object> extentProp = new Dictionary<string, object>();

            extentProp.Add("OccurTimeOnClient", DateTime.Now.ToLocalDateTime().ToString());
            extentProp.Add("ServerTime", DateTime.Now.ToString());
            extentProp.Add("SourceUrl", HttpContext.Current.Request.Url);

            if (currentUser != null)
            {
                extentProp.Add("Email", currentUser.Email ?? "");
                extentProp.Add("UserName", currentUser.UserName ?? "");
                //extentProp.Add("UserCredential", currentUser.RoleID > 0 ? ((EWebpagesRoles)currentUser.RoleID).ToString() : "");
                extentProp.Add("ClientIP", HttpContext.Current.Request.UserHostAddress);
            }

            if (exp is DbEntityValidationException)
            {
                StringBuilder extentExceptionMsg = new StringBuilder();

                extentExceptionMsg.AppendLine("");

                DbEntityValidationException entityException = exp as DbEntityValidationException;

                foreach (var validationErrors in entityException.EntityValidationErrors)
                {
                    var entityName = (validationErrors.Entry != null && validationErrors.Entry.Entity != null)
                        ? validationErrors.Entry.Entity.ToString() : string.Empty;

                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        extentExceptionMsg.AppendLine(string.Concat("Exception Entity: ", entityName));

                        extentExceptionMsg.AppendLine(string.Concat("Exception Property: ", validationError.PropertyName));

                        extentExceptionMsg.AppendLine(string.Concat("Error Message: ", validationError.ErrorMessage));

                        extentExceptionMsg.AppendLine("");
                    }
                }

                extentProp.Add("DbEntityValidationException Message", extentExceptionMsg.ToString());
            }


            if (exp.InnerException != null)
            {
                var innerExp = exp.InnerException;

                extentProp.Add("InnerException Message", innerExp.Message);
                extentProp.Add("InnerException StackTrace", innerExp.StackTrace);

                if (innerExp is DbEntityValidationException)
                {
                    StringBuilder extentExceptionMsg = new StringBuilder();

                    extentExceptionMsg.AppendLine("");

                    DbEntityValidationException entityException = innerExp as DbEntityValidationException;

                    foreach (var validationErrors in entityException.EntityValidationErrors)
                    {
                        var entityName = (validationErrors.Entry != null && validationErrors.Entry.Entity != null)
                            ? validationErrors.Entry.Entity.ToString() : string.Empty;

                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            extentExceptionMsg.AppendLine(string.Concat("Exception Entity: ", entityName));

                            extentExceptionMsg.AppendLine(string.Concat("Exception Property: ", validationError.PropertyName));

                            extentExceptionMsg.AppendLine(string.Concat("Error Message: ", validationError.ErrorMessage));

                            extentExceptionMsg.AppendLine("");
                        }
                    }

                    extentProp.Add("DbEntityValidationException Message", extentExceptionMsg.ToString());
                }

                if (exp.InnerException.InnerException != null)
                {
                    extentProp.Add("InnerException of InnerException Message", exp.InnerException.Message);
                    extentProp.Add("InnerException of InnerException StackTrace", exp.InnerException.StackTrace);
                }
            }

            extentProp.Add("StackTrace", exp.StackTrace);
            extentProp.Add("TargetSite", exp.TargetSite);

            Utility.WriteLog(exp.Source, exp.Message, categories, (int)LogEventType.Business, extentProp);
        }

        /// <summary>
        /// 获取Int参数值
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <returns>System.Nullable{System.Int32}.</returns>
        public static int? GetIntFromQueryString(string paramName)
        {
            string sParamValue = HttpContext.Current.Request.QueryString[paramName];

            int iParamValue;

            if (int.TryParse(sParamValue, out iParamValue))
                return iParamValue;
            else
                return null;
        }

        /// <summary>
        /// 获取日期时间类型参数值
        /// </summary>
        public static DateTime? GetDateTimeFromQueryString(string paramName)
        {
            string sParamValue = HttpContext.Current.Request.QueryString[paramName];

            DateTime dtParamValue;

            if (DateTime.TryParse(sParamValue, out dtParamValue))
                return dtParamValue;
            else
                return null;
        }

        #region Session Names

        /// <summary>
        /// 公用会话名称(session临时数据)
        /// </summary>
        public class WebSessionNames
        {
            /// <summary>
            /// 入库单明细数据（session临时数据）
            /// </summary>
            public static readonly string StockInDetailData = "STOCK_IN_DETAIL_DATA";

            /// <summary>
            /// 出库单明细数据（session临时数据）
            /// </summary>
            public static readonly string StockOutDetailData = "STOCK_OUT_DETAIL_DATA";
        }

        #endregion

    }
}