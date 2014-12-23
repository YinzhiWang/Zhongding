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

namespace ZhongDing.Web.Views.Procures
{
    public partial class ProcureOrderMaintenance : WorkflowBasePage
    {
        #region Members

        private IProcureOrderApplicationRepository _PageProcureOrderAppRepository;
        private IProcureOrderApplicationRepository PageProcureOrderAppRepository
        {
            get
            {
                if (_PageProcureOrderAppRepository == null)
                    _PageProcureOrderAppRepository = new ProcureOrderApplicationRepository();

                return _PageProcureOrderAppRepository;
            }
        }

        private ISupplierRepository _PageSupplierRepository;
        private ISupplierRepository PageSupplierRepository
        {
            get
            {
                if (_PageSupplierRepository == null)
                    _PageSupplierRepository = new SupplierRepository();

                return _PageSupplierRepository;
            }
        }

        private IProcureOrderAppDetailRepository _PageProcureOrderAppDetailRepository;
        private IProcureOrderAppDetailRepository PageProcureOrderAppDetailRepository
        {
            get
            {
                if (_PageProcureOrderAppDetailRepository == null)
                    _PageProcureOrderAppDetailRepository = new ProcureOrderAppDetailRepository();

                return _PageProcureOrderAppDetailRepository;
            }
        }

        private IApplicationPaymentRepository _PageAppPaymentRepository;
        private IApplicationPaymentRepository PageAppPaymentRepository
        {
            get
            {
                if (_PageAppPaymentRepository == null)
                    _PageAppPaymentRepository = new ApplicationPaymentRepository();

                return _PageAppPaymentRepository;
            }
        }

        private IBankAccountRepository _PageBankAccountRepository;
        private IBankAccountRepository PageBankAccountRepository
        {
            get
            {
                if (_PageBankAccountRepository == null)
                    _PageBankAccountRepository = new BankAccountRepository();

                return _PageBankAccountRepository;
            }
        }

