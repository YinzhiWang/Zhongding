using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ZhongDing.Web
{
    /// <summary>
    /// Class PageEnhance
    /// </summary>
    public static class PageEnhance
    {
        /// <summary>
        /// Redirects to login page.
        /// </summary>
        /// <param name="srcPage">The SRC page.</param>
        /// <param name="sParameters">The s parameters.</param>
        public static void RedirectToLoginPage(this System.Web.UI.Page srcPage, string sParameters = "")
        {
            string sReturnUrl = srcPage.Request.Url.AbsoluteUri;
            if (!string.IsNullOrEmpty(sParameters))
            {
                sReturnUrl = sReturnUrl + sParameters;
            }

            srcPage.Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + srcPage.Server.UrlEncode(sReturnUrl));
        }

        /// <summary>
        /// Redirects to default page.
        /// </summary>
        /// <param name="srcPage">The SRC page.</param>
        public static void RedirectToDefaultPage(this System.Web.UI.Page srcPage)
        {
            srcPage.Response.Redirect("~/Default.aspx");
        }

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <param name="srcPage">The SRC page.</param>
        /// <returns>System.String.</returns>
        public static string GetBaseUrl(this System.Web.UI.Page srcPage)
        {
            string baseUrl = "http";

            if (srcPage.Request.IsSecureConnection)
            {
                baseUrl += "s";
            }

            baseUrl += "://";
            baseUrl += srcPage.Request.Url.Host;

            if (srcPage.Request.Url.Port != 80)
            {
                baseUrl += ":" + srcPage.Request.Url.Port;
            }

            baseUrl += srcPage.Request.ApplicationPath;

            if (baseUrl.Substring(baseUrl.Length - 1, 1) != "/")
            {
                baseUrl += "/";
            }
            return baseUrl;
        }
    }
}