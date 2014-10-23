using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ZhongDing.Common
{
    public class EmailService
    {

        /// <summary>
        /// 发送邮件.
        /// </summary>
        /// <param name="sTo">收件人.</param>
        /// <param name="sSubject">标题.</param>
        /// <param name="sBody">邮件内容.</param>
        /// <param name="isBodyHtml">如果设置为 <c>true</c> [该邮件是html格式的].</param>
        /// <param name="sAttachmentList">附件列表.</param>
        /// <returns><c>true</c> 发送成功, <c>false</c> 否则</returns>
        public static bool SendEmail(string sTo, string sSubject, string sBody,
            bool isBodyHtml = false, string sAttachmentList = "")
        {
            bool isSuccessfulSent = false;

            if (!WebConfig.EmailEnabled)
                return isSuccessfulSent;

            try
            {
                if (string.IsNullOrWhiteSpace(sTo))
                    return isSuccessfulSent;

                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress(WebConfig.EmailFrom);
                mailMsg.IsBodyHtml = isBodyHtml;
                mailMsg.Subject = sSubject;
                mailMsg.Body = ReBuildEmailBody(sBody, isBodyHtml);

                //添加收件人
                ArrayList toAddressArray = GetEmailAddressArrayList(sTo);

                foreach (MailAddress toAddress in toAddressArray)
                {
                    mailMsg.To.Add(toAddress);
                }

                //添加config中配置的收件人
                if (!string.IsNullOrWhiteSpace(WebConfig.EmailTo))
                {
                    ArrayList defaultToAddressArray = GetEmailAddressArrayList(WebConfig.EmailTo);

                    foreach (MailAddress defaultToAddress in defaultToAddressArray)
                    {
                        mailMsg.To.Add(defaultToAddress);
                    }
                }

                if (!string.IsNullOrWhiteSpace(WebConfig.EmailCc))
                {
                    ArrayList ccAddressArray = GetEmailAddressArrayList(WebConfig.EmailCc);

                    foreach (MailAddress ccAddress in ccAddressArray)
                    {
                        mailMsg.CC.Add(ccAddress);
                    }
                }

                if (!string.IsNullOrWhiteSpace(WebConfig.EmailBcc))
                {
                    ArrayList bccAddressArray = GetEmailAddressArrayList(WebConfig.EmailBcc);

                    foreach (MailAddress bccAddress in bccAddressArray)
                    {
                        mailMsg.CC.Add(bccAddress);
                    }
                }


                if (!string.IsNullOrEmpty(sAttachmentList))
                {
                    ArrayList attachmentArray = GetAttachmentArrayList(sAttachmentList);

                    foreach (Attachment attachment in attachmentArray)
                    {
                        mailMsg.Attachments.Add(attachment);
                    }
                }

                //初始化Smtp服务客户端
                SmtpClient smtp = new SmtpClient(WebConfig.EmailSmtpServer);

                smtp.Port = WebConfig.EmailSmtpServerPort;

                smtp.Credentials = new NetworkCredential(WebConfig.EmailUserName, WebConfig.EmailPassword);

                //发送邮件
                smtp.Send(mailMsg);

                isSuccessfulSent = true;
            }
            catch (Exception exp)
            {
                Utility.WriteExceptionLog(exp);
            }
            finally
            {
                Utility.WriteTrace(BuildEmailTraceLog(sTo, sSubject, sBody, isSuccessfulSent));
            }

            return isSuccessfulSent;
        }

        #region Private Methods

        /// <summary>
        /// Gets the email address array list.
        /// </summary>
        /// <param name="strEmailAddressList">The STR email address list.</param>
        /// <returns>ArrayList.</returns>
        private static ArrayList GetEmailAddressArrayList(string strEmailAddressList)
        {
            int iFor;
            int iStart, iEnd, ilength;
            string[] strArray;
            MailAddress mailAddress;
            ArrayList returnArrayList;

            if (string.IsNullOrEmpty(strEmailAddressList)) return null;

            if (strEmailAddressList.Length < 1) return null;

            try
            {
                returnArrayList = new ArrayList();
                if (strEmailAddressList.IndexOf(",") >= 0)
                {
                    strArray = strEmailAddressList.Split(",".ToCharArray());

                    for (iFor = 0; iFor <= strArray.Length - 1; iFor++)
                    {
                        iStart = strArray[iFor].IndexOf("<");
                        iEnd = strArray[iFor].IndexOf(">");
                        ilength = iEnd - iStart - 1;
                        if (iStart >= 0 && iEnd >= 0)
                            mailAddress = new MailAddress(strArray[iFor].Substring(iStart + 1, iEnd - iStart - 1), strArray[iFor].Substring(0, iStart));
                        else
                            mailAddress = new MailAddress(strArray[iFor]);

                        if (!string.IsNullOrWhiteSpace(mailAddress.Address))
                            returnArrayList.Add(mailAddress);
                    }
                }
                else if (strEmailAddressList.IndexOf(";") >= 0)
                {
                    strArray = strEmailAddressList.Split(";".ToCharArray());
                    for (iFor = 0; iFor <= strArray.Length - 1; iFor++)
                    {
                        iStart = strArray[iFor].IndexOf("<");
                        iEnd = strArray[iFor].IndexOf(">");
                        ilength = iEnd - iStart - 1;
                        if (iStart >= 0 && iEnd >= 0)
                            mailAddress = new MailAddress(strArray[iFor].Substring(iStart + 1, iEnd - iStart - 1), strArray[iFor].Substring(0, iStart));
                        else
                            mailAddress = new MailAddress(strArray[iFor]);

                        if (!string.IsNullOrWhiteSpace(mailAddress.Address))
                            returnArrayList.Add(mailAddress);
                    }
                }
                else
                {
                    iStart = strEmailAddressList.IndexOf("<");
                    iEnd = strEmailAddressList.IndexOf(">");
                    ilength = iEnd - iStart - 1;
                    if (iStart >= 0 && iEnd >= 0)
                        mailAddress = new MailAddress(strEmailAddressList.Substring(iStart + 1, iEnd - iStart - 1), strEmailAddressList.Substring(0, iStart));
                    else
                        mailAddress = new MailAddress(strEmailAddressList);

                    if (!string.IsNullOrWhiteSpace(mailAddress.Address))
                        returnArrayList.Add(mailAddress);
                }
            }
            catch (Exception exp)
            {
                Utility.WriteExceptionLog(exp);

                return null;
            }

            return returnArrayList;

        }

        /// <summary>
        /// Gets the attachment array list.
        /// </summary>
        /// <param name="sAttachmentList">The s attachment list.</param>
        /// <returns>ArrayList.</returns>
        private static ArrayList GetAttachmentArrayList(string sAttachmentList)
        {
            ArrayList returnArrayList = new ArrayList();

            string[] strArray;

            try
            {
                if (string.IsNullOrEmpty(sAttachmentList)) return null;

                if (sAttachmentList.IndexOf(",") >= 0)
                {

                    strArray = sAttachmentList.Split(",".ToCharArray());
                    returnArrayList.AddRange(BuildAttachmentList(strArray));
                }
                else if (sAttachmentList.IndexOf(";") >= 0)
                {

                    strArray = sAttachmentList.Split(";".ToCharArray());
                    returnArrayList.AddRange(BuildAttachmentList(strArray));
                }
                else
                {
                    if (File.Exists(sAttachmentList))
                    {
                        Attachment mAttachment = new Attachment(sAttachmentList);
                        returnArrayList.Add(mAttachment);
                    }
                }
            }
            catch (Exception exp)
            {
                Utility.WriteExceptionLog(exp);

                return null;
            }

            return returnArrayList;
        }

        /// <summary>
        /// Builds the attachment list.
        /// </summary>
        /// <param name="strArray">The STR array.</param>
        /// <returns>ArrayList.</returns>
        private static ArrayList BuildAttachmentList(string[] strArray)
        {
            ArrayList arrayListAttachment = new ArrayList();

            foreach (string sAttachmentPath in strArray)
            {
                if (File.Exists(sAttachmentPath))
                {
                    Attachment mAttachment = new Attachment(sAttachmentPath);
                    arrayListAttachment.Add(mAttachment);
                }
            }

            return arrayListAttachment;
        }

        /// <summary>
        /// Res the build email body.
        /// </summary>
        /// <param name="sBody">The s body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        /// <returns>System.String.</returns>
        private static string ReBuildEmailBody(string sBody, bool isBodyHtml = false)
        {

            StringBuilder sbAdjustEmailBody = new StringBuilder(sBody);

            if (isBodyHtml)
            {
                sbAdjustEmailBody.AppendLine("<br />");
                sbAdjustEmailBody.AppendLine("<br />");

                sbAdjustEmailBody.AppendLine("--------------------------------");
                sbAdjustEmailBody.AppendLine("<br />");
                sbAdjustEmailBody.AppendLine("众鼎医药");
                sbAdjustEmailBody.AppendLine("<br />");
                sbAdjustEmailBody.AppendLine("官网：<a href=\"#\" target=\"_blank\">www.#######.cn</a>");
                sbAdjustEmailBody.AppendLine("<br />");
                sbAdjustEmailBody.AppendLine("众鼎医药咨询信息系统：<a href=\"" + WebConfig.WebsiteRootUrl + "\" target=\"_blank\">众鼎医药咨询信息系统</a>");
                sbAdjustEmailBody.AppendLine("<br />");
                sbAdjustEmailBody.AppendLine("电话：XXXXXXXXX");
                sbAdjustEmailBody.AppendLine("<br />");
                sbAdjustEmailBody.AppendLine("传真：XXXXXXXXX");
                sbAdjustEmailBody.AppendLine("<br />");
                sbAdjustEmailBody.AppendLine("该邮件由系统自动发送，请勿回复！");
            }
            else
            {
                sbAdjustEmailBody.AppendLine("\n");
                sbAdjustEmailBody.AppendLine("\n");

                sbAdjustEmailBody.AppendLine("--------------------------------");
                sbAdjustEmailBody.AppendLine("\n");
                sbAdjustEmailBody.AppendLine("众鼎医药");
                sbAdjustEmailBody.AppendLine("\n");
                sbAdjustEmailBody.AppendLine("官网：www..#######..cn");
                sbAdjustEmailBody.AppendLine("\n");
                sbAdjustEmailBody.AppendLine("众鼎医药咨询信息系统：" + WebConfig.WebsiteRootUrl);
                sbAdjustEmailBody.AppendLine("\n");
                sbAdjustEmailBody.AppendLine("电话：XXXXXXXXX");
                sbAdjustEmailBody.AppendLine("\n");
                sbAdjustEmailBody.AppendLine("传真：XXXXXXXXX");
                sbAdjustEmailBody.AppendLine("\n");
                sbAdjustEmailBody.AppendLine("该邮件由系统自动发送，请勿回复！");
            }



            return sbAdjustEmailBody.ToString();
        }

        /// <summary>
        /// Builds the email trace log.
        /// </summary>
        /// <param name="sTo">The s to.</param>
        /// <param name="sSubject">The s subject.</param>
        /// <param name="sBody">The s body.</param>
        /// <returns>System.String.</returns>
        private static string BuildEmailTraceLog(string sTo, string sSubject, string sBody, bool isSent)
        {
            StringBuilder sbEmailTraceLog = new StringBuilder();

            sbEmailTraceLog.AppendLine("----Email Details-----------");

            sbEmailTraceLog.AppendLine("Method: SendEmail.");

            if (HttpContext.Current != null
                && HttpContext.Current.Request != null)
            {
                sbEmailTraceLog.AppendLine("SourceUrl: " + HttpContext.Current.Request.Url);
                sbEmailTraceLog.AppendLine("ClientIP: " + HttpContext.Current.Request.UserHostAddress);
            }

            sbEmailTraceLog.AppendLine("");

            if (isSent)
                sbEmailTraceLog.AppendLine("Send Status: Successful.");
            else
                sbEmailTraceLog.AppendLine("Send Status: Failed.");

            sbEmailTraceLog.AppendLine("");

            sbEmailTraceLog.AppendLine("Send To: " + sTo + ".");

            sbEmailTraceLog.AppendLine("Email Subject: " + sSubject + ".");

            sbEmailTraceLog.AppendLine("Email Body: " + sBody + ".");

            return sbEmailTraceLog.ToString();
        }

        #endregion
    }
}