        private ProcureOrderApplication _CurrentEntity;
        private ProcureOrderApplication CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageProcureOrderAppRepository.GetByID(this.CurrentEntityID);

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
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewProcureOrder);
                    else
                        _CanAccessUserIDs = PageWorkflowStatusRepository.GetCanAccessUserIDsByID(this.CurrentEntity.WorkflowStatusID);
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
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditOrder);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ProcureOrderManage;
            this.CurrentWorkFlowID = (int)EWorkflow.ProcureOrder;

            if (!IsPostBack)
            {
                BindSuppliers();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindSuppliers()
        {
            var suppliers = PageSupplierRepository.GetDropdownItems(new UISearchDropdownItem
            {
                Extension = new UISearchExtension
                {
                    CompanyID = CurrentUser.CompanyID
                }
            });

            rcbxSupplier.DataSource = suppliers;
            rcbxSupplier.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxSupplier.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxSupplier.DataBind();

            rcbxSupplier.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                divComments.Visible = true;
                divOtherSections.Visible = true;
                divIsStop.Visible = true;

                txtOrderCode.Text = this.CurrentEntity.OrderCode;
                lblCreateBy.Text = PageUsersRepository.GetUserFullNameByID(this.CurrentEntity.CreatedBy.HasValue
                    ? this.CurrentEntity.CreatedBy.Value : GlobalConst.INVALID_INT);
                rdpOrderDate.SelectedDate = this.CurrentEntity.OrderDate;
                rcbxSupplier.SelectedValue = this.CurrentEntity.SupplierID.ToString();
                rdpEstDeliveryDate.SelectedDate = this.CurrentEntity.EstDeliveryDate;
                rdpEstDeliveryDate.MinDate = this.CurrentEntity.OrderDate;
                cbxIsStop.Checked = this.CurrentEntity.IsStop;

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

                        btnAudit.Visible = false;
                        btnReturn.Visible = false;
                        divAudit.Visible = false;
                        divAppPayments.Visible = false;

                        #endregion

                        break;
                    case EWorkflowStatus.Submit:
                        #region 已提交，待审核

                        DisabledBasicInfoControls();

                        divAppPayments.Visible = false;

                        if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                            ShowAuditControls(true);
                        else
                            ShowAuditControls(false);

                        #endregion

                        break;
                    case EWorkflowStatus.ApprovedBasicInfo:
                    case EWorkflowStatus.ReturnPaymentInfo:
                        #region 基础信息已审核，待填写支付信息或支付信息退回，待修改支付信息

                        DisabledBasicInfoControls();

                        ShowAuditControls(false);

                        if (CurrentUser.UserID == this.CurrentEntity.CreatedBy
                            || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                            ShowSaveButtons(true);
                        else
                        {
                            ShowSaveButtons(false);

                            rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                        }

                        #endregion

                        break;
                    case EWorkflowStatus.AuditingOfPaymentInfo:
                        #region 已填写或修改支付信息，待审核

                        DisabledBasicInfoControls();

                        ShowSaveButtons(false);

                        rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;

                        if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                            ShowAuditControls(true);
                        else
                            ShowAuditControls(false);

                        #endregion
                        break;

                    case EWorkflowStatus.ToBePaid:
                        #region 支付信息已审核，待支付

                        DisabledBasicInfoControls();

                        ShowSaveButtons(false);

                        ShowAuditControls(false);

                        rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;

                        if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                            btnPay.Visible = true;

                        #endregion

                        break;
                    case EWorkflowStatus.Paid:
                    case EWorkflowStatus.InWarehouse:
                        #region 已支付，不能修改

                        DisabledBasicInfoControls();

                        ShowSaveButtons(false);

                        ShowAuditControls(false);

                        rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;

                        #endregion
                        break;
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
        /// 显示或隐藏暂存提交按钮
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
            btnAudit.Visible = isShow;
            btnReturn.Visible = isShow;
        }

        /// <summary>
        /// 禁用基础信息相关控件
        /// </summary>
        private void DisabledBasicInfoControls()
        {
            rdpOrderDate.Enabled = false;
            rdpEstDeliveryDate.Enabled = false;
            rcbxSupplier.Enabled = false;
            cbxIsStop.Enabled = false;
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
            btnAudit.Visible = false;
            btnReturn.Visible = false;
            divIsStop.Visible = false;
            divComments.Visible = false;
            divOtherSections.Visible = false;

            rdpOrderDate.SelectedDate = DateTime.Now;
            rdpEstDeliveryDate.MinDate = DateTime.Now;
            lblCreateBy.Text = CurrentUser.FullName;

            txtOrderCode.Text = Utility.GenerateAutoSerialNo(PageProcureOrderAppRepository.GetMaxEntityID(),
                GlobalConst.EntityAutoSerialNo.SerialNoPrefix.PROCURE_ORDER);
        }

        #endregion

        #region Events

        #region Grid Events

        protected void rgAppNotes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

        #region rgOrderProducts Events

        protected void rgOrderProducts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchProcureOrderAppDetail()
            {
                ProcureOrderApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
            };

            int totalRecords;

            var orderProducts = PageProcureOrderAppDetailRepository.GetUIList(uiSearchObj,
                rgOrderProducts.CurrentPageIndex, rgOrderProducts.PageSize, out totalRecords);

            rgOrderProducts.DataSource = orderProducts;

            rgOrderProducts.VirtualItemCount = totalRecords;
        }

        protected void rgOrderProducts_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                PageProcureOrderAppDetailRepository.DeleteByID(id);
                PageProcureOrderAppDetailRepository.Save();
            }

            rgOrderProducts.Rebind();
        }

        protected void rgOrderProducts_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;
                Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

                if (plAddCommand != null)
                {
                    if (this.CurrentEntity != null
                        && (this.CurrentEntity.CreatedBy == CurrentUser.UserID || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                        && (this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.TemporarySave
                        || this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ReturnBasicInfo))
                        plAddCommand.Visible = true;
                    else
                        plAddCommand.Visible = false;
                }
            }
        }

        protected void rgOrderProducts_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (this.CurrentEntity != null
                && (this.CurrentEntity.CreatedBy == CurrentUser.UserID || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                && (this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.TemporarySave
                || this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ReturnBasicInfo))
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

        #endregion

        #region rgAppPayments Events

        protected void rgAppPayments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationPayment
            {
                ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj, rgAppPayments.CurrentPageIndex, rgAppPayments.PageSize, out totalRecords);

            rgAppPayments.DataSource = appPayments;
            rgAppPayments.VirtualItemCount = totalRecords;

        }

        protected void rgAppPayments_ItemCreated(object sender, GridItemEventArgs e)
        {

        }

        protected void rgAppPayments_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (this.CurrentEntity != null
                && (this.CurrentEntity.CreatedBy == CurrentUser.UserID || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                && (this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedBasicInfo
                || this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ReturnPaymentInfo))
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

        protected void rgAppPayments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;

                UIApplicationPayment uiEntity = null;

                if (e.Item.ItemIndex >= 0)
                    uiEntity = (UIApplicationPayment)gridDataItem.DataItem;

                var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                if (rcbxFromAccount != null)
                {
                    var uiSearchObj = new UISearchDropdownItem
                    {
                        Extension = new UISearchExtension
                        {
                            OwnerTypeID = (int)EOwnerType.Company,
                            CompanyID = CurrentUser.CompanyID
                        }
                    };

                    if (e.Item.ItemIndex < 0)
                    {
                        var excludeItemValues = PageAppPaymentRepository
                        .GetList(x => x.ApplicationID == this.CurrentEntityID)
                        .Select(x => x.FromBankAccountID).ToList();

                        if (excludeItemValues.Count > 0)
                            uiSearchObj.ExcludeItemValues = excludeItemValues;
                    }

                    var bankAccounts = PageBankAccountRepository.GetDropdownItems(uiSearchObj);
                    rcbxFromAccount.DataSource = bankAccounts;
                    rcbxFromAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    rcbxFromAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    rcbxFromAccount.DataBind();

                    if (uiEntity != null)
                        rcbxFromAccount.SelectedValue = uiEntity.FromBankAccountID.ToString();
                }

                var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");

                if (rcbxToAccount != null)
                {
                    var uiSearchObj = new UISearchDropdownItem
                    {
                        Extension = new UISearchExtension
                        {
                            OwnerTypeID = (int)EOwnerType.Supplier,
                        }
                    };

                    if (e.Item.ItemIndex < 0)
                    {
                        var excludeItemValues = PageAppPaymentRepository
                            .GetList(x => x.ApplicationID == this.CurrentEntityID)
                            .Select(x => x.ToBankAccountID).ToList();

                        if (excludeItemValues.Count > 0)
                            uiSearchObj.ExcludeItemValues = excludeItemValues;
                    }

                    if (!string.IsNullOrEmpty(rcbxSupplier.SelectedValue))
                    {
                        int supplierID;

                        if (int.TryParse(rcbxSupplier.SelectedValue, out supplierID))
                        {
                            var includeItemValues = PageSupplierRepository
                                .GetBankAccounts(supplierID)
                                .Select(x => x.BankAccountID.HasValue ? x.BankAccountID.Value : GlobalConst.INVALID_INT)
                                .ToList();

                            if (includeItemValues.Count > 0)
                                uiSearchObj.IncludeItemValues = includeItemValues;
                        }
                    }

                    var bankAccounts = PageBankAccountRepository.GetDropdownItems(uiSearchObj);
                    rcbxToAccount.DataSource = bankAccounts;
                    rcbxToAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    rcbxToAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    rcbxToAccount.DataBind();

                    if (uiEntity != null)
                        rcbxToAccount.SelectedValue = uiEntity.ToBankAccountID.ToString();
                }

                var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                if (txtAmount != null && uiEntity != null)
                    txtAmount.DbValue = uiEntity.Amount;
            }
        }

        protected void rgAppPayments_EditCommand(object sender, GridCommandEventArgs e)
        {

        }

        protected void rgAppPayments_InsertCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;

                ApplicationPayment appPayment = new ApplicationPayment();

                var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                if (!string.IsNullOrEmpty(rcbxFromAccount.SelectedValue))
                {
                    int fromAccountID;
                    if (int.TryParse(rcbxFromAccount.SelectedValue, out fromAccountID))
                        appPayment.FromBankAccountID = fromAccountID;
                    appPayment.FromAccount = rcbxFromAccount.SelectedItem.Text;
                }

                var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");

                if (!string.IsNullOrEmpty(rcbxToAccount.SelectedValue))
                {
                    int toAccountID;
                    if (int.TryParse(rcbxToAccount.SelectedValue, out toAccountID))
                        appPayment.ToBankAccountID = toAccountID;
                    appPayment.ToAccount = rcbxToAccount.SelectedItem.Text;
                }

                var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                appPayment.Amount = (decimal?)txtAmount.Value;

                appPayment.ApplicationID = this.CurrentEntityID.Value;
                appPayment.WorkflowID = this.CurrentWorkFlowID;
                appPayment.PaymentStatusID = (int)EPaymentStatus.ToBePaid;
                appPayment.PaymentTypeID = (int)EPaymentType.Expend;

                PageAppPaymentRepository.Add(appPayment);

                PageAppPaymentRepository.Save();
            }

            rgAppPayments.Rebind();
        }

        protected void rgAppPayments_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    var appPayment = PageAppPaymentRepository.GetByID(id);

                    var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                    if (!string.IsNullOrEmpty(rcbxFromAccount.SelectedValue))
                    {
                        int fromAccountID;
                        if (int.TryParse(rcbxFromAccount.SelectedValue, out fromAccountID))
                            appPayment.FromBankAccountID = fromAccountID;
                        appPayment.FromAccount = rcbxFromAccount.SelectedItem.Text;
                    }

                    var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");

                    if (!string.IsNullOrEmpty(rcbxToAccount.SelectedValue))
                    {
                        int toAccountID;
                        if (int.TryParse(rcbxToAccount.SelectedValue, out toAccountID))
                            appPayment.ToBankAccountID = toAccountID;
                        appPayment.ToAccount = rcbxToAccount.SelectedItem.Text;
                    }

                    var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                    appPayment.Amount = (decimal?)txtAmount.Value;

                    PageAppPaymentRepository.Save();

                    rgAppPayments.Rebind();
                }
            }
        }

        protected void rgAppPayments_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                PageAppPaymentRepository.DeleteByID(id);
                PageAppPaymentRepository.Save();

                rgAppPayments.Rebind();
            }
        }

        #endregion

        #endregion

        #region Others Events

        protected void rdpOrderDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            rdpEstDeliveryDate.MinDate = e.NewDate.HasValue ? e.NewDate.Value : DateTime.Now;
        }

        #endregion

        #region Buttons Events

        /// <summary>
        /// 暂存
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            ProcureOrderApplication currentEntity = null;

            if (this.CurrentEntityID.HasValue
            && this.CurrentEntityID > 0)
                currentEntity = PageProcureOrderAppRepository.GetByID(this.CurrentEntityID);

            if (currentEntity == null)
            {
                currentEntity = new ProcureOrderApplication();
                currentEntity.OrderCode = Utility.GenerateAutoSerialNo(PageProcureOrderAppRepository.GetMaxEntityID(),
                GlobalConst.EntityAutoSerialNo.SerialNoPrefix.PROCURE_ORDER);
                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.TemporarySave;
                PageProcureOrderAppRepository.Add(currentEntity);
            }

            SaveProcureOrderApp(currentEntity);

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

        private void SaveProcureOrderApp(ProcureOrderApplication currentEntity)
        {
            currentEntity.OrderDate = rdpOrderDate.SelectedDate.HasValue
                ? rdpOrderDate.SelectedDate.Value : DateTime.Now;
            currentEntity.SupplierID = Convert.ToInt32(rcbxSupplier.SelectedValue);
            currentEntity.EstDeliveryDate = rdpEstDeliveryDate.SelectedDate;

            if (cbxIsStop.Checked)
            {
                currentEntity.IsStop = cbxIsStop.Checked;
                currentEntity.StoppedBy = CurrentUser.UserID;
                currentEntity.StoppedOn = DateTime.Now;
            }

            PageProcureOrderAppRepository.Save();

            if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = (int)EWorkflow.ProcureOrder;
                appNote.WorkflowStepID = (int)EWorkflowStep.NewProcureOrder;
                appNote.ApplicationID = currentEntity.ID;
                appNote.Note = txtComment.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            if (this.CurrentEntity != null)
            {
                switch (this.CurrentEntity.WorkflowStatusID)
                {
                    case (int)EWorkflowStatus.TemporarySave:
                    case (int)EWorkflowStatus.ReturnBasicInfo:

                        if (this.CurrentEntity.ProcureOrderAppDetail.Where(x => x.IsDeleted == false).Count() > 0)
                        {
                            this.CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.Submit;
                            SaveProcureOrderApp(this.CurrentEntity);

                            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SUBMITED_TO_AUDITTING_REDIRECT);
                        }
                        else
                        {
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                            this.Master.BaseNotification.AutoCloseDelay = 1000;
                            this.Master.BaseNotification.Show("采购货品为空，订单不能提交");
                        }

                        break;

                    case (int)EWorkflowStatus.ApprovedBasicInfo:
                    case (int)EWorkflowStatus.ReturnPaymentInfo:

                        if (PageAppPaymentRepository.GetList(x => x.ApplicationID == this.CurrentEntity.ID).Count() > 0)
                        {
                            this.CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.AuditingOfPaymentInfo;
                            PageProcureOrderAppRepository.Save();

                            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SUBMITED_TO_AUDITTING_REDIRECT);
                        }
                        else
                        {
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                            this.Master.BaseNotification.AutoCloseDelay = 1000;
                            this.Master.BaseNotification.Show("支付信息为空，订单不能提交");
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        protected void btnAudit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAuditComment.Text.Trim()))
                cvAuditComment.IsValid = false;

            if (!IsValid) return;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IProcureOrderApplicationRepository orderAppRepository = new ProcureOrderApplicationRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    orderAppRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = orderAppRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = (int)EWorkflow.ProcureOrder;
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditProcureOrder;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ApprovedBasicInfo;

                                break;

                            case (int)EWorkflowStatus.AuditingOfPaymentInfo:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditPaymentInfo;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ToBePaid;

                                break;
                        }

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                    }
                }
            }
        }

        /// <summary>
        /// 退回：审核未通过
        /// </summary>
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

                    IProcureOrderApplicationRepository orderAppRepository = new ProcureOrderApplicationRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    orderAppRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = orderAppRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = (int)EWorkflow.ProcureOrder;
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditProcureOrder;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

                                break;

                            case (int)EWorkflowStatus.AuditingOfPaymentInfo:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditPaymentInfo;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnPaymentInfo;

                                break;
                        }

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                    }
                }
            }
        }

        /// <summary>
        /// 订单支付：支付信息审核通过，可支付
        /// </summary>
        protected void btnPay_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IProcureOrderApplicationRepository orderAppRepository = new ProcureOrderApplicationRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    orderAppRepository.SetDbModel(db);
                    appPaymentRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = orderAppRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        currentEntity.WorkflowStatusID = (int)EWorkflowStatus.Paid;

                        var appPayments = appPaymentRepository.GetList(x => x.ApplicationID == currentEntity.ID);

                        foreach (var item in appPayments)
                        {
                            item.PaymentStatusID = (int)EPaymentStatus.Paid;
                        }

                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = (int)EWorkflow.ProcureOrder;
                        appNote.WorkflowStepID = (int)EWorkflowStep.ProcureOrderCashier;
                        appNote.ApplicationID = currentEntity.ID;
                        appNote.Note = "订单已支付（由系统自动生成）";
                        appNoteRepository.Add(appNote);

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}