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
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.HRM
{
    public partial class WorkflowPermissionMaintenance : BasePage
    {
        #region Members

        private IWorkflowStepRepository _PageWorkflowStepRepository;
        private IWorkflowStepRepository PageWorkflowStepRepository
        {
            get
            {
                if (_PageWorkflowStepRepository == null)
                    _PageWorkflowStepRepository = new WorkflowStepRepository();

                return _PageWorkflowStepRepository;
            }
        }
        private IUserGroupRepository _PageUserGroupRepository;
        protected IUserGroupRepository PageUserGroupRepository
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
            if ((!this.CurrentEntityID.HasValue
                || this.CurrentEntityID <= 0))
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_REDIRECT);

                return;
            }

            this.Master.MenuItemID = (int)EMenuItem.WorkflowPermissionManage;

            if (!IsPostBack)
            {
                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var currentEntity = PageWorkflowStepRepository.GetByID(this.CurrentEntityID);

                var selectedUserIDs = new List<int>();
                var selectedUserGroupIDs = new List<int>();
                if (currentEntity != null)
                {
                    lblWorkflowName.Text = currentEntity.Workflow.WorkflowName;
                    lblWorkflowStepName.Text = currentEntity.StepName;
                    {
                        selectedUserIDs = currentEntity.WorkflowStepUser
                            .Where(x => x.IsDeleted == false).Select(x => x.UserID).ToList();

                        if (selectedUserIDs.Count == 0)
                            selectedUserIDs.Add(GlobalConst.INVALID_INT);

                        var uiSearchSelectedObj = new UISearchDropdownItem();
                        uiSearchSelectedObj.IncludeItemValues = selectedUserIDs;

                        var selectedUsers = PageUsersRepository.GetDropdownItems(uiSearchSelectedObj);

                        lbxSelectedUsers.DataSource = selectedUsers;
                        lbxSelectedUsers.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                        lbxSelectedUsers.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                        lbxSelectedUsers.DataBind();
                    }
                    //---------------------------------------------------------------------------
                    {
                        selectedUserGroupIDs = currentEntity.WorkflowStepUserGroup
                            .Where(x => x.IsDeleted == false).Select(x => x.UserGroupID).ToList();
                        if (selectedUserGroupIDs.Count == 0)
                            selectedUserGroupIDs.Add(GlobalConst.INVALID_INT);

                        var uiSearchSelectedObj = new UISearchDropdownItem();
                        uiSearchSelectedObj.IncludeItemValues = selectedUserGroupIDs;

                        var selectedUserGroups = PageUserGroupRepository.GetDropdownItems(uiSearchSelectedObj);

                        lbxSelectedUserGroup.DataSource = selectedUserGroups;
                        lbxSelectedUserGroup.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                        lbxSelectedUserGroup.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                        lbxSelectedUserGroup.DataBind();
                    }
                }
                {
                    var uiSearchAllUserObj = new UISearchDropdownItem();
                    uiSearchAllUserObj.ExcludeItemValues = selectedUserIDs;


                    var allUsers = PageUsersRepository.GetDropdownItems(uiSearchAllUserObj);

                    lbxAllUsers.DataSource = allUsers;
                    lbxAllUsers.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    lbxAllUsers.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    lbxAllUsers.DataBind();
                }
                //------------------------------------------------------------------------
                {
                    var uiSearchAllUserGroupObj = new UISearchDropdownItem();
                    uiSearchAllUserGroupObj.ExcludeItemValues = selectedUserGroupIDs;

                    var allUserGroup = PageUserGroupRepository.GetDropdownItems(uiSearchAllUserGroupObj);

                    lbxAllUserGroup.DataSource = allUserGroup;
                    lbxAllUserGroup.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    lbxAllUserGroup.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    lbxAllUserGroup.DataBind();
                }
            }
        }

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var currentEntity = PageWorkflowStepRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    {
                        var oldStepUsers = currentEntity.WorkflowStepUser.Where(x => x.IsDeleted == false);

                        if (lbxSelectedUsers.Items.Count > 0)
                        {
                            foreach (RadListBoxItem item in lbxSelectedUsers.Items)
                            {
                                var curStepUser = oldStepUsers.FirstOrDefault(x => x.UserID.ToString() == item.Value);

                                if (curStepUser == null)
                                {
                                    curStepUser = new WorkflowStepUser();
                                    curStepUser.UserID = int.Parse(item.Value);

                                    currentEntity.WorkflowStepUser.Add(curStepUser);
                                }
                            }
                        }

                        //删除之前的未被选择的用户
                        foreach (var item in oldStepUsers)
                        {
                            if (!lbxSelectedUsers.Items.Any(x => x.Value == item.UserID.ToString()))
                                item.IsDeleted = true;
                        }
                    }
                    //-------------------------------------------------------------------------------------------------------------
                    {
                        var oldStepUserGroup = currentEntity.WorkflowStepUserGroup.Where(x => x.IsDeleted == false);

                        if (lbxSelectedUserGroup.Items.Count > 0)
                        {
                            foreach (RadListBoxItem item in lbxSelectedUserGroup.Items)
                            {
                                var curStepUserGroup = oldStepUserGroup.FirstOrDefault(x => x.UserGroupID.ToString() == item.Value);

                                if (curStepUserGroup == null)
                                {
                                    curStepUserGroup = new WorkflowStepUserGroup();
                                    curStepUserGroup.UserGroupID = int.Parse(item.Value);

                                    currentEntity.WorkflowStepUserGroup.Add(curStepUserGroup);
                                }
                            }
                        }

                        //删除之前的未被选择的用户
                        foreach (var item in oldStepUserGroup)
                        {
                            if (!lbxSelectedUserGroup.Items.Any(x => x.Value == item.UserGroupID.ToString()))
                                item.IsDeleted = true;
                        }
                    }
                    PageWorkflowStepRepository.Save();

                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
                }
            }
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