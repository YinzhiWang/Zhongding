using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.HRM
{
    public partial class UserGroupManagement : BasePage
    {
        #region Members

        private IUserGroupRepository _PageUserGroupRepository;
        private IUserGroupRepository PageUserGroupRepository
        {
            get
            {
                if (_PageUserGroupRepository == null)
                    _PageUserGroupRepository = new UserGroupRepository();

                return _PageUserGroupRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.UserGroupManage;

        }

        private void BindUserGroup(bool isNeedRebind)
        {
            UISearchUserGroup uiSearchObj = new UISearchUserGroup()
            {
                GroupName = txtGroupName.Text.Trim()
            };

            int totalRecords = 0;

            var uiUserGroups = PageUserGroupRepository.GetUIList(uiSearchObj, rgUserGroups.CurrentPageIndex, rgUserGroups.PageSize, out totalRecords);

            rgUserGroups.VirtualItemCount = totalRecords;

            rgUserGroups.DataSource = uiUserGroups;

            if (isNeedRebind)
                rgUserGroups.Rebind();
        }


        protected void rgUserGroups_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindUserGroup(false);
        }

        protected void rgUserGroups_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {

                PageUserGroupRepository.DeleteByID(id);
                PageUserGroupRepository.Save();
            }

            rgUserGroups.Rebind();
        }

        protected void rgUserGroups_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            base.PermissionOptionCheckGridCreate(e.Item);
        }

        protected void rgUserGroups_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            base.PermissionOptionCheckGridDelete(e.OwnerTableView.Columns);
        }

        protected void rgUserGroups_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindUserGroup(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            txtGroupName.Text = string.Empty;

            BindUserGroup(true);
        }



        protected override EPermission PagePermissionID()
        {
            return EPermission.PermissionManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}