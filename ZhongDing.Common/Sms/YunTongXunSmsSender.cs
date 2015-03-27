using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZhongDing.Common.Sms
{
    public class YunTongXunSmsSenderParameter : BaseSmsSenderParameter
    {
        public string TemplateId { get; set; }
        public string[] Contents { get; set; }
    }
    public class YunTongXunSmsSender : BaseSmsSender
    {
        public override SmsSenderResult Send(IBaseSmsSenderParameter para)
        {
            //if (!isSendRealSendCode)
            //{
            //    return new SmsSenderResult() { Result = true, Info = "发送成功！" };
            //}

            //YunTongXunSmsSenderParameter yunTongXunMobileValidateParameter = para as YunTongXunSmsSenderParameter;

            //var baseConfig = YunTongXunConfig.GetYunTongXunInfo();

            //string PhoneNumber = yunTongXunMobileValidateParameter.PhoneNumber;
            //string TemplateId = yunTongXunMobileValidateParameter.TemplateId;
            //string[] Contents = yunTongXunMobileValidateParameter.Contents;
            //string ret = null;
            //CCPRestSDK.CCPRestSDK api = new CCPRestSDK.CCPRestSDK();
            ////ip格式如下，不带https://
            //bool isInit = api.init(baseConfig.CCPRestSDK_Address, baseConfig.CCPRestSDK_Port);
            //api.setAccount(baseConfig.ACCOUNT_SID, baseConfig.AUTH_TOKEN);
            //api.setAppId(baseConfig.APP_ID);

            //try
            //{
            //    if (isInit)
            //    {
            //        Dictionary<string, object> retData = null;
            //        // var retData=  api.SendSMS(短信接收号码, "【记忆王】test ning");
            //        retData = api.SendTemplateSMS(PhoneNumber, TemplateId, Contents);
            //        ret = GetDictionaryData(retData);
            //        if (retData != null && retData["statusCode"] != null && retData["statusCode"].ToString() == "000000")
            //        {
            //            return new SmsSenderResult() { Result = true, Info = "发送成功！" };
            //        }
            //    }
            //    else
            //    {
            //        ret = "初始化失败";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ret = ex.Message;
            //}
            //return new SmsSenderResult() { Result = false, Info = "发送失败，请稍后重试！", ErrorInfo = ret };

            return new SmsSenderResult() { Result = true, Info = "发送成功" };
        }
        public SmsSenderResult SendVoice(IBaseSmsSenderParameter para)
        {
            //if (!isSendRealSendCode)
            //{
            //    return new SmsSenderResult() { Result = true, Info = "发送成功！" };
            //}

            //YunTongXunSmsSenderParameter yunTongXunMobileValidateParameter = para as YunTongXunSmsSenderParameter;
            //var baseConfig = YunTongXunConfig.GetYunTongXunInfo();


            //string PhoneNumber = yunTongXunMobileValidateParameter.PhoneNumber;
            //string TemplateId = yunTongXunMobileValidateParameter.TemplateId;
            //string[] Contents = yunTongXunMobileValidateParameter.Contents;

            //string ret = null;
            //CCPRestSDK.CCPRestSDK api = new CCPRestSDK.CCPRestSDK();
            ////ip格式如下，不带https://
            //bool isInit = api.init(baseConfig.CCPRestSDK_Address, baseConfig.CCPRestSDK_Port);
            //api.setAccount(baseConfig.ACCOUNT_SID, baseConfig.AUTH_TOKEN);
            //api.setAppId(baseConfig.APP_ID);


            //try
            //{
            //    if (isInit)
            //    {
            //        // var retData=  api.SendSMS(短信接收号码, "【记忆王】test ning");
            //        Dictionary<string, object> retData = api.VoiceVerify(PhoneNumber, para.Code, "17701030128", "3", "");
            //        ret = GetDictionaryData(retData);
            //        if (retData != null && retData["statusCode"] != null && retData["statusCode"].ToString() == "000000")
            //        {
            //            return new SmsSenderResult() { Result = true, Info = "发送成功！" };
            //        }
            //    }
            //    else
            //    {
            //        ret = "初始化失败";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ret = ex.Message;

            //}
            //return new SmsSenderResult() { Result = false, Info = "发送失败，请稍后重试！", ErrorInfo = ret };


            return new SmsSenderResult() { Result = true, Info = "发送成功" };
        }
        private string GetDictionaryData(Dictionary<string, object> data)
        {
            string ret = null;
            foreach (KeyValuePair<string, object> item in data)
            {
                if (item.Value != null && item.Value.GetType() == typeof(Dictionary<string, object>))
                {
                    ret += item.Key.ToString() + "={";
                    ret += GetDictionaryData((Dictionary<string, object>)item.Value);
                    ret += "};";
                }
                else
                {
                    ret += item.Key.ToString() + "=" + (item.Value == null ? "null" : item.Value.ToString()) + ";";
                }
            }
            return ret;
        }
    }
}
