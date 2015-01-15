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
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Sales
{
    public partial class DBAppMaintenance : WorkflowBasePage
    {
        #region Members

        private IDaBaoApplicationRepository _PageDaBaoAppRepository;
        private IDaBaoApplicationRepository PageDaBaoAppRepository
        {
            get
            {
                if (_PageDaBaoAppRepository == null)
                    _PageDaBaoAppRepository = new DaBaoApplicationRepository();

                return _PageDaBaoAppRepository;
            }
        }

        private ISalesOrderAppDetailRepository _PageSalesOrderAppDetailRepository;
        private ISalesOrderAppDetailRepository PageSalesOrderAppDetailRepository
        {
            get
            {
                if (_PageSalesOrderAppDetailRepository == null)
                    _PageSalesOrderAppDetailRepository = new SalesOrderAppDetailRepository();

                return _PageSalesOrderAppDetailRepository;
            }
        }

        private IDepartmentRepository _PageDepartmentRepository;
        private IDepartmentRepository PageDepartmentRepository
        {
            get
            {
                if (_PageDepartmentRepository == null)
                    _PageDepartmentRepository = new DepartmentRepository();

                return _PageDepartmentRepository;
            }
        }

        private IDistributionCompanyRepository _PageDistributionCompanyRepository;
        private IDistributionCompanyRepository PageDistributionCompanyRepository
        {
            get
            {
                if (_PageDistributionCompanyRepository == null)
                    _PageDistributionCompanyRepository = new DistributionCompanyRepository();

                return _PageDistributionCompanyRepository;
            }
        }

        private DaBaoApplication _CurrentEntity;
        private DaBaoApplication CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageDaBaoAppRepository.GetByID(this.CurrentEntityID);

                return _CurrentEntity;
            }
        }

        private IList<int> _CanAccessUserIDs;
        private IList<int> CanAccessUserIDs
        {
            get
            {
                if (_CanAccessUserIDs == null)
                {
                    if (this.CurrentEntity == null)
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditDBOrder);
                    else
                        _CanAccessUserIDs = PageWorkflowStatusRepository.GetCanAccessUserIDsByID(this.CurrentWorkFlowID, this.CurrentEntity.WorkflowStatusID);
                }

                return _CanAccessUserIDs;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditDBOrder);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DBOrderManage;
            this.CurrentWorkFlowID = (int)EWorkflow.DBOrder;

            if (!this.CanEditUserIDs.Contains(CurrentUser.UserID))
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.AutoCloseDelay = 1000;
                this.Master.BaseNotification.Show("您没有权限修改大包配送订单");

                return;
            }

            if (!IsPostBack)
            {
                BindDepartments();

                BindDistributionCompanies();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindDepartments()
        {
            var departments = PageDepartmentRepository.GetDropdownItems();

            rcbxDepartment.DataSource = departments;
            rcbxDepartment.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDepartment.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDepartment.DataBind();

            rcbxDepartment.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindDistributionCompanies()
        {
            var distributionCompanies = PageDistributionCompanyRepository.GetDropdownItems();

            rcbxDistributionCompany.DataSource = distributionCompanies;
            rcbxDistributionCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDistributionCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDistributionCompany.DataBind();

            rcbxDistributionCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                divComments.Visible = true;
                divOtherSections.Visible = true;

                var salesOrderApp = this.CurrentEntity.SalesOrderApplication;

                if (salesOrderApp != null)
                {
                    txtOrderCode.Text = salesOrderApp.OrderCode;
                    txtOrderDate.Text = salesOrderApp.OrderDate.ToString("yyyy/MM/dd HH:mm:ss");
                }

                rcbxDepartment.SelectedValue = this.CurrentEntity.DepartmentID.ToString();
                rcbxDistributionCompany.SelectedValue = this.CurrentEntity.DistributionCompanyID.ToString();

                lblReceiverName.Text = this.CurrentEntity.ReceiverName;
                lblReceiverPhone.Text = this.CurrentEntity.ReceiverPhone;
                lblReceiverAddress.Text = this.CurrentEntity.ReceiverAddress;

                EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                switch (workfolwStatus)
                {
                    case EWorkflowStatus.Submit:
                        #region 已从配送申请单生成订单

                        if (this.CanEditUserIDs.Contains(CurrentUser.UserID))
                            ShowSaveButtons(true);
                        else
                            DisabledBasicInfoControls();

                        #endregion

                        break;
                }
            }
            else
            {
                InitDefaultData();

                if (!this.CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show("您没有权限修改大包配送订单");
                }
            }
        }

        /// <summary>
        /// 显示或隐藏保存和提交按钮
        /// </summary>
        private void ShowSaveButtons(bool isShow)
        {
            btnSubmit.Visible = isShow;
        }

        /// <summary>
        /// 禁用基础信息相关控件
        /// </summary>
        private void DisabledBasicInfoControls()
        {
            rcbxDepartment.Enabled = false;
            rcbxDistributionCompany.Enabled = false;
            txtComment.Enabled = false;

            btnSubmit.Visible = false;
        }

        /// <summary>
        /// 初始化默认值
        /// </summary>
        private void InitDefaultData()
        {
            btnSubmit.Visible = false;
            divComments.Visible = false;
            divOtherSections.Visible = false;
        }

        #endregion

        protected void cvDepartment_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(rcbxDepartment.Text))
            {
                UISearchDropdownItem uiSearchObj = new UISearchDropdownItem()
                {
                    ItemText = rcbxDepartment.Text.Trim()
                };

                var departments = PageDepartmentRepository.GetDropdownItems(uiSearchObj);

                if (departments.Count <= 0)
                    args.IsValid = false;
            }
        }

        protected void cvDistributionCompany_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(rcbxDistributionCompany.Text))
            {
                UISearchDropdownItem uiSearchObj = new UISearchDropdownItem()
                {
                    ItemText = rcbxDistributionCompany.Text.Trim()
                };

                var distributionCompanies = PageDistributionCompanyRepository.GetDropdownItems(uiSearchObj);

                if (distributionCompanies.Count <= 0)
                    args.IsValid = false;
            }
        }

        protected void rcbxDistributionCompany_ItemDataBound(object sender, Telerik.Web.UI.RadComboBoxItemEventArgs e)
        {
            UIDropdownItem dataItem = (UIDropdownItem)e.Item.DataItem;

            if (dataItem != null)
                e.Item.Attributes["Extension"] = Utility.JsonSeralize(dataItem.Extension);
        }

        protected void rgAppNotes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationNote()
            {
                WorkflowID = this.CurrentWorkFlowID,
                ApplicationID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            var appNotes = PageAppNoteRepository.GetUIList(uiSearchObj);

            rgAppNotes.DataSource = appNotes;
        }

        protected void rgOrderProducts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchSalesOrderAppDetail
            {
                SalesOrderApplicationID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var requestProducts = PageSalesOrderAppDetailRepository
                .GetUIList(uiSearchObj, rgOrderProducts.CurrentPageIndex, rgOrderProducts.PageSize, out totalRecords);

            rgOrderProducts.DataSource = requestProducts;
        }

        protected void rgOrderProducts_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                PageSalesOrderAppDetailRepository.DeleteByID(id);
                PageSalesOrderAppDetailRepository.Save();
            }

            rgOrderProducts.Rebind();
        }

        protected void rgOrderProducts_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;
                Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

                if (plAddCommand != null)
                {
                    if (this.CurrentEntity != null
                        && this.CanEditUserIDs.Contains(CurrentUser.UserID))
                        plAddCommand.Visible = true;
                    else
                        plAddCommand.Visible = false;
                }
            }
        }

        protected void rgOrderProducts_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            if (this.CurrentEntity != null
                && this.CanEditUserIDs.Contains(CurrentUser.UserID))
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = true;
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = true;
            }
            else
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = false;
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!this.CanEditUserIDs.Contains(CurrentUser.UserID))
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.AutoCloseDelay = 1000;
                this.Master.BaseNotification.Show("您没有权限修改大包配送订单");

                return;
            }

            if (!IsValid) return;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IDaBaoApplicationRepository dbAppRepository = new DaBaoApplicationRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();
                    IDistributionCompanyRepository distCompanyRepository = new DistributionCompanyRepository();

                    dbAppRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);
                    distCompanyRepository.SetDbModel(db);

                    DaBaoApplication currentEntity = dbAppRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        currentEntity.DepartmentID = Convert.ToInt32(rcbxDepartment.SelectedValue);
                        currentEntity.DistributionCompanyID = Convert.ToInt32(rcbxDistributionCompany.SelectedValue);

                        var distCompany = distCompanyRepository.GetByID(currentEntity.DistributionCompanyID);

                        if (distCompany != null)
                        {
                            currentEntity.ReceiverName = distCompany.ReceiverName;
                            currentEntity.ReceiverPhone = distCompany.PhoneNumber;
                            currentEntity.ReceiverAddress = distCompany.Address;
                        }

                        if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
                        {
                            var appNote = new ApplicationNote();
                            appNote.WorkflowID = (int)EWorkflow.DBOrder;
                            appNote.WorkflowStepID = (int)EWorkflowStep.EditDBOrder;
                            appNote.NoteTypeID = (int)EAppNoteType.Comment;
                            appNote.ApplicationID = currentEntity.ID;
                            appNote.Note = txtComment.Text.Trim();

                            appNoteRepository.Add(appNote);
                        }

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
                    }
                    else
                    {
                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                        this.Master.BaseNotification.AutoCloseDelay = 1000;
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_REDIRECT);
                    }
                }
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.AutoCloseDelay = 1000;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_REDIRECT);
            }

        }
    }
}