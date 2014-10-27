using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;

namespace ZhongDing.Web.Account
{
    public partial class Login : System.Web.UI.Page
    {
        private const string ERROR_MSG_INVALID_USERNAME_OR_PWD = "用户名或密码错误";
        #region Members

        private IUsersRepository _PageUsersRepository;
        private IUsersRepository PageUsersRepository
        {
            get
            {
                if (_PageUsersRepository == null)
                {
                    _PageUsersRepository = new UsersRepository();
                }
                return _PageUsersRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string userPassword = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(userName)
                || string.IsNullOrEmpty(userPassword))
            {
                if (string.IsNullOrEmpty(userName))
                    hdnErrorMsg.Value = "请输入您的用户名";
                if (string.IsNullOrEmpty(userPassword))
                    hdnErrorMsgPwd.Value = "请输入您的密码";
                return;
            }
            else
            {
                MembershipUser mUser = Membership.GetUser(userName);

                if (mUser == null)//通过Email获取用户
                {
                    userName = Membership.GetUserNameByEmail(userName);

                    if (!string.IsNullOrEmpty(userName))
                        mUser = Membership.GetUser(userName);
                }

                if (mUser == null)
                {
                    hdnErrorMsg.Value = ERROR_MSG_INVALID_USERNAME_OR_PWD;
                    return;
                }
                else
                {
                    if (mUser.IsLockedOut)
                    {
                        DateTime lastLockedOutDate = mUser.LastLockoutDate;
                        int lockedOutTime = Utility.GetDateDiff(lastLockedOutDate, DateTime.Now, DateDiffType.Minute);

                        if (lockedOutTime >= WebConfig.LockedOutTimeout)
                            mUser.UnlockUser();
                        else
                        {

                            hdnErrorMsg.Value = "用户被锁定，请" + WebConfig.LockedOutTimeout + "分钟后再登录或联系管理员解锁。";
                            return;
                        }
                    }

                    var user = PageUsersRepository.GetByUserName(mUser.UserName);

                    if (user != null)
                    {
                        bool isValidUser = Membership.ValidateUser(userName, userPassword);

                        if (!isValidUser)//验证用户失败
                        {
                            //PageUserLogRepository.SaveUserLog(user.UserID, "用户：" + mUser.UserName + " 尝试登录系统，登录失败");

                            hdnErrorMsg.Value = ERROR_MSG_INVALID_USERNAME_OR_PWD;
                            return;
                        }
                        else //验证成功
                        {
                            FormsAuthentication.SetAuthCookie(userName, false);

                            //PageUserLogRepository.SaveUserLog(user.UserID, "用户：" + mUser.UserName + " 成功登录系统");

                            if (Request["ReturnUrl"] != null)
                                Response.Redirect(Server.UrlDecode(Request["ReturnUrl"]));
                            else
                                Response.Redirect(FormsAuthentication.DefaultUrl, true);
                        }
                    }
                    else
                    {
                        hdnErrorMsg.Value = ERROR_MSG_INVALID_USERNAME_OR_PWD;
                        return;
                    }
                }
            }
        }
    }
}