using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common
{
    public class WebConfig
    {
        #region Config consts

        #region Web site config consts

        private static readonly string CONFIGKEY_EMAILENABLED = "Email.Enabled";
        private static readonly string CONFIGKEY_EMAILSMTPSERVER = "Email.SmtpServer";
        private static readonly string CONFIGKEY_EMAILSMTPSERVERPORT = "Email.SmtpServerPort";
        private static readonly string CONFIGKEY_EMAILFROM = "Email.From";
        private static readonly string CONFIGKEY_EMAILUSERNAME = "Email.UserName";
        private static readonly string CONFIGKEY_EMAILPASSWORD = "Email.Password";
        private static readonly string CONFIGKEY_EMAILTO = "Email.To";
        private static readonly string CONFIGKEY_EMAILCC = "Email.Cc";
        private static readonly string CONFIGKEY_EMAILBCC = "Email.Bcc";

        private static readonly string CONFIGKEY_UPLOADFILEPATH_COMMON = "UploadFilePath.Common";
        private static readonly string CONFIGKEY_UPLOADFILEPATH_SUPPLIER_CONTRACT = "UploadFilePath.SupplierContract";
        private static readonly string CONFIGKEY_UPLOADFILEPATH_PRODUCT = "UploadFilePath.Product";
        private static readonly string CONFIGKEY_UPLOADFILEPATH_DCFLOWDATA = "UploadFilePath.DCFlowData";
        private static readonly string CONFIGKEY_UPLOADFILEPATH_DCINVENTORYDATA = "UploadFilePath.DCInventoryData";
        private static readonly string CONFIGKEY_UPLOADFILEPATH_CLIENTFLOWDATA = "UploadFilePath.ClientFlowData";



        private static readonly string CONFIGKEY_MEMBERSHIP_PASSWORDRESETLENGTH = "Membership.PasswordResetLength";
        private static readonly string CONFIGKEY_MEMBERSHIP_PASSWORDRESETNONALPHANUMERICCOUNT = "Membership.PasswordResetNonalphanumericCount";
        private static readonly string CONFIGKEY_MEMBERSHIP_LOCKEDOUTTIMEOUT = "Membership.LockedOutTimeout";

        private static readonly string CONFIGKEY_WEBSITE_ROOTURL = "Website.RootUrl";
        private static readonly string CONFIGKEY_WEBSITE_ABSOLUTE_ROOT_PATH = "Website.AbsoluteRootPath";

        private static readonly string CONFIGKEY_MAX_GUARANTEE_AMOUNT = "MaxGuaranteeAmount";

        #endregion

        #region Win service config consts

        /// <summary>
        /// 计算库存服务运行的开始时间
        /// </summary>
        private static readonly string CONFIGKEY_CALCULATEINVENTORY_SERVICE_STARTTIME = "CalculateInventoryService.StartTime";

        /// <summary>
        /// 计算库存服务运行的周期
        /// </summary>
        private static readonly string CONFIGKEY_CALCULATEINVENTORY_SERVICE_INTERVAl = "CalculateInventoryService.Interval";

        /// <summary>
        /// 导入数据服务运行的开始时间
        /// </summary>
        private static readonly string CONFIGKEY_IMPORTDATA_SERVICE_STARTTIME = "ImportDataService.StartTime";

        /// <summary>
        /// 导入数据服务运行的周期
        /// </summary>
        private static readonly string CONFIGKEY_IMPORTDATA_SERVICE_INTERVAl = "ImportDataService.Interval";
        /// <summary>
        /// 一下是云通讯 SKD Config
        /// </summary>
        private static readonly string CONFIGKEY_YunTongXun_ACCOUNT_SID = "YunTongXun.ACCOUNT_SID";
        private static readonly string CONFIGKEY_YunTongXun_AUTH_TOKEN = "YunTongXun.AUTH_TOKEN";
        private static readonly string CONFIGKEY_YunTongXun_APP_ID = "YunTongXun.APP_ID";
        private static readonly string CONFIGKEY_YunTongXun_TemplateId_StockOutReminder = "YunTongXun.TemplateId_StockOutReminder";
        private static readonly string CONFIGKEY_YunTongXun_CCPRestSDK_Address = "YunTongXun.CCPRestSDK_Address";
        private static readonly string CONFIGKEY_YunTongXun_CCPRestSDK_Port = "YunTongXun.CCPRestSDK_Port";


        #endregion

        #endregion

        #region Config properties

        #region Web site config properties

        public static bool EmailEnabled
        {
            get
            {
                bool emailEnabled = false;

                if (bool.TryParse(ConfigurationManager.AppSettings[CONFIGKEY_EMAILENABLED], out emailEnabled))
                    emailEnabled = true;

                return emailEnabled;
            }
        }

        public static string EmailSmtpServer
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_EMAILSMTPSERVER];
            }
        }

        public static int EmailSmtpServerPort
        {
            get
            {
                int emailSmtpServerPort = GlobalConst.INVALID_INT;

                try
                {
                    emailSmtpServerPort = int.Parse(ConfigurationManager.AppSettings[CONFIGKEY_EMAILSMTPSERVERPORT]);
                }
                catch { }
                return emailSmtpServerPort;
            }
        }

        public static string EmailFrom
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_EMAILFROM];
            }
        }

        public static string EmailUserName
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_EMAILUSERNAME];
            }
        }

        public static string EmailPassword
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_EMAILPASSWORD];
            }
        }

        public static string EmailTo
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_EMAILTO];
            }
        }

        public static string EmailCc
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_EMAILCC];
            }
        }

        public static string EmailBcc
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_EMAILBCC];
            }
        }

        /// <summary>
        /// 公共文件上传路径
        /// </summary>
        /// <value>The upload file path common.</value>
        public static string UploadFilePathCommon
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_UPLOADFILEPATH_COMMON];
            }
        }

        /// <summary>
        /// 产品文件上传路径
        /// </summary>
        /// <value>The upload file path product.</value>
        public static string UploadFilePathProduct
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_UPLOADFILEPATH_PRODUCT];
            }
        }

        /// <summary>
        /// 供应商合同文件上传路径
        /// </summary>
        public static string UploadFilePathSupplierContract
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_UPLOADFILEPATH_SUPPLIER_CONTRACT];
            }
        }

        /// <summary>
        /// 配送公司流向数据上传路径
        /// </summary>
        public static string UploadFilePathDCFlowData
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_UPLOADFILEPATH_DCFLOWDATA];
            }
        }

        /// <summary>
        /// 配送公司库存数据上传路径
        /// </summary>
        public static string UploadFilePathDCInventoryData
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_UPLOADFILEPATH_DCINVENTORYDATA];
            }
        }

        /// <summary>
        /// 商业客户流向数据上传路径
        /// </summary>
        /// <value>The upload file path client flow data.</value>
        public static string UploadFilePathClientFlowData
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_UPLOADFILEPATH_CLIENTFLOWDATA];
            }
        }

        /// <summary>
        /// 密码恢复时格式长度
        /// </summary>
        public static int PasswordResetLength
        {
            get
            {

                int passwordResetLength;
                if (int.TryParse(ConfigurationManager.AppSettings[CONFIGKEY_MEMBERSHIP_PASSWORDRESETLENGTH], out passwordResetLength))
                    return passwordResetLength;
                else
                    return GlobalConst.DEFAULT_PASSWORD_RESET_LENGTH;
            }
        }

        /// <summary>
        /// 密码恢复时格式中特殊字符的个数
        /// </summary>
        public static int PasswordResetNonalphanumericCount
        {
            get
            {

                int passwordResetNonalphanumericCount;
                if (int.TryParse(ConfigurationManager.AppSettings[CONFIGKEY_MEMBERSHIP_PASSWORDRESETNONALPHANUMERICCOUNT], out passwordResetNonalphanumericCount))
                    return passwordResetNonalphanumericCount;
                else
                    return GlobalConst.DEFAULT_PASSWORD_RESET_NONALPHANUMERIC_COUNT;
            }
        }

        /// <summary>
        /// 锁定用户超时时间
        /// </summary>
        /// <value>The locked out timeout.</value>
        public static int LockedOutTimeout
        {
            get
            {

                int lockedOutTimeout;
                if (int.TryParse(ConfigurationManager.AppSettings[CONFIGKEY_MEMBERSHIP_LOCKEDOUTTIMEOUT], out lockedOutTimeout))
                    return lockedOutTimeout;
                else
                    return GlobalConst.DEFAULT_LOCKEDOUT_TIMEOUT;
            }
        }

        /// <summary>
        /// 系统部署后的根目录Url.
        /// </summary>
        /// <value>The website root URL.</value>
        public static string WebsiteRootUrl
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_WEBSITE_ROOTURL];
            }
        }

        /// <summary>
        /// 系统部署后的根目录绝对路径.
        /// </summary>
        public static string WebsiteAbsoluteRootPath
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_WEBSITE_ABSOLUTE_ROOT_PATH];
            }
        }

        /// <summary>
        /// 最大担保金额
        /// </summary>
        /// <value>The max guarantee amount.</value>
        public static decimal MaxGuaranteeAmount
        {
            get
            {
                decimal maxGuaranteeAmount;
                if (decimal.TryParse(ConfigurationManager.AppSettings[CONFIGKEY_MAX_GUARANTEE_AMOUNT], out maxGuaranteeAmount))
                    return maxGuaranteeAmount;
                else
                    return GlobalConst.DEFAULT_MAX_GUARANTEE_AMOUNT;
            }
        }

        #endregion

        #region Win Service config properties

        /// <summary>
        /// 计算库存服务运行的开始时间
        /// </summary>
        public static DateTime CalculateInventoryServiceStartTime
        {
            get
            {
                try
                {
                    return DateTime.Parse(ConfigurationManager.AppSettings[CONFIGKEY_CALCULATEINVENTORY_SERVICE_STARTTIME]);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 计算库存服务运行的周期
        /// </summary>
        public static int CalculateInventoryServiceInterval
        {
            get
            {
                var sInterval = ConfigurationManager.AppSettings[CONFIGKEY_CALCULATEINVENTORY_SERVICE_INTERVAl];

                int iInerval;
                if (int.TryParse(sInterval, out iInerval))
                    return iInerval;
                else
                    return GlobalConst.WIN_SERVICE_DEFAULT_INTERVAl;
            }
        }

        /// <summary>
        /// 导入数据服务运行的开始时间
        /// </summary>
        public static DateTime ImportDataServiceStartTime
        {
            get
            {
                try
                {
                    return DateTime.Parse(ConfigurationManager.AppSettings[CONFIGKEY_IMPORTDATA_SERVICE_STARTTIME]);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 导入数据服务运行的周期
        /// </summary>
        public static int ImportDataServiceInterval
        {
            get
            {
                var sInterval = ConfigurationManager.AppSettings[CONFIGKEY_IMPORTDATA_SERVICE_INTERVAl];

                int iInerval;
                if (int.TryParse(sInterval, out iInerval))
                    return iInerval;
                else
                    return GlobalConst.WIN_SERVICE_DEFAULT_INTERVAl;
            }
        }

        #endregion

        #region 云通讯
        public static string YunTongXun_ACCOUNT_SID
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_YunTongXun_ACCOUNT_SID];
            }
        }
        public static string YunTongXun_AUTH_TOKEN
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_YunTongXun_AUTH_TOKEN];
            }
        }
        public static string YunTongXun_APP_ID
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_YunTongXun_APP_ID];
            }
        }
        public static string YunTongXun_TemplateId_StockOutReminder
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_YunTongXun_TemplateId_StockOutReminder];
            }
        }
        public static string YunTongXun_CCPRestSDK_Address
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_YunTongXun_CCPRestSDK_Address];
            }
        }
        public static string YunTongXun_CCPRestSDK_Port
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_YunTongXun_CCPRestSDK_Port];
            }
        }

        #endregion

        #endregion

    }
}
