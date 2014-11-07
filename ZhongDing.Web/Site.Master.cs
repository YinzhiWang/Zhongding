using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Common;

namespace ZhongDing.Web
{
    public partial class SiteMaster : MasterPage
    {
        private int _MenuItemID;
        public int MenuItemID
        {
            get
            {
                if (_MenuItemID <= 0)
                    _MenuItemID = 1;

                return _MenuItemID;
            }
            set
            {
                _MenuItemID = value;
            }
        }

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

        /// <summary>
        /// 公用的脚本管理控件
        /// </summary>
        /// <value>The base script manager.</value>
        public RadScriptManager BaseScriptManager
        {
            get
            {
                return this.radScriptManager;
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