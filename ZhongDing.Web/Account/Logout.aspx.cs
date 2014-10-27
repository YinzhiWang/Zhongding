using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZhongDing.Web.Account
{
    public partial class Logout : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated
                && CurrentUser.UserID > 0)
            {
                //base.PageUserLogRepository.SaveUserLog(CurrentUser.UserID, "用户：" + CurrentUser.UserName + " 成功退出系统");

                FormsAuthentication.SignOut();
                Session.Clear();
            }

            Response.Redirect(FormsAuthentication.LoginUrl, true);
        }
    }
}