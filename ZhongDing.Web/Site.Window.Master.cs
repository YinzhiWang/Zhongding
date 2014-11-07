using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Common;

namespace ZhongDing.Web
{
    public partial class Site_Window : MasterPage
    {
        /// <summary>
        /// 公用的提示控件
        /// </summary>
        public RadNotification BaseNotification
        {
            get
            {
                return this.radNotification;
            }
        }

        public string BaseUrl
        {
            get
            {
                return PageEnhance.GetBaseUrl(this.Page);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = Page.Title + " - " + GlobalConst.WEBSITE_MASTER_PAGE_MAIN_TITLE;
        }
    }
}