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
    public partial class UserGroupMaintenance : BasePage
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
                    txtComment.Text = currentEntity.Comment;
                    txtGroupName.Text = currentEntity.GroupName;

                    //lblWorkflowName.Text = currentEntity.Workflow.WorkflowName;
                    //lblWorkflowStepName.Text = currentEntity.StepName;

                    selectedUserIDs = currentEntity.UserGroupUser
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

                var uiSearchAllObj = new UISearchDropdownItem();
                uiSearchAllObj.ExcludeItemValues = selectedUserIDs;

                var allUsers = PageUsersRepository.GetDropdownItems(uiSearchAllObj);

                lbxAllUsers.DataSource = allUsers;
                lbxAllUsers.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                lbxAllUsers.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                lbxAllUsers.DataBind();
            }
            else
            {
                btnDelete.Visible = false;

            }
        }


        protected void cvCompanyName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (PageUserGroupRepository.GetList(x => x.ID != this.CurrentEntityID
                && x.GroupName.Equals(txtGroupName.Text.Trim())).Count() > 0)
            {
                args.IsValid = false;
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
                currentEntity.GroupName = txtGroupName.Text.Trim();
                currentEntity.Comment = txtComment.Text.Trim();

                var oldStepUsers = currentEntity.UserGroupUser.Where(x => x.IsDeleted == false);

                if (lbxSelectedUsers.Items.Count > 0)
                {
                    foreach (RadListBoxItem item in lbxSelectedUsers.Items)
                    {
                        var curStepUser = oldStepUsers.FirstOrDefault(x => x.UserID.ToString() == item.Value);

                        if (curStepUser == null)
                        {
                            curStepUser = new UserGroupUser();
                            curStepUser.UserID = int.Parse(item.Value);

                            currentEntity.UserGroupUser.Add(curStepUser);
                        }
                    }
                }

                //删除之前的未被选择的用户
                foreach (var item in oldStepUsers)
                {
                    if (!lbxSelectedUsers.Items.Any(x => x.Value == item.UserID.ToString()))
                        item.IsDeleted = true;
                }


                PageUserGroupRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                   && this.CurrentEntityID > 0)
            {
                PageUserGroupRepository.DeleteByID(this.CurrentEntityID);
                PageUserGroupRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }
    }
}