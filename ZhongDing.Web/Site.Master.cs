using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZhongDing.Web
{
    public partial class SiteMaster : MasterPage
    {
        private const string SYSTEM_NAME = "众鼎医药咨询信息系统";

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

        public string BaseUrl
        {
            get
            {
                return PageEnhance.GetBaseUrl(this.Page);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = Page.Title + " - " + SYSTEM_NAME;
        }
    }
}