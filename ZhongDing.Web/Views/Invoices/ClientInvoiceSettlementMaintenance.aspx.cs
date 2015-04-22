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
using ZhongDing.Common.Extension;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Invoices
{
    public partial class ClientInvoiceSettlementMaintenance : WorkflowBasePage
    {
        #region Members
        private IClientInvoiceSettlementRepository _PageClientInvoiceSettlementRepository;
        private IClientInvoiceSettlementRepository PageClientInvoiceSettlementRepository
        {
            get
            {
                if (_PageClientInvoiceSettlementRepository == null)
                    _PageClientInvoiceSettlementRepository = new ClientInvoiceSettlementRepository();

                return _PageClientInvoiceSettlementRepository;
            }
        }

        private IClientInvoiceSettlementDetailRepository _PageClientInvoiceSDRepository;
        private IClientInvoiceSettlementDetailRepository PageClientInvoiceSDRepository
        {
            get
            {
                if (_PageClientInvoiceSDRepository == null)
                    _PageClientInvoiceSDRepository = new ClientInvoiceSettlementDetailRepository();

                return _PageClientInvoiceSDRepository;
            }
        }

        private IClientCompanyRepository _PageClientCompanyRepository;
        private IClientCompanyRepository PageClientCompanyRepository
        {
            get
            {
                if (_PageClientCompanyRepository == null)
                    _PageClientCompanyRepository = new ClientCompanyRepository();

                return _PageClientCompanyRepository;
            }
        }

        private ClientInvoiceSettlement _CurrentEntity;
        private ClientInvoiceSettlement CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageClientInvoiceSettlementRepository.GetByID(this.CurrentEntityID);

                return _CurrentEntity;
            }
        }

        private ICompanyRepository _PageCompanyRepository;
        private ICompanyRepository PageCompanyRepository
        {
            get
            {
                if (_PageCompanyRepository == null)
                    _PageCompanyRepository = new CompanyRepository();

                return _PageCompanyRepository;
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

        private IList<int> _CanAccessUserIDs;
        private IList<int> CanAccessUserIDs
        {
            get
            {
                if (_CanAccessUserIDs == null)
                {
                    if (this.CurrentEntity == null)
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewCISettlement);
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
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditCISettlement);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.ClientInvoiceSettlement;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientInvoiceSettlementManage;

            if (!IsPostBack)
            {
                BindClientCompanies();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindClientCompanies()
        {
            var clientCompanies = PageClientCompanyRepository.GetDropdownItems();
            rcbxClientCompany.DataSource = clientCompanies;
            rcbxClientCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientCompany.DataBind();

            rcbxClientCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                lblCreateBy.Text = PageUsersRepository.GetUserFullNameByID(this.CurrentEntity.CreatedBy.HasValue
                    ? this.CurrentEntity.CreatedBy.Value : GlobalConst.INVALID_INT);

                rdpSettlementDate.SelectedDate = CurrentEntity.SettlementDate;
                rcbxClientCompany.SelectedValue = this.CurrentEntity.ClientCompanyID.ToString();

                lblTotalPayAmount.Text = CurrentEntity.TotalPayAmount.ToString("C2");
                lblCapitalAmount.Text = CurrentEntity.TotalPayAmount.ToString().ConvertToChineseMoney();

                BindPaymentSummary();

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
                        case EWorkflowStatus.ApprovedByTreasurers:
                            #region 已提交或需进入下一级审核，待审核

                            DisabledBasicInfoControls();

                            divAppPayments.Visible = false;

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                                ShowAuditControls(true);
                            else
                                ShowAuditControls(false);

                            #endregion

                            break;

                        case EWorkflowStatus.ApprovedByDeptManagers:
                            #region 审核通过，待支付

                            DisabledBasicInfoControls();
                            ShowAuditControls(false);

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                            {
                                btnPay.Visible = true;
                            }
                            else
                                rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                            #endregion
                            break;

                        case EWorkflowStatus.Paid:
                            #region 已支付，只能撤销

                            DisabledBasicInfoControls();

                            ShowSaveButtons(false);

                            ShowAuditControls(false);

                            rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;

                            if (CurrentEntity.PaidBy == CurrentUser.UserID)
                            {
                                divCancel.Visible = true;
                                btnCancel.Visible = true;
                            }

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
                    this.Master.BaseNotification.Show("您没有权限新增客户发票结算");
                }
            }
        }

        private void BindPaymentSummary()
        {
            var appPaymentAmounts = PageAppPaymentRepository
                .GetList(x => x.WorkflowID == CurrentWorkFlowID && x.ApplicationID == CurrentEntity.ID)
                .Select(x => x.Amount).ToList();

            if (appPaymentAmounts.Count > 0)
            {
                decimal totalPaymentAmount = appPaymentAmounts.Sum(x => x ?? 0);

                lblTotalPaymentAmount.Text = totalPaymentAmount.ToString("C2");
                lblCapitalTotalPaymentAmount.Text = totalPaymentAmount.ToString().ConvertToChineseMoney();
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
            btnAudit.Visible = isShow;
            btnReturn.Visible = isShow;
        }

        /// <summary>
        /// 禁用基础信息相关控件
        /// </summary>
        private void DisabledBasicInfoControls()
        {
            rdpSettlementDate.Enabled = false;
            rcbxClientCompany.Enabled = false;
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
            btnCancel.Visible = false;
            divComment.Visible = false;
            divCancel.Visible = false;
            divComments.Visible = false;
            divOtherSections.Visible = false;

            rdpSettlementDate.SelectedDate = DateTime.Now;
            lblCreateBy.Text = CurrentUser.FullName;
        }

        private void BindClientInvoices(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchClientInvoiceSettlementDetail
            {
                ClientInvoiceSettlementID = this.CurrentEntityID.HasValue
                ? CurrentEntityID.Value : GlobalConst.INVALID_INT,
                CompanyID = CurrentUser.CompanyID,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate
            };

            if (!string.IsNullOrEmpty(rcbxClientCompany.SelectedValue))
                uiSearchObj.ClientCompanyID = Convert.ToInt32(rcbxClientCompany.SelectedValue);
            else
                uiSearchObj.ClientCompanyID = GlobalConst.INVALID_INT;

            if (this.CurrentEntity != null)
            {
                if ((this.CurrentEntity.CreatedBy == CurrentUser.UserID || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                && (this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.TemporarySave
                || this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ReturnBasicInfo))
                    uiSearchObj.OnlyIncludeChecked = false;
                else
                    uiSearchObj.OnlyIncludeChecked = true;
            }

            var uiEntities = PageClientInvoiceSDRepository.GetUIList(uiSearchObj);

            rgClientInvoices.DataSource = uiEntities;

            if (isNeedRebind)
                rgClientInvoices.Rebind();
        }


        #endregion

        #region Grid Events

        #region App Notes

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

        #endregion

        #region rgClientInvoices

        protected void rgClientInvoices_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindClientInvoices(false);
        }

        protected void rgClientInvoices_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (this.CurrentEntity == null || (this.CurrentEntity != null
                && (this.CurrentEntity.CreatedBy == CurrentUser.UserID || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                && (this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.TemporarySave
                || this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ReturnBasicInfo)))
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_CLIENT_SELECT).Visible = true;
            }
            else
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_CLIENT_SELECT).Visible = false;
            }
        }

        protected void rgClientInvoices_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                UIClientInvoiceSettlementDetail uiEntity = (UIClientInvoiceSettlementDetail)gridDataItem.DataItem;

                var clientSelectColumn = rgClientInvoices.MasterTableView.GetColumn("ClientSelect");

                if (clientSelectColumn != null)
                {
                    var clientSelectCell = gridDataItem.Cells[clientSelectColumn.OrderIndex];

                    if (clientSelectCell != null)
                    {
                        if (typeof(CheckBox) == clientSelectCell.Controls[0].GetType())
                        {
                            var curCheckBox = clientSelectCell.Controls[0] as CheckBox;

                            if (uiEntity.IsChecked == false
                                && uiEntity.IsContainDeductionInvoice
                                && !uiEntity.ClientTaxDeductionRatio.HasValue)
                            {
                                gridDataItem.SelectableMode = GridItemSelectableMode.None;
                            }
                            else
                            {
                                gridDataItem.Selected = uiEntity.IsChecked;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region rgAppPayments

        protected void rgAppPayments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationPayment
            {
                WorkflowID = this.CurrentWorkFlowID,
                ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj, rgAppPayments.CurrentPageIndex, rgAppPayments.PageSize, out totalRecords);

            rgAppPayments.DataSource = appPayments;
            rgAppPayments.VirtualItemCount = totalRecords;
        }

        protected void rgAppPayments_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                || (this.CanAccessUserIDs.Contains(CurrentUser.UserID)
                    && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedByDeptManagers)))
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = true;
            }
            else
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = false;
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

                var rdpPayDate = (RadDatePicker)e.Item.FindControl("rdpPayDate");
                if (rdpPayDate != null && uiEntity != null)
                    rdpPayDate.SelectedDate = uiEntity.PayDate;
                else
                    rdpPayDate.SelectedDate = DateTime.Now;

                var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                if (rcbxFromAccount != null)
                {
                    var uiSearchObj = new UISearchDropdownItem
                    {
                        Extension = new UISearchExtension
                        {
                            OwnerTypeID = (int)EOwnerType.Company,
                            CompanyID = CurrentEntity.CompanyID
                        }
                    };

                    var bankAccounts = PageBankAccountRepository.GetDropdownItems(uiSearchObj);
                    rcbxFromAccount.DataSource = bankAccounts;
                    rcbxFromAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    rcbxFromAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    rcbxFromAccount.DataBind();

                    if (uiEntity != null)
                        rcbxFromAccount.SelectedValue = uiEntity.FromBankAccountID.ToString();
                }

                var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                if (txtAmount != null && uiEntity != null)
                    txtAmount.DbValue = uiEntity.Amount;

                var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                if (txtFee != null && uiEntity != null)
                    txtFee.DbValue = uiEntity.Fee;
            }
        }

        protected void rgAppPayments_InsertCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;

                ApplicationPayment appPayment = new ApplicationPayment();

                var rdpPayDate = (RadDatePicker)e.Item.FindControl("rdpPayDate");
                appPayment.PayDate = rdpPayDate.SelectedDate;

                var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                if (!string.IsNullOrEmpty(rcbxFromAccount.SelectedValue))
                {
                    int fromAccountID;
                    if (int.TryParse(rcbxFromAccount.SelectedValue, out fromAccountID))
                        appPayment.FromBankAccountID = fromAccountID;
                    appPayment.FromAccount = rcbxFromAccount.SelectedItem.Text;
                }

                var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                appPayment.Amount = (decimal?)txtAmount.Value;

                var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                appPayment.Fee = (decimal?)txtFee.Value;

                appPayment.ApplicationID = this.CurrentEntityID.Value;
                appPayment.WorkflowID = this.CurrentWorkFlowID;
                appPayment.PaymentStatusID = (int)EPaymentStatus.ToBePaid;
                appPayment.PaymentTypeID = (int)EPaymentType.Expend;

                PageAppPaymentRepository.Add(appPayment);

                PageAppPaymentRepository.Save();
            }

            rgAppPayments.Rebind();

            BindPaymentSummary();
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

                    var rdpPayDate = (RadDatePicker)e.Item.FindControl("rdpPayDate");
                    appPayment.PayDate = rdpPayDate.SelectedDate;

                    var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                    if (!string.IsNullOrEmpty(rcbxFromAccount.SelectedValue))
                    {
                        int fromAccountID;
                        if (int.TryParse(rcbxFromAccount.SelectedValue, out fromAccountID))
                            appPayment.FromBankAccountID = fromAccountID;
                        appPayment.FromAccount = rcbxFromAccount.SelectedItem.Text;
                    }

                    var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                    appPayment.Amount = (decimal?)txtAmount.Value;

                    var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                    appPayment.Fee = (decimal?)txtFee.Value;

                    PageAppPaymentRepository.Save();

                    rgAppPayments.Rebind();
                }
            }

            BindPaymentSummary();
        }

        #endregion

        #endregion

        protected void rcbxClientCompany_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindClientInvoices(true);
        }

        #region Buttons Events

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindClientInvoices(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpBeginDate.Clear();
            rdpEndDate.Clear();

            BindClientInvoices(true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            var selectedItems = rgClientInvoices.SelectedItems;

            if (selectedItems.Count == 0)
            {
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.AutoCloseDelay = 1000;
                this.Master.BaseNotification.Show("请选择要结算的发票");

                return;
            }

            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                var db = unitOfWork.GetDbModel();

                IClientInvoiceRepository ciRepository = new ClientInvoiceRepository();
                IClientInvoiceSettlementRepository cisRepository = new ClientInvoiceSettlementRepository();
                IClientInvoiceSettlementDetailRepository cisdRepository = new ClientInvoiceSettlementDetailRepository();

                ciRepository.SetDbModel(db);
                cisRepository.SetDbModel(db);
                cisdRepository.SetDbModel(db);

                ClientInvoiceSettlement clientInvoiceSettlement = cisRepository.GetByID(this.CurrentEntityID);

                if (clientInvoiceSettlement == null)
                {
                    clientInvoiceSettlement = new ClientInvoiceSettlement()
                    {
                        CompanyID = CurrentUser.CompanyID,
                        WorkflowStatusID = (int)EWorkflowStatus.TemporarySave,
                    };

                    cisRepository.Add(clientInvoiceSettlement);
                }

                clientInvoiceSettlement.SettlementDate = rdpSettlementDate.SelectedDate ?? DateTime.Now;
                clientInvoiceSettlement.ClientCompanyID = Convert.ToInt32(rcbxClientCompany.SelectedValue);

                List<int> clientInvoiceIDs = new List<int>();

                foreach (var item in selectedItems)
                {
                    var editableItem = ((GridEditableItem)item);

                    int clientInvoiceID = Convert.ToInt32(editableItem.GetDataKeyValue("ClientInvoiceID").ToString());
                    int clientCompanyID = Convert.ToInt32(editableItem.GetDataKeyValue("ClientCompanyID").ToString());
                    DateTime invoiceDate = Convert.ToDateTime(editableItem.GetDataKeyValue("InvoiceDate").ToString());
                    string invoiceNumber = editableItem.GetDataKeyValue("InvoiceNumber").ToString();

                    decimal totalInvoiceAmount = 0;
                    if (editableItem.GetDataKeyValue("TotalInvoiceAmount") != null)
                        totalInvoiceAmount = Convert.ToDecimal(editableItem.GetDataKeyValue("TotalInvoiceAmount").ToString());

                    decimal? clientTaxHighRatio = null;
                    if (editableItem.GetDataKeyValue("ClientTaxHighRatio") != null)
                        clientTaxHighRatio = Convert.ToDecimal(editableItem.GetDataKeyValue("ClientTaxHighRatio").ToString());

                    decimal? clientTaxLowRatio = null;
                    if (editableItem.GetDataKeyValue("ClientTaxLowRatio") != null)
                        clientTaxLowRatio = Convert.ToDecimal(editableItem.GetDataKeyValue("ClientTaxLowRatio").ToString());

                    decimal? clientTaxDeductionRatio = null;
                    if (editableItem.GetDataKeyValue("ClientTaxDeductionRatio") != null)
                        clientTaxDeductionRatio = Convert.ToDecimal(editableItem.GetDataKeyValue("ClientTaxDeductionRatio").ToString());

                    decimal? highRatioAmount = null;
                    if (editableItem.GetDataKeyValue("HighRatioAmount") != null)
                        highRatioAmount = Convert.ToDecimal(editableItem.GetDataKeyValue("HighRatioAmount").ToString());

                    decimal? lowRatioAmount = null;
                    if (editableItem.GetDataKeyValue("LowRatioAmount") != null)
                        lowRatioAmount = Convert.ToDecimal(editableItem.GetDataKeyValue("LowRatioAmount").ToString());

                    decimal? deductionRatioAmount = null;
                    if (editableItem.GetDataKeyValue("DeductionRatioAmount") != null)
                        deductionRatioAmount = Convert.ToDecimal(editableItem.GetDataKeyValue("DeductionRatioAmount").ToString());

                    decimal payAmount = 0;
                    if (editableItem.GetDataKeyValue("PayAmount") != null)
                        payAmount = Convert.ToDecimal(editableItem.GetDataKeyValue("PayAmount").ToString());

                    var clientInvoiceSettlementDetail = clientInvoiceSettlement.ClientInvoiceSettlementDetail
                        .FirstOrDefault(x => x.IsDeleted == false && x.ClientInvoiceID == clientInvoiceID);

                    if (clientInvoiceSettlementDetail == null)
                    {
                        clientInvoiceSettlementDetail = new ClientInvoiceSettlementDetail();
                        clientInvoiceSettlement.ClientInvoiceSettlementDetail.Add(clientInvoiceSettlementDetail);
                    }

                    clientInvoiceSettlementDetail.ClientInvoiceID = clientInvoiceID;
                    clientInvoiceSettlementDetail.ClientCompanyID = clientCompanyID;
                    clientInvoiceSettlementDetail.InvoiceDate = invoiceDate;
                    clientInvoiceSettlementDetail.InvoiceNumber = invoiceNumber;
                    clientInvoiceSettlementDetail.TotalInvoiceAmount = totalInvoiceAmount;
                    clientInvoiceSettlementDetail.ClientTaxHighRatio = clientTaxHighRatio;
                    clientInvoiceSettlementDetail.ClientTaxLowRatio = clientTaxLowRatio;
                    clientInvoiceSettlementDetail.ClientTaxDeductionRatio = clientTaxDeductionRatio;
                    clientInvoiceSettlementDetail.HighRatioAmount = highRatioAmount;
                    clientInvoiceSettlementDetail.LowRatioAmount = lowRatioAmount;
                    clientInvoiceSettlementDetail.DeductionRatioAmount = deductionRatioAmount;
                    clientInvoiceSettlementDetail.PayAmount = payAmount;

                    clientInvoiceIDs.Add(clientInvoiceID);
                }

                foreach (var item in clientInvoiceSettlement.ClientInvoiceSettlementDetail
                    .Where(x => x.IsDeleted == false && !clientInvoiceIDs.Contains(x.ClientInvoiceID)))
                {
                    item.IsDeleted = true;

                    var clientInvoice = ciRepository.GetByID(item.ClientInvoiceID);

                    clientInvoice.IsSettled = false;
                    clientInvoice.SettledDate = null;
                }

                var clientInvoices = ciRepository.GetList(x => clientInvoiceIDs.Contains(x.ID));

                foreach (var clientInvoice in clientInvoices)
                {
                    clientInvoice.IsSettled = true;
                    clientInvoice.SettledDate = DateTime.Now;
                }

                clientInvoiceSettlement.TotalInvoiceAmount = clientInvoiceSettlement.ClientInvoiceSettlementDetail
                    .Where(x => x.IsDeleted == false)
                    .Sum(x => x.TotalInvoiceAmount);
                clientInvoiceSettlement.TotalPayAmount = clientInvoiceSettlement.ClientInvoiceSettlementDetail
                    .Where(x => x.IsDeleted == false)
                    .Sum(x => x.PayAmount);

                unitOfWork.SaveChanges();

                hdnCurrentEntityID.Value = clientInvoiceSettlement.ID.ToString();

                if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
                {
                    var appNote = new ApplicationNote();
                    appNote.WorkflowID = CurrentWorkFlowID;
                    appNote.WorkflowStepID = (int)EWorkflowStep.NewCISettlement;
                    appNote.NoteTypeID = (int)EAppNoteType.Comment;
                    appNote.ApplicationID = clientInvoiceSettlement.ID;
                    appNote.Note = txtComment.Text.Trim();

                    PageAppNoteRepository.Add(appNote);

                    PageAppNoteRepository.Save();
                }

                this.Master.BaseNotification.OnClientHidden = "refreshMaintenancePage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            var selectedItems = rgClientInvoices.SelectedItems;

            if (selectedItems.Count == 0)
            {
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.AutoCloseDelay = 1000;
                this.Master.BaseNotification.Show("请选择要结算的发票");

                return;
            }

            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                var db = unitOfWork.GetDbModel();

                IClientInvoiceRepository ciRepository = new ClientInvoiceRepository();
                IClientInvoiceSettlementRepository cisRepository = new ClientInvoiceSettlementRepository();
                IClientInvoiceSettlementDetailRepository cisdRepository = new ClientInvoiceSettlementDetailRepository();

                ciRepository.SetDbModel(db);
                cisRepository.SetDbModel(db);
                cisdRepository.SetDbModel(db);

                ClientInvoiceSettlement clientInvoiceSettlement = cisRepository.GetByID(this.CurrentEntityID);

                if (clientInvoiceSettlement != null)
                {
                    clientInvoiceSettlement.WorkflowStatusID = (int)EWorkflowStatus.Submit;
                    clientInvoiceSettlement.SettlementDate = rdpSettlementDate.SelectedDate ?? DateTime.Now;
                    clientInvoiceSettlement.ClientCompanyID = Convert.ToInt32(rcbxClientCompany.SelectedValue);

                    List<int> clientInvoiceIDs = new List<int>();

                    foreach (var item in selectedItems)
                    {
                        var editableItem = ((GridEditableItem)item);

                        int clientInvoiceID = Convert.ToInt32(editableItem.GetDataKeyValue("ClientInvoiceID").ToString());
                        int clientCompanyID = Convert.ToInt32(editableItem.GetDataKeyValue("ClientCompanyID").ToString());
                        DateTime invoiceDate = Convert.ToDateTime(editableItem.GetDataKeyValue("InvoiceDate").ToString());
                        string invoiceNumber = editableItem.GetDataKeyValue("InvoiceNumber").ToString();

                        decimal totalInvoiceAmount = 0;
                        if (editableItem.GetDataKeyValue("TotalInvoiceAmount") != null)
                            totalInvoiceAmount = Convert.ToDecimal(editableItem.GetDataKeyValue("TotalInvoiceAmount").ToString());

                        decimal? clientTaxHighRatio = null;
                        if (editableItem.GetDataKeyValue("ClientTaxHighRatio") != null)
                            clientTaxHighRatio = Convert.ToDecimal(editableItem.GetDataKeyValue("ClientTaxHighRatio").ToString());

                        decimal? clientTaxLowRatio = null;
                        if (editableItem.GetDataKeyValue("ClientTaxLowRatio") != null)
                            clientTaxLowRatio = Convert.ToDecimal(editableItem.GetDataKeyValue("ClientTaxLowRatio").ToString());

                        decimal? clientTaxDeductionRatio = null;
                        if (editableItem.GetDataKeyValue("ClientTaxDeductionRatio") != null)
                            clientTaxDeductionRatio = Convert.ToDecimal(editableItem.GetDataKeyValue("ClientTaxDeductionRatio").ToString());

                        decimal? highRatioAmount = null;
                        if (editableItem.GetDataKeyValue("HighRatioAmount") != null)
                            highRatioAmount = Convert.ToDecimal(editableItem.GetDataKeyValue("HighRatioAmount").ToString());

                        decimal? lowRatioAmount = null;
                        if (editableItem.GetDataKeyValue("LowRatioAmount") != null)
                            lowRatioAmount = Convert.ToDecimal(editableItem.GetDataKeyValue("LowRatioAmount").ToString());

                        decimal? deductionRatioAmount = null;
                        if (editableItem.GetDataKeyValue("DeductionRatioAmount") != null)
                            deductionRatioAmount = Convert.ToDecimal(editableItem.GetDataKeyValue("DeductionRatioAmount").ToString());

                        decimal payAmount = 0;
                        if (editableItem.GetDataKeyValue("PayAmount") != null)
                            payAmount = Convert.ToDecimal(editableItem.GetDataKeyValue("PayAmount").ToString());

                        var clientInvoiceSettlementDetail = clientInvoiceSettlement.ClientInvoiceSettlementDetail
                            .FirstOrDefault(x => x.IsDeleted == false && x.ClientInvoiceID == clientInvoiceID);

                        if (clientInvoiceSettlementDetail == null)
                        {
                            clientInvoiceSettlementDetail = new ClientInvoiceSettlementDetail();
                            clientInvoiceSettlement.ClientInvoiceSettlementDetail.Add(clientInvoiceSettlementDetail);
                        }

                        clientInvoiceSettlementDetail.ClientInvoiceID = clientInvoiceID;
                        clientInvoiceSettlementDetail.ClientCompanyID = clientCompanyID;
                        clientInvoiceSettlementDetail.InvoiceDate = invoiceDate;
                        clientInvoiceSettlementDetail.InvoiceNumber = invoiceNumber;
                        clientInvoiceSettlementDetail.TotalInvoiceAmount = totalInvoiceAmount;
                        clientInvoiceSettlementDetail.ClientTaxHighRatio = clientTaxHighRatio;
                        clientInvoiceSettlementDetail.ClientTaxLowRatio = clientTaxLowRatio;
                        clientInvoiceSettlementDetail.ClientTaxDeductionRatio = clientTaxDeductionRatio;
                        clientInvoiceSettlementDetail.HighRatioAmount = highRatioAmount;
                        clientInvoiceSettlementDetail.LowRatioAmount = lowRatioAmount;
                        clientInvoiceSettlementDetail.DeductionRatioAmount = deductionRatioAmount;
                        clientInvoiceSettlementDetail.PayAmount = payAmount;

                        clientInvoiceIDs.Add(clientInvoiceID);
                    }

                    var clientInvoices = ciRepository.GetList(x => clientInvoiceIDs.Contains(x.ID));

                    foreach (var clientInvoice in clientInvoices)
                    {
                        clientInvoice.IsSettled = true;
                        clientInvoice.SettledDate = DateTime.Now;
                    }

                    foreach (var item in clientInvoiceSettlement.ClientInvoiceSettlementDetail
                        .Where(x => x.IsDeleted == false && !clientInvoiceIDs.Contains(x.ClientInvoiceID)))
                    {
                        item.IsDeleted = true;

                        var clientInvoice = ciRepository.GetByID(item.ClientInvoiceID);

                        clientInvoice.IsSettled = false;
                        clientInvoice.SettledDate = null;
                    }

                    clientInvoiceSettlement.TotalInvoiceAmount = clientInvoiceSettlement.ClientInvoiceSettlementDetail
                        .Where(x => x.IsDeleted == false)
                        .Sum(x => x.TotalInvoiceAmount);
                    clientInvoiceSettlement.TotalPayAmount = clientInvoiceSettlement.ClientInvoiceSettlementDetail
                        .Where(x => x.IsDeleted == false)
                        .Sum(x => x.PayAmount);

                    unitOfWork.SaveChanges();

                    if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = CurrentWorkFlowID;
                        appNote.WorkflowStepID = (int)EWorkflowStep.NewCISettlement;
                        appNote.NoteTypeID = (int)EAppNoteType.Comment;
                        appNote.ApplicationID = clientInvoiceSettlement.ID;
                        appNote.Note = txtComment.Text.Trim();

                        PageAppNoteRepository.Add(appNote);

                        PageAppNoteRepository.Save();
                    }

                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                }
            }
        }

        protected void btnAudit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAuditComment.Text.Trim()))
                cvAuditComment.IsValid = false;

            if (!IsValid) return;

            if (this.CurrentEntity != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IClientInvoiceSettlementRepository cisRepository = new ClientInvoiceSettlementRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    cisRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = cisRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = CurrentWorkFlowID;
                        appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                        appNote.ApplicationID = currentEntity.ID;
                        appNote.Note = txtAuditComment.Text.Trim();
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditCISettlementByTreasurers;
                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByTreasurers;

                                break;

                            case (int)EWorkflowStatus.ApprovedByTreasurers:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditCISettlementByDeptManagers;
                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByDeptManagers;

                                break;
                        }

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

                    IClientInvoiceSettlementRepository cisRepository = new ClientInvoiceSettlementRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    cisRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = cisRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = CurrentWorkFlowID;
                        appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                        appNote.ApplicationID = currentEntity.ID;
                        appNote.Note = txtAuditComment.Text.Trim();
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditCISettlementByTreasurers;
                                break;

                            case (int)EWorkflowStatus.ApprovedByTreasurers:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditCISettlementByDeptManagers;
                                break;
                        }

                        currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                    }
                }
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            if (CurrentEntity != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IClientInvoiceSettlementRepository cisRepository = new ClientInvoiceSettlementRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();

                    cisRepository.SetDbModel(db);
                    appPaymentRepository.SetDbModel(db);

                    var currentEntity = cisRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {

                        var appPayments = appPaymentRepository.GetList(x => x.WorkflowID == CurrentWorkFlowID
                            && x.ApplicationID == CurrentEntity.ID).ToList();

                        var totalNeedSettledAmount = CurrentEntity.ClientInvoiceSettlementDetail
                            .Where(x => x.IsDeleted == false).Sum(x => x.PayAmount);

                        var totalPayAmount = appPayments.Sum(x => x.Amount.HasValue ? x.Amount.Value : 0M);

                        if (totalPayAmount != totalNeedSettledAmount)
                        {
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                            this.Master.BaseNotification.AutoCloseDelay = 1000;
                            this.Master.BaseNotification.Show("支付总额不等于应结算总额，不能确认支付");
                        }
                        else
                        {
                            foreach (var item in appPayments)
                            {
                                item.PaymentStatusID = (int)EPaymentStatus.Paid;
                            }

                            currentEntity.WorkflowStatusID = (int)EWorkflowStatus.Paid;
                            currentEntity.PaidDate = DateTime.Now;
                            currentEntity.PaidBy = CurrentUser.UserID;

                            unitOfWork.SaveChanges();

                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                        }
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCancelComment.Text.Trim()))
            {
                cvCancelComment.IsValid = false;
            }

            if (!IsValid) return;

            if (this.CurrentEntity != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IClientInvoiceSettlementRepository cisRepository = new ClientInvoiceSettlementRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();

                    cisRepository.SetDbModel(db);
                    appPaymentRepository.SetDbModel(db);

                    var currentEntity = cisRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        foreach (var item in currentEntity.ClientInvoiceSettlementDetail.Where(x => x.IsDeleted == false))
                        {
                            item.ClientInvoice.IsSettled = null;
                            item.ClientInvoice.SettledDate = null;
                        }

                        currentEntity.IsCanceled = true;
                        currentEntity.CanceledReason = txtCancelComment.Text.Trim();
                        currentEntity.CanceledDate = DateTime.Now;
                        currentEntity.CanceledBy = CurrentUser.UserID;
                    }

                    var appPayments = appPaymentRepository.GetList(x => x.WorkflowID == CurrentWorkFlowID
                            && x.ApplicationID == CurrentEntity.ID).ToList();

                    foreach (var item in appPayments)
                    {
                        var appPayment = new ApplicationPayment()
                        {
                            ApplicationID = item.ApplicationID,
                            WorkflowID = item.WorkflowID,
                            PaymentStatusID = item.PaymentStatusID,
                            PaymentTypeID = item.PaymentTypeID,
                            PayDate = item.PayDate,
                            FromBankAccountID = item.FromBankAccountID,
                            FromAccount = item.FromAccount,
                            Amount = -item.Amount,
                            Fee = -item.Fee
                        };

                        appPaymentRepository.Add(appPayment);
                    }

                    unitOfWork.SaveChanges();

                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);

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

        #endregion

    }
}