using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.HRM
{
    public partial class UserGroupPermissionMaintenance : BasePage
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

        private IUserGroupPermissionRepository _PageUserGroupPermissionRepository;
        private IUserGroupPermissionRepository PageUserGroupPermissionRepository
        {
            get
            {
                if (_PageUserGroupPermissionRepository == null)
                    _PageUserGroupPermissionRepository = new UserGroupPermissionRepository();
                return _PageUserGroupPermissionRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.UserGroupManage;

            if (!IsPostBack)
            {
                LoadEntityData();
            }

        }
        private void LoadEntityData()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var currentEntity = PageUserGroupRepository.GetByID(this.CurrentEntityID);

                var selectedUserIDs = new List<int>();

                if (currentEntity != null)
                {


                    lblUserGroupName.Text = currentEntity.GroupName;

                    //lblWorkflowName.Text = currentEntity.Workflow.WorkflowName;
                    //lblWorkflowStepName.Text = currentEntity.StepName;

                    selectedUserIDs = currentEntity.UserGroupUser
                        .Where(x => x.IsDeleted == false).Select(x => x.UserID).ToList();

                    if (selectedUserIDs.Count == 0)
                        selectedUserIDs.Add(GlobalConst.INVALID_INT);

                    var uiSearchSelectedObj = new UISearchDropdownItem();
                    uiSearchSelectedObj.IncludeItemValues = selectedUserIDs;

                    var selectedUsers = PageUsersRepository.GetDropdownItems(uiSearchSelectedObj);

                }

                var uiSearchAllObj = new UISearchDropdownItem();
                uiSearchAllObj.ExcludeItemValues = selectedUserIDs;


            }
            else
            {
                //btnDelete.Visible = false;

            }
        }




        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            UserGroup currentEntity = null;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                currentEntity = PageUserGroupRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new UserGroup();
                PageUserGroupRepository.Add(currentEntity);
            }

            if (currentEntity != null)
            {

                var selectedItems = rgUserGroupPermissions.Items;
                int userGroupID = this.CurrentEntityID.Value;
                foreach (var item in selectedItems)
                {
                    var editableItem = ((GridEditableItem)item);
                    int permissionID = Convert.ToInt32(editableItem.GetDataKeyValue("PermissionID").ToString());

                    var cBoxHasCreate = (CheckBox)editableItem.FindControl("cBoxHasCreate");
                    var cBoxHasEdit = (CheckBox)editableItem.FindControl("cBoxHasEdit");
                    var cBoxHasDelete = (CheckBox)editableItem.FindControl("cBoxHasDelete");
                    var cBoxHasView = (CheckBox)editableItem.FindControl("cBoxHasView");
                    var cBoxHasPrint = (CheckBox)editableItem.FindControl("cBoxHasPrint");
                    var cBoxHasExport = (CheckBox)editableItem.FindControl("cBoxHasExport");
                    UserGroupPermission userGroupPermission = PageUserGroupPermissionRepository.GetByUserGroupIDAndPermissionID(userGroupID, permissionID);

                    if (cBoxHasCreate.Checked || cBoxHasEdit.Checked || cBoxHasDelete.Checked || cBoxHasView.Checked || cBoxHasPrint.Checked || cBoxHasExport.Checked)
                    {
                        EPermissionOption permissionOption = EPermissionOption.None;

                        if (cBoxHasCreate.Checked)
                            permissionOption = permissionOption | EPermissionOption.Create;
                        if (cBoxHasEdit.Checked)
                            permissionOption = permissionOption | EPermissionOption.Edit;
                        if (cBoxHasDelete.Checked)
                            permissionOption = permissionOption | EPermissionOption.Delete;
                        if (cBoxHasView.Checked)
                            permissionOption = permissionOption | EPermissionOption.View;
                        if (cBoxHasPrint.Checked)
                            permissionOption = permissionOption | EPermissionOption.Print;
                        if (cBoxHasExport.Checked)
                            permissionOption = permissionOption | EPermissionOption.Export;


                        if (userGroupPermission == null)
                        {
                            userGroupPermission = new UserGroupPermission()
                            {
                                UserGroupID = userGroupID,
                                PermissionID = permissionID,
                                Value = (int)permissionOption,
                            };
                            PageUserGroupPermissionRepository.Add(userGroupPermission);
                        }
                        else
                        {
                            userGroupPermission.Value = (int)permissionOption;
                        }

                    }
                    else
                    {
                        if (userGroupPermission != null)
                            PageUserGroupPermissionRepository.DeleteByID(userGroupPermission.ID);
                    }
                    PageUserGroupPermissionRepository.Save();

                }


                PageUserGroupRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //if (this.CurrentEntityID.HasValue
            //       && this.CurrentEntityID > 0)
            //{
            //    PageUserGroupRepository.DeleteByID(this.CurrentEntityID);
            //    PageUserGroupRepository.Save();

            //    this.Master.BaseNotification.OnClientHidden = "onClientHidden";
            //    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            //}
        }
        private void BindUserGroupPermission(bool isNeedRebind)
        {
            UISearchUserGroupPermission uiSearchObj = new UISearchUserGroupPermission()
            {
                //GroupName = txtGroupName.Text.Trim()
                UserGroupID = this.CurrentEntityID
            };

            int totalRecords = 0;

            var uiUserGroups = PageUserGroupPermissionRepository.GetUIList(uiSearchObj, rgUserGroupPermissions.CurrentPageIndex, rgUserGroupPermissions.PageSize, out totalRecords);

            rgUserGroupPermissions.VirtualItemCount = totalRecords;

            rgUserGroupPermissions.DataSource = uiUserGroups;

            if (isNeedRebind)
                rgUserGroupPermissions.Rebind();
        }

        protected void rgUserGroupPermissions_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindUserGroupPermission(false);
        }

        protected void rgUserGroupPermissions_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {

                PageUserGroupPermissionRepository.DeleteByID(id);
                PageUserGroupPermissionRepository.Save();
            }

            rgUserGroupPermissions.Rebind();
        }

        protected void rgUserGroupPermissions_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgUserGroupPermissions_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgUserGroupPermissions_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }
    }
}