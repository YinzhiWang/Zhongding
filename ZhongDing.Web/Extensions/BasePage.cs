﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Web.Extensions;
using ZhongDing.Common.Extension;

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

        /// <summary>
        /// 当前实体ID
        /// </summary>
        /// <value>The current entity ID.</value>
        public int? CurrentEntityID
        {
            get
            {
                string sEntityID = Request.QueryString["EntityID"];

                int iEntityID;

                if (int.TryParse(sEntityID, out iEntityID))
                    return iEntityID;
                else
                    return null;
            }
        }

        /// <summary>
        /// 需刷新的Grid的ClientID
        /// </summary>
        /// <value>The grid client ID.</value>
        protected string GridClientID
        {
            get
            {
                return Request.QueryString["GridClientID"];
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
                && CurrentUser.UserID > 0
                && CurrentUser.CompanyID > 0)//已登录用户检查用户权限
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

                string redirectUrl = FormsAuthentication.LoginUrl;

                string requestUrl = HttpContext.Current.Request.Url == null
                    ? string.Empty : HttpContext.Current.Request.Url.AbsoluteUri;

                //如果该请求页面是在window里打开，则取父页面的url
                if (!string.IsNullOrEmpty(requestUrl) && requestUrl.IndexOf("&rwndrnd=") > -1)
                {
                    string referUrl = HttpContext.Current.Request.UrlReferrer == null
                        ? string.Empty : HttpContext.Current.Request.UrlReferrer.AbsoluteUri;

                    if (!string.IsNullOrEmpty(referUrl))
                        redirectUrl += "?ReturnUrl=" + Server.UrlEncode(referUrl);
                }
                else
                    redirectUrl += "?ReturnUrl=" + Server.UrlEncode(requestUrl);

                Response.Redirect(redirectUrl, true);
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

        /// <summary>
        /// 获取url的string参数.
        /// </summary>
        /// <param name="parameterKey">The parameter key.</param>
        /// <returns>System.String.</returns>
        public string StrParameter(string parameterKey)
        {
            string parameterValue = "";
            if (Request.QueryString[parameterKey] != null)
            {
                parameterValue = Request.QueryString[parameterKey].ToString();
            }
            else
            {
                parameterValue = "";
            }
            return parameterValue;
        }
        /// <summary>
        /// 获取url的int参数
        /// </summary>
        /// <param name="parameterKey"></param>
        /// <returns></returns>
        public int? IntParameter(string parameterKey)
        {
            int? parameterValue = null;
            if (Request.QueryString[parameterKey] != null)
            {
                int temp = 0;
                if (int.TryParse(Request.QueryString[parameterKey].ToString(), out temp))
                {
                    parameterValue = temp;
                }
            }
            return parameterValue;
        }
        /// <summary>
        /// 获取url的DateTime参数
        /// </summary>
        /// <param name="parameterKey"></param>
        /// <returns></returns>
        public DateTime? DateTimeParameter(string parameterKey)
        {
            DateTime? parameterValue = null;
            if (Request.QueryString[parameterKey] != null)
            {
                string tempStr = Request.QueryString[parameterKey].ToString();
                DateTime tempDateTime;
                if (DateTime.TryParse(tempStr, out tempDateTime))
                {
                    parameterValue = tempDateTime;
                }
            }
            return parameterValue;
        }

        public void ShowErrorMessage(string msg, string onClientHidden = null)
        {
            SiteMaster siteMaster = (SiteMaster)this.Master;
            if (onClientHidden.IsNotNullOrEmpty())
                siteMaster.BaseNotification.OnClientHidden = onClientHidden;
            siteMaster.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
            siteMaster.BaseNotification.AutoCloseDelay = 1000;
            siteMaster.BaseNotification.Show(msg);
        }
        public void ShowSuccessMessage(string msg, string onClientHidden = null)
        {
            SiteMaster siteMaster = (SiteMaster)this.Master;
            if (onClientHidden.IsNotNullOrEmpty())
                siteMaster.BaseNotification.OnClientHidden = onClientHidden;
            siteMaster.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
            siteMaster.BaseNotification.AutoCloseDelay = 1000;
            siteMaster.BaseNotification.Show(msg);
        }
    }
}