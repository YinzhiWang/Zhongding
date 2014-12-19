using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Newtonsoft.Json;
using ZhongDing.Common.Enums;

namespace ZhongDing.Common
{
    /// <summary>
    /// Class Utility
    /// </summary>
    public static partial class Utility
    {
        /// <summary>
        /// Jsons the seralize.
        /// </summary>
        /// <param name="srcObj">The SRC obj.</param>
        /// <returns>System.String.</returns>
        public static string JsonSeralize(object srcObj)
        {
            return JsonConvert.SerializeObject(srcObj);
        }

        /// <summary>
        /// Jsons the deserialize object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src">The SRC.</param>
        /// <returns>``0.</returns>
        public static T JsonDeserializeObject<T>(string src)
        {
            return JsonConvert.DeserializeObject<T>(src);
        }

        /// <summary>
        /// Writes the exception log.
        /// </summary>
        /// <param name="exp">The exp.</param>
        public static void WriteExceptionLog(Exception exp, LogEventType logEventType = LogEventType.Business)
        {
            List<string> categories = new List<string>();
            categories.Add(LogCategory.Exception.ToString());

            IDictionary<string, object> extentProp = new Dictionary<string, object>();
            extentProp.Add("OccurTimeOnClient", DateTime.Now.ToLocalDateTime().ToString());
            extentProp.Add("ServerTime", DateTime.Now.ToString());

            if (HttpContext.Current != null
                && HttpContext.Current.Request != null)
            {
                extentProp.Add("SourceUrl", HttpContext.Current.Request.Url);
                extentProp.Add("ClientIP", HttpContext.Current.Request.UserHostAddress);
            }

            if (exp.InnerException != null)
            {
                exp = exp.InnerException;
            }

            extentProp.Add("StackTrace", exp.StackTrace);
            extentProp.Add("TargetSite", exp.TargetSite);

            WriteLog(exp.Source, exp.Message, categories, (int)logEventType, extentProp);
        }

        /// <summary>
        /// Writes the trace.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public static void WriteTrace(string msg)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(msg, "General");
        }

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="categories">The categories.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="extendProp">The extend prop.</param>
        public static void WriteLog(string title, string msg, ICollection<string> categories, int eventId, IDictionary<string, object> extendProp)
        {
            LogEntry lEntry = new LogEntry();
            lEntry.Categories = categories;
            lEntry.Message = msg;
            lEntry.Title = title;
            lEntry.EventId = eventId;
            lEntry.ExtendedProperties = extendProp;

            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(lEntry);

        }

        /// <summary>
        /// Prefixes url with "http://" if it does not already begin with "http".
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string PrefixUrlWithHttpIfNecessary(string url)
        {

            if (!string.IsNullOrEmpty(url))
            {

                if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {

                    url = string.Format("http://{0}", url);

                }

            }

            return url;

        }

        /// <summary>
        /// Gets the date diff.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="diffType">Type of the diff.</param>
        /// <returns>System.Int32.</returns>
        public static int GetDateDiff(DateTime startDate, DateTime endDate, DateDiffType diffType = DateDiffType.Day)
        {
            TimeSpan timeSpan = endDate - startDate;
            switch (diffType)
            {
                case DateDiffType.Day:
                    return timeSpan.Days;
                case DateDiffType.Hour:
                    return timeSpan.Hours;
                case DateDiffType.Minute:
                    return timeSpan.Minutes;
                default:
                    return timeSpan.Days;
            }
        }


        /// <summary>
        /// Cuts the string.
        /// </summary>
        /// <param name="originalStr">The original STR.</param>
        /// <param name="cutLength">Length of the cut.</param>
        /// <param name="isContainEllipses">if set to <c>true</c> [is contain ellipses].</param>
        /// <returns>System.String.</returns>
        public static string CutString(string originalStr, int cutLength, bool isContainEllipses = false)
        {
            string cutStr = string.Empty;

            if (!string.IsNullOrWhiteSpace(originalStr))
            {
                if (originalStr.Length > cutLength)
                    cutStr = originalStr.Substring(0, cutLength);
                else
                    cutStr = originalStr;
            }

            if (isContainEllipses
                && !string.IsNullOrWhiteSpace(cutStr))
            {
                cutStr += "...";
            }

            return cutStr;
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="isContainMapPath">if set to <c>true</c> [is contain map path].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static bool DeleteFile(string filePath, bool isContainMapPath = false)
        {
            bool isDeleted = false;

            if (!string.IsNullOrEmpty(filePath))
            {
                if (!isContainMapPath)
                {
                    if (!filePath.StartsWith("~"))
                        filePath = "~" + filePath;

                    filePath = HttpContext.Current.Server.MapPath(filePath);
                }

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);

                    isDeleted = true;
                }
            }

            return isDeleted;

        }

        /// <summary>
        /// Clears the HTML tags.
        /// </summary>
        /// <param name="strHtml">The STR HTML.</param>
        /// <returns>System.String.</returns>
        public static string ClearHTMLTags(string strHtml)
        {
            string[] Regexs ={
                        @"<script[^>]*?>.*?</script>",
                        @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                        @"([\r\n])[\s]+",
                        @"&(quot|#34);",
                        @"&(amp|#38);",
                        @"&(lt|#60);",
                        @"&(gt|#62);",
                        @"&(nbsp|#160);",
                        @"&(iexcl|#161);",
                        @"&(cent|#162);",
                        @"&(pound|#163);",
                        @"&(copy|#169);",
                        @"&#(\d+);",
                        @"-->",
                        @"<!--.*\n"};

            string[] Replaces ={
                            "",
                            "",
                            " ",
                            "\"",
                            "&",
                            "<",
                            ">",
                            " ",
                            "\xa1", //chr(161),
                            "\xa2", //chr(162),
                            "\xa3", //chr(163),
                            "\xa9", //chr(169),
                            "",
                            "\r\n",
                            ""};

            string s = strHtml;
            for (int i = 0; i < Regexs.Length; i++)
            {
                s = new Regex(Regexs[i], RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(s, Replaces[i]);
            }
            s.Replace("<", "");
            s.Replace(">", "");
            s.Replace("\r\n", " ");
            return s;
        }

        /// <summary>
        /// Gets the network IP address.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetNetworkIPAddress()
        {
            string sNetWorkIP = string.Empty;

            try
            {
                HttpRequest request = HttpContext.Current.Request;

                if (request.ServerVariables["http_VIA"] != null)
                {
                    if (request.ServerVariables["http_X_FORWARDED_FOR"] != null)
                    {
                        sNetWorkIP = request.ServerVariables["http_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
                    }
                }
                else
                {
                    sNetWorkIP = request.UserHostAddress;
                }
            }
            catch (Exception exp)
            {
                WriteExceptionLog(exp);
            }

            return sNetWorkIP;
        }

        /// <summary>
        /// 格式化利率: 0.10被格式化为10
        /// </summary>
        /// <param name="dbRate">数据库里保存的利率(decimal).</param>
        /// <returns>System.Nullable{System.Double}.</returns>
        public static double? FormatRate(decimal? dbRate)
        {
            double? formattedRate = null;

            if (dbRate.HasValue)
                formattedRate = (double?)dbRate.Value * 100;

            return formattedRate;
        }

        /// <summary>
        /// 生成自动编号
        /// </summary>
        /// <param name="maxEntityID">最大实体ID.</param>
        /// <param name="prefix">编号前缀.</param>
        /// <param name="suffix">编号后缀.</param>
        /// <returns>生成的自动编号.</returns>
        public static string GenerateAutoSerialNo(int? maxEntityID, string prefix = "", string suffix = "")
        {
            string serialNo = string.Empty;

            int curEntityID = (maxEntityID.HasValue && maxEntityID >= 0) ? (maxEntityID.Value + 1) : 1;

            if (curEntityID < 10)
                serialNo = "00" + curEntityID.ToString();
            else if (curEntityID >= 10 && curEntityID < 100)
                serialNo = "0" + curEntityID.ToString();
            else
                serialNo = curEntityID.ToString();

            if (!string.IsNullOrEmpty(prefix))
                serialNo = prefix + serialNo;

            if (!string.IsNullOrEmpty(suffix))
                serialNo += suffix;

            return serialNo;
        }

        /// <summary>
        /// 遮罩帐号
        /// </summary>
        /// <param name="originalStr">The original STR.</param>
        /// <returns>System.String.</returns>
        public static string MaskAccount(string originalStr)
        {
            string maskedStr = string.Empty;

            if (!string.IsNullOrEmpty(originalStr))
            {
                if (originalStr.Length >= 8)
                    maskedStr = originalStr.Substring(0, 4) + "****" + originalStr.Substring(originalStr.Length - 4);
                else if (originalStr.Length >= 4)
                    maskedStr = "****" + originalStr.Substring(originalStr.Length - 4);
                else
                    maskedStr = "****" + originalStr;
            }

            return maskedStr;
        }

        /// <summary>
        /// 格式化银行卡号
        /// </summary>
        /// <param name="originalStr">原字符.</param>
        /// <param name="separator">分隔符.</param>
        public static string FormatAccountNumber(string originalStr, string separator = " ")
        {
            string newValue = string.Empty;

            if (!string.IsNullOrEmpty(originalStr))
            {
                originalStr = originalStr.Replace("-", "").Replace(" ", "");

                if (originalStr.Length >= 4)
                {
                    newValue = originalStr.Substring(0, 4);

                    if (originalStr.Length >= 8)
                    {
                        newValue += separator + originalStr.Substring(4, 4);

                        if (originalStr.Length >= 12)
                        {
                            newValue += separator + originalStr.Substring(8, 4);

                            if (originalStr.Length >= 16)
                            {
                                newValue += separator + originalStr.Substring(12, 4);

                                if (originalStr.Length >= 19)
                                {
                                    newValue += separator + originalStr.Substring(16, 3);
                                }
                            }
                            else
                                newValue += separator + originalStr.Substring(8);
                        }
                        else
                            newValue += separator + originalStr.Substring(8);
                    }
                    else
                        newValue += separator + originalStr.Substring(4);
                }
            }

            return newValue;
        }


        /// <summary>
        /// 是否有效的帐号
        /// </summary>
        /// <param name="accountNumber">The account number.</param>
        /// <returns><c>true</c> if [is valid account number] [the specified account number]; otherwise, <c>false</c>.</returns>
        public static bool IsValidAccountNumber(string accountNumber)
        {
            bool isValidAccountNumber = true;

            if (!string.IsNullOrEmpty(accountNumber))
            {
                //去除-和空格
                accountNumber = accountNumber.Replace("-", "").Replace(" ", "");

                char firstChar = Convert.ToChar(accountNumber.Substring(0, 1));

                int repeatCount = 0;

                foreach (char curChar in accountNumber.ToCharArray())
                {
                    if (curChar.Equals(firstChar))
                        repeatCount++;
                }

                if (repeatCount == accountNumber.Length)
                    isValidAccountNumber = false;
            }

            return isValidAccountNumber;
        }
    }
}
