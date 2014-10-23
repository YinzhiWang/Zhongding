using System;
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

            extentProp.Add("OccurTimeOnClient", DateTime.UtcNow.ToLocalDateTime().ToString());
            extentProp.Add("ServerTime", DateTime.UtcNow.ToString());
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
    }
}