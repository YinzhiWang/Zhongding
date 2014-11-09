using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common
{
    public static class WebConfig
    {
        #region Web config consts
        private const string CONFIGKEY_EMAILENABLED = "Email.Enabled";
        private const string CONFIGKEY_EMAILSMTPSERVER = "Email.SmtpServer";
        private const string CONFIGKEY_EMAILSMTPSERVERPORT = "Email.SmtpServerPort";
        private const string CONFIGKEY_EMAILFROM = "Email.From";
        private const string CONFIGKEY_EMAILUSERNAME = "Email.UserName";
        private const string CONFIGKEY_EMAILPASSWORD = "Email.Password";
        private const string CONFIGKEY_EMAILTO = "Email.To";
        private const string CONFIGKEY_EMAILCC = "Email.Cc";
        private const string CONFIGKEY_EMAILBCC = "Email.Bcc";

        private const string CONFIGKEY_UPLOADFILEPATH_COMMON = "UploadFilePath.Common";
        private const string CONFIGKEY_UPLOADFILEPATH_SUPPLIER_CONTRACT = "UploadFilePath.SupplierContract";
        private const string CONFIGKEY_UPLOADFILEPATH_GUARANTEECOMPANY = "UploadFilePath.GuaranteeCompany";
        private const string CONFIGKEY_UPLOADFILEPATH_RELATEDCOMPANY = "UploadFilePath.RelatedCompany";
        private const string CONFIGKEY_UPLOADFILEPATH_PRODUCT = "UploadFilePath.Product";

        private const string CONFIGKEY_MEMBERSHIP_PASSWORDRESETLENGTH = "Membership.PasswordResetLength";
        private const string CONFIGKEY_MEMBERSHIP_PASSWORDRESETNONALPHANUMERICCOUNT = "Membership.PasswordResetNonalphanumericCount";
        private const string CONFIGKEY_MEMBERSHIP_LOCKEDOUTTIMEOUT = "Membership.LockedOutTimeout";

        private const string CONFIGKEY_WEBSITE_ROOTURL = "Website.RootUrl";
        #endregion

        #region Web config properties

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
        /// 担保公司文件上传路径
        /// </summary>
        /// <value>The upload file path guarantee company.</value>
        public static string UploadFilePathGuaranteeCompany
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_UPLOADFILEPATH_GUARANTEECOMPANY];
            }
        }


        /// <summary>
        /// 公司关联的公司的文件上传路径
        /// </summary>
        /// <value>The upload file path related company.</value>
        public static string UploadFilePathRelatedCompany
        {
            get
            {
                return ConfigurationManager.AppSettings[CONFIGKEY_UPLOADFILEPATH_RELATEDCOMPANY];
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

        #endregion

    }
}
