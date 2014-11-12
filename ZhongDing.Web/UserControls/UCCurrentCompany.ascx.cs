using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.UserControls
{
    public partial class UCCurrentCompany : System.Web.UI.UserControl
    {
        #region 属性
        private SiteUser _CurrentUser;

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <value>The current user.</value>
        protected SiteUser CurrentUser
        {
            get
            {
                if (_CurrentUser == null)
                {
                    _CurrentUser = SiteUser.GetCurrentSiteUser();
                }

                return _CurrentUser;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}