using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web
{
    public class BasePage : Page
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

        private IUsersRepository _PageUsersRepository;
        protected IUsersRepository PageUsersRepository
        {
            get
            {
                if (_PageUsersRepository == null)
                    _PageUsersRepository = new UsersRepository();

                return _PageUsersRepository;
            }
        }

        #endregion

        #region 虚方法
        /// <summary>
        /// 虚方法：获取页面权限ID，每个需要控制权限的页面都要override该方法
        /// </summary>
        /// <returns>System.Int32.</returns>
        //protected virtual int PagePermissionID()
        //{
        //    return (int)EPermission.NULL;
        //}

        #endregion


        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnPreInit(EventArgs e)
        {
            if (User.Identity.IsAuthenticated
                && CurrentUser.UserID > 0)//已登录用户检查用户权限
            {
                //string[] userRoles = Roles.GetRolesForUser();

                //IList<int> rolePermossionIDs = PagePermissionRepository.GetPermissionsByRoleNames(userRoles).Select(x => x.ID).ToList();

                //int currentPagePermissionID = this.PagePermissionID();

                //if (!rolePermossionIDs.Contains(currentPagePermissionID)
                //    && (currentPagePermissionID == (int)EPermission.NULL))
                //    this.RedirectToNoAccessPage(); //没有权限访问
            }
            else //未登录用户强制退出并清理session后跳转到登录页面
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                Response.Redirect(FormsAuthentication.LoginUrl, true);
            }

            base.OnPreInit(e);
        }

        public string BaseUrl
        {
            get
            {
                return PageEnhance.GetBaseUrl(this);
            }
        }

        /// <summary>
        /// 直接跳转到无权限访问页面
        /// </summary>
        protected void RedirectToNoAccessPage()
        {
            Response.Redirect("~/NoAccess.aspx", true); //没有权限访问
        }

    }
}