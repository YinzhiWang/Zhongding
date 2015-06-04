using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Common.Enums;

namespace ZhongDing.Web
{
    public partial class NoAccess : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.Home;
            ShowErrorMessage("没有权限访问", 1000 * 60 * 60);
        }


        protected override EPermission PagePermissionID()
        {
            return EPermission.NULL;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.None;
        }
    }
}