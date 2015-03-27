using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Sms
{
    public class YunTongXunConfig
    {
        private YunTongXunConfig()
        {
        }
        private static YunTongXunInfo _YunTongXunInfo;
        public static YunTongXunInfo GetYunTongXunInfo()
        {
            if (_YunTongXunInfo == null)
            {
                _YunTongXunInfo = new YunTongXunInfo()
                {
                    ACCOUNT_SID = WebConfig.YunTongXun_ACCOUNT_SID,
                    APP_ID = WebConfig.YunTongXun_APP_ID,
                    AUTH_TOKEN = WebConfig.YunTongXun_AUTH_TOKEN,
                    CCPRestSDK_Address = WebConfig.YunTongXun_CCPRestSDK_Address,
                    CCPRestSDK_Port = WebConfig.YunTongXun_CCPRestSDK_Port,
                    TemplateId_StockOutReminder = WebConfig.YunTongXun_TemplateId_StockOutReminder
                };
            }

            return _YunTongXunInfo;
        }
    }
    [Serializable]
    public class YunTongXunInfo
    {
        public string ACCOUNT_SID { get; set; }
        public string AUTH_TOKEN { get; set; }
        public string APP_ID { get; set; }
        public string TemplateId_StockOutReminder { get; set; }
        public string CCPRestSDK_Address { get; set; }
        public string CCPRestSDK_Port { get; set; }
        //string ACCOUNT_SID = "8a48b5514790f9dc014796b9fae402a1";
        //string AUTH_TOKEN = "21e4716578b5420e979fd13d6ffde220";
        //string APP_ID = "8a48b5514790f9dc014796bcf6eb02aa";
        //string PhoneNumber = yunTongXunMobileValidateParameter.PhoneNumber;
        //string TemplateId = yunTongXunMobileValidateParameter.TemplateId;
        //string[] Contents = yunTongXunMobileValidateParameter.Contents;

        //string ret = null;
        //CCPRestSDK.CCPRestSDK api = new CCPRestSDK.CCPRestSDK();
        ////ip格式如下，不带https://
        //bool isInit = api.init("sandboxapp.cloopen.com", "8883");
    }
}
