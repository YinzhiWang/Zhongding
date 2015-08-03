using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Business.Repositories.Reminders;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.Reminder.UserControls
{
    public class BaseReminder : BaseUserControl
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
        private IPermissionRepository _PagePermissionRepository;
        protected IPermissionRepository PagePermissionRepository
        {
            get
            {
                if (_PagePermissionRepository == null)
                    _PagePermissionRepository = new PermissionRepository();

                return _PagePermissionRepository;
            }
        }


        #endregion

        #region 虚方法
        /// <summary>
        /// 虚方法：获取页面权限ID，每个需要控制权限的页面都要override该方法
        /// </summary>
        /// <returns>System.Int32.</returns>
        protected virtual EPermission PagePermissionID()
        {
            return EPermission.NULL;
        }
        protected virtual EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.None;
        }
        public virtual void BindData(bool isBindData)
        {
            IsBindData = isBindData;
        }
        protected bool IsBindData
        {
            get
            {
                var hfIsBindData = (HiddenField)FindControl("hfIsBindData");
                if (hfIsBindData != null)
                {
                    return bool.Parse(hfIsBindData.Value);
                }
                return false;
            }
            set
            {
                var hfIsBindData = (HiddenField)FindControl("hfIsBindData");
                if (hfIsBindData != null)
                {
                    hfIsBindData.Value = value.ToString();
                }
            }
        }

        #endregion

        protected EPermissionOption PermissionOption = EPermissionOption.None;

        protected bool IsSystemAdminUser
        {
            get
            {
                int userID = CurrentUser.UserID;
                return userID == GlobalConst.DEFAULT_SYSTEM_ADMIN_USERID;
            }
        }
        public bool HasPermissionCheck(IDictionary<int, int> permossionIDAndValues)
        {
            int userID = CurrentUser.UserID;
            int currentPagePermissionID = (int)this.PagePermissionID();

            if (currentPagePermissionID != (int)EPermission.NULL)
            {
                if (userID != GlobalConst.DEFAULT_SYSTEM_ADMIN_USERID)
                {
                    if (!permossionIDAndValues.ContainsKey(currentPagePermissionID))
                    {
                        return false; //没有权限访问
                    }
                    else
                    {
                        EPermissionOption permissionValue = PermissionManager.ConverEnum(permossionIDAndValues[currentPagePermissionID]);
                        this.PermissionOption = permissionValue;
                        EPermissionOption pageAccessEPermissionOption = this.PageAccessEPermissionOption();
                        if (pageAccessEPermissionOption != EPermissionOption.None)
                        {
                            if (!PermissionManager.HasRight(permissionValue, pageAccessEPermissionOption))
                            {
                                return false; //没有权限访问
                            }
                        }
                    }
                }
            }
            return true;
        }

        protected bool HasPermissionCreate
        {
            get
            {
                return IsSystemAdminUser || PermissionManager.HasRight(this.PermissionOption, EPermissionOption.Create);
            }
        }
        protected bool HasPermissionEdit
        {
            get
            {
                return IsSystemAdminUser || PermissionManager.HasRight(this.PermissionOption, EPermissionOption.Edit);
            }
        }
        protected bool HasPermissionDelete
        {
            get
            {
                return IsSystemAdminUser || PermissionManager.HasRight(this.PermissionOption, EPermissionOption.Delete);
            }
        }
        protected bool HasPermissionView
        {
            get
            {
                return IsSystemAdminUser || PermissionManager.HasRight(this.PermissionOption, EPermissionOption.View);
            }
        }
        protected bool HasPermissionPrint
        {
            get
            {
                return IsSystemAdminUser || PermissionManager.HasRight(this.PermissionOption, EPermissionOption.Print);
            }
        }
        protected bool HasPermissionExport
        {
            get
            {
                return IsSystemAdminUser || PermissionManager.HasRight(this.PermissionOption, EPermissionOption.Export);
            }
        }



        #region Repository s
        private IReminderRepository _PageReminderRepository;
        protected IReminderRepository PageReminderRepository
        {
            get
            {
                if (_PageReminderRepository == null)
                    _PageReminderRepository = new ReminderRepository();

                return _PageReminderRepository;
            }
        }
        #endregion




    }
}