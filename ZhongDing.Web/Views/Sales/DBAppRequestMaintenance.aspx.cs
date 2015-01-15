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
    public partial class DBAppRequestMaintenance : WorkflowBasePage
    {
        #region Members

        private IDaBaoRequestApplicationRepository _PageDaBaoRequestAppRepository;
        private IDaBaoRequestApplicationRepository PageDaBaoRequestAppRepository
        {
            get
            {
                if (_PageDaBaoRequestAppRepository == null)
                    _PageDaBaoRequestAppRepository = new DaBaoRequestApplicationRepository();

                return _PageDaBaoRequestAppRepository;
            }
        }

        private IDaBaoRequestAppDetailRepository _PageDaBaoRequestAppDetailRepository;
        private IDaBaoRequestAppDetailRepository PageDaBaoRequestAppDetailRepository
        {
            get
            {
                if (_PageDaBaoRequestAppDetailRepository == null)
                    _PageDaBaoRequestAppDetailRepository = new DaBaoRequestAppDetailRepository();

                return _PageDaBaoRequestAppDetailRepository;
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

        private DaBaoRequestApplication _CurrentEntity;
        private DaBaoRequestApplication CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageDaBaoRequestAppRepository.GetByID(this.CurrentEntityID);

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
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewDBOrderRequest);
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
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditDBOrderRequest);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DBOrderRequestManage;
            this.CurrentWorkFlowID = (int)EWorkflow.DBOrderRequest;

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

                lblCreateBy.Text = PageUsersRepository.GetUserFullNameByID(this.CurrentEntity.CreatedBy.HasValue
                    ? this.CurrentEntity.CreatedBy.Value : GlobalConst.INVALID_INT);

                rcbxDepartment.SelectedValue = this.CurrentEntity.DepartmentID.ToString();
                rcbxDistributionCompany.SelectedValue = this.CurrentEntity.DistributionCompanyID.ToString();

                lblReceiverName.Text = this.CurrentEntity.ReceiverName;
                lblReceiverPhone.Text = this.CurrentEntity.ReceiverPhone;
                lblReceiverAddress.Text = this.CurrentEntity.ReceiverAddress;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    btnSave.Visible = true;
                    btnSubmit.Visible = false;
                    ShowAuditControls(false);
                }
                else
                {
                    EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                    switch (workfolwStatus)
                    {
                        case EWorkflowStatus.TemporarySave:
                        case EWorkflowStatus.ReturnBasicInfo:
                            #region 暂存和基础信息退回（订单创建者才能修改）

                            if (CurrentUser.UserID == this.CurrentEntity.CreatedBy
                                || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                                ShowSaveButtons(true);
                            else
                                DisabledBasicInfoControls();

                            btnExportToOrder.Visible = false;
                            btnReturn.Visible = false;
                            divAudit.Visible = false;

                            #endregion

                            break;
                        case EWorkflowStatus.Submit:
                            #region 已提交，待审核

                            DisabledBasicInfoControls();

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                                ShowAuditControls(true);
                            else
                                ShowAuditControls(false);

                            #endregion

                            break;

                        case EWorkflowStatus.ExportToDBOrder:
                            #region 已生成配送订单，不能修改

                            DisabledBasicInfoControls();

                            ShowSaveButtons(false);

                            ShowAuditControls(false);

                            #endregion
                            break;
                    }

                }
            }
            else
            {
                InitDefaultData();

                if (!this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                {
                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show("您没有权限新增采购订单");
                }
            }
        }

        /// <summary>
        /// 显示或隐藏保存和提交按钮
        /// </summary>
        private void ShowSaveButtons(bool isShow)
        {
            btnSave.Visible = isShow;
            btnSubmit.Visible = isShow;
        }

        /// <summary>
        /// 显示或隐藏审核相关控件
        /// </summary>
        private void ShowAuditControls(bool isShow)
        {
            divAudit.Visible = isShow;
            btnExportToOrder.Visible = isShow;
            btnReturn.Visible = isShow;
        }

        /// <summary>
        /// 禁用基础信息相关控件
        /// </summary>
        private void DisabledBasicInfoControls()
        {
            rcbxDepartment.Enabled = false;
            rcbxDistributionCompany.Enabled = false;
            txtComment.Enabled = false;

            btnSave.Visible = false;
            btnSubmit.Visible = false;
        }

        /// <summary>
        /// 初始化默认值
        /// </summary>
        private void InitDefaultData()
        {
            btnSubmit.Visible = false;
            btnExportToOrder.Visible = false;
            btnReturn.Visible = false;
            divComment.Visible = false;
            divComments.Visible = false;
            divOtherSections.Visible = false;

            lblCreateBy.Text = CurrentUser.FullName;
        }

        /// <summary>
        /// 保存大包申请单基本信息
        /// </summary>
        private void SaveDBRequestAppBasicData(DaBaoRequestApplication currentEntity)
        {
            currentEntity.DepartmentID = Convert.ToInt32(rcbxDepartment.SelectedValue);
            currentEntity.DistributionCompanyID = Convert.ToInt32(rcbxDistributionCompany.SelectedValue);

            var distributionCompany = PageDistributionCompanyRepository.GetByID(currentEntity.DistributionCompanyID);

            if (distributionCompany != null)
            {
                currentEntity.ReceiverName = distributionCompany.ReceiverName;
                currentEntity.ReceiverPhone = distributionCompany.PhoneNumber;
                currentEntity.ReceiverAddress = distributionCompany.Address;
            }

            PageDaBaoRequestAppRepository.Save();

            if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = (int)EWorkflow.DBOrderRequest;
                appNote.NoteTypeID = (int)EAppNoteType.Comment;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                    appNote.WorkflowStepID = (int)EWorkflowStep.EditDBOrderRequest;
                else
                    appNote.WorkflowStepID = (int)EWorkflowStep.NewDBOrderRequest;

                appNote.ApplicationID = currentEntity.ID;
                appNote.Note = txtComment.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }
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

        protected void rcbxDistributionCompany_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            UIDropdownItem dataItem = (UIDropdownItem)e.Item.DataItem;

            if (dataItem != null)
                e.Item.Attributes["Extension"] = Utility.JsonSeralize(dataItem.Extension);
        }

        protected void rgAppNotes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationNote()
            {
                WorkflowID = this.CurrentWorkFlowID,
                NoteTypeID = (int)EAppNoteType.Comment,
                ApplicationID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            var appNotes = PageAppNoteRepository.GetUIList(uiSearchObj);

            rgAppNotes.DataSource = appNotes;
        }

        protected void rgAuditNotes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationNote()
            {
                WorkflowID = this.CurrentWorkFlowID,
                NoteTypeID = (int)EAppNoteType.AuditOpinion,
                ApplicationID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            var appNotes = PageAppNoteRepository.GetUIList(uiSearchObj);

            rgAuditNotes.DataSource = appNotes;
        }

        protected void rgRequestProducts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchDaBaoAppDetail
            {
                DaBaoRequestApplicationID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var requestProducts = PageDaBaoRequestAppDetailRepository
                .GetUIList(uiSearchObj, rgRequestProducts.CurrentPageIndex, rgRequestProducts.PageSize, out totalRecords);

            rgRequestProducts.DataSource = requestProducts;
        }

        protected void rgRequestProducts_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                PageDaBaoRequestAppDetailRepository.DeleteByID(id);
                PageDaBaoRequestAppDetailRepository.Save();
            }

            rgRequestProducts.Rebind();
        }

        protected void rgRequestProducts_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;
                Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

                if (plAddCommand != null)
                {
                    if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                            || (this.CurrentEntity.CreatedBy == CurrentUser.UserID
                                && (this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.TemporarySave
                                    || this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ReturnBasicInfo))))
                        plAddCommand.Visible = true;
                    else
                        plAddCommand.Visible = false;
                }
            }
        }

        protected void rgRequestProducts_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                    || (this.CurrentEntity.CreatedBy == CurrentUser.UserID
                        && (this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.TemporarySave
                            || this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ReturnBasicInfo))))
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            DaBaoRequestApplication currentEntity = null;

            if (this.CurrentEntityID.HasValue
            && this.CurrentEntityID > 0)
                currentEntity = PageDaBaoRequestAppRepository.GetByID(this.CurrentEntityID);

            if (currentEntity == null)
            {
                currentEntity = new DaBaoRequestApplication();
                currentEntity.CompanyID = CurrentUser.CompanyID;
                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.TemporarySave;
                PageDaBaoRequestAppRepository.Add(currentEntity);
            }

            SaveDBRequestAppBasicData(currentEntity);

            hdnCurrentEntityID.Value = currentEntity.ID.ToString();

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "refreshMaintenancePage";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REFRESH);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntity != null)
            {
                EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                switch (workfolwStatus)
                {
                    case EWorkflowStatus.TemporarySave:
                    case EWorkflowStatus.ReturnBasicInfo:
                        if (this.CurrentEntity.DaBaoRequestAppDetail.Where(x => x.IsDeleted == false).Count() > 0)
                        {
                            this.CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.Submit;

                            SaveDBRequestAppBasicData(this.CurrentEntity);

                            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                        }
                        else
                        {
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                            this.Master.BaseNotification.AutoCloseDelay = 1000;
                            this.Master.BaseNotification.Show("货品为空，申请单不能提交");
                        }

                        break;
                }
            }
        }

        protected void btnExportToOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAuditComment.Text.Trim()))
                cvAuditComment.IsValid = false;

            if (!IsValid) return;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IDaBaoRequestApplicationRepository dbRequestAppRepository = new DaBaoRequestApplicationRepository();
                    ISalesOrderApplicationRepository salesOrderAppRepository = new SalesOrderApplicationRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    dbRequestAppRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = dbRequestAppRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = (int)EWorkflow.DBOrderRequest;
                        appNote.WorkflowStepID = (int)EWorkflowStep.AuditDBOrderRequest;
                        appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                        appNote.ApplicationID = currentEntity.ID;
                        appNote.Note = txtAuditComment.Text.Trim();

                        appNoteRepository.Add(appNote);

                        var dbOrderApp = new DaBaoApplication
                        {
                            CompanyID = currentEntity.CompanyID,
                            WorkflowStatusID = (int)EWorkflowStatus.Submit,
                            DepartmentID = currentEntity.DepartmentID,
                            DistributionCompanyID = currentEntity.DistributionCompanyID,
                            ReceiverName = currentEntity.ReceiverName,
                            ReceiverPhone = currentEntity.ReceiverPhone,
                            ReceiverAddress = currentEntity.ReceiverAddress
                        };

                        var salesOrderApp = new SalesOrderApplication
                        {
                            OrderDate = DateTime.Now,
                            SaleOrderTypeID = (int)ESaleOrderType.DaBaoMode
                        };

                        salesOrderApp.OrderCode = Utility.GenerateAutoSerialNo(salesOrderAppRepository.GetMaxEntityID(),
                            GlobalConst.EntityAutoSerialNo.SerialNoPrefix.DABAO_ORDER);

                        foreach (var item in currentEntity.DaBaoRequestAppDetail.Where(x => x.IsDeleted == false))
                        {
                            var salesOrderAppDetail = new SalesOrderAppDetail
                            {
                                ProductID = item.ProductID,
                                ProductSpecificationID = item.ProductSpecificationID,
                                SalesPrice = item.SalesPrice,
                                Count = item.Count,
                                TotalSalesAmount = item.TotalSalesAmount
                            };

                            salesOrderApp.SalesOrderAppDetail.Add(salesOrderAppDetail);
                        }

                        dbOrderApp.SalesOrderApplication = salesOrderApp;

                        currentEntity.DaBaoApplication = dbOrderApp;

                        currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ExportToDBOrder;

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                    }
                }
            }

        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAuditComment.Text.Trim()))
                cvAuditComment.IsValid = false;

            if (!IsValid) return;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IDaBaoRequestApplicationRepository dbRequestAppRepository = new DaBaoRequestApplicationRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    dbRequestAppRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = dbRequestAppRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = (int)EWorkflow.DBOrderRequest;
                        appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditDBOrderRequest;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

                                break;
                        }

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                    }
                }
            }
        }

    }
}