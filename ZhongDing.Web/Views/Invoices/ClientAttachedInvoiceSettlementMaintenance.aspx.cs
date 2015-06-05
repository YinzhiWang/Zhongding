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
    public partial class ClientAttachedInvoiceSettlementMaintenance : WorkflowBasePage
    {
        #region Members
        private IClientAttachedInvoiceSettlementRepository _PageClientAttachedInvoiceSettlementRepository;
        private IClientAttachedInvoiceSettlementRepository PageClientAttachedInvoiceSettlementRepository
        {
            get
            {
                if (_PageClientAttachedInvoiceSettlementRepository == null)
                    _PageClientAttachedInvoiceSettlementRepository = new ClientAttachedInvoiceSettlementRepository();

                return _PageClientAttachedInvoiceSettlementRepository;
            }
        }

        private IClientAttachedInvoiceSettlementDetailRepository _PageClientAttachedInvoiceSDRepository;
        private IClientAttachedInvoiceSettlementDetailRepository PageClientAttachedInvoiceSDRepository
        {
            get
            {
                if (_PageClientAttachedInvoiceSDRepository == null)
                    _PageClientAttachedInvoiceSDRepository = new ClientAttachedInvoiceSettlementDetailRepository();

                return _PageClientAttachedInvoiceSDRepository;
            }
        }

        private IClientUserRepository _PageClientUserRepository;
        private IClientUserRepository PageClientUserRepository
        {
            get
            {
                if (_PageClientUserRepository == null)
                    _PageClientUserRepository = new ClientUserRepository();

                return _PageClientUserRepository;
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

        private ClientAttachedInvoiceSettlement _CurrentEntity;
        private ClientAttachedInvoiceSettlement CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageClientAttachedInvoiceSettlementRepository.GetByID(this.CurrentEntityID);

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

        private ICostTypeRepository _PageCostTypeRepository;
        private ICostTypeRepository PageCostTypeRepository
        {
            get
            {
                if (_PageCostTypeRepository == null)
                    _PageCostTypeRepository = new CostTypeRepository();

                return _PageCostTypeRepository;
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
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewCAISettlement);
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
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditCAISettlement);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.ClientAttachedInvoiceSettlement;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientAttachedInvoiceSettlementManage;

            if (!IsPostBack)
            {
                BindClientUsers();

                BindBankAccount();

                BindOtherCostTypes();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindClientUsers()
        {
            var clientUsers = PageClientUserRepository.GetDropdownItems(new UISearchDropdownItem
            {
                Extension = new UISearchExtension { OnlyIncludeValidClientUser = true }
            });

            rcbxClientUser.DataSource = clientUsers;
            rcbxClientUser.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientUser.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientUser.DataBind();

            rcbxClientUser.Items.Insert(0, new RadComboBoxItem("", ""));

        }

        private void BindClientCompanies()
        {
            rcbxClientCompany.ClearSelection();
            rcbxClientCompany.Items.Clear();

            var uiSearchObj = new UISearchDropdownItem();

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
            {
                int clientUserID;
                if (int.TryParse(rcbxClientUser.SelectedValue, out clientUserID))
                    uiSearchObj.Extension = new UISearchExtension { ClientUserID = clientUserID };
            }

            var clientCompanies = PageClientCompanyRepository.GetDropdownItems(uiSearchObj);
            rcbxClientCompany.DataSource = clientCompanies;
            rcbxClientCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientCompany.DataBind();

            rcbxClientCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindBankAccount()
        {
            var uiSearchObj = new UISearchDropdownItem
            {
                Extension = new UISearchExtension
                {
                    OwnerTypeID = (int)EOwnerType.Company,
                    CompanyID = CurrentUser.CompanyID
                }
            };

            var bankAccounts = PageBankAccountRepository.GetDropdownItems(uiSearchObj);
            rcbxToAccount.DataSource = bankAccounts;
            rcbxToAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxToAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxToAccount.DataBind();
        }

        private void BindOtherCostTypes()
        {
            var costTypes = PageCostTypeRepository.GetDropdownItems();

            rcbxOtherCostType.DataSource = costTypes;
            rcbxOtherCostType.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxOtherCostType.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxOtherCostType.DataBind();

            rcbxOtherCostType.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                rcbxClientUser.SelectedValue = CurrentEntity.ClientUserID.ToString();
                BindClientCompanies();
                rcbxClientCompany.SelectedValue = CurrentEntity.ClientCompanyID.ToString();

                txtReceiveAmount.DbValue = CurrentEntity.ReceiveAmount;
                rcbxToAccount.SelectedValue = CurrentEntity.ReceiveBankAccountID.ToString();

                rcbxOtherCostType.SelectedValue = CurrentEntity.OtherCostTypeID.HasValue
                    ? CurrentEntity.OtherCostTypeID.ToString() : string.Empty;
                txtOtherCostAmount.DbValue = CurrentEntity.OtherCostAmount;

                rdpConfirmDate.SelectedDate = CurrentEntity.ConfirmDate;
                rdpSettlementDate.SelectedDate = CurrentEntity.SettlementDate;

                if (CurrentEntity.TotalSettlementAmount.HasValue)
                {
                    lblTotalSettlementAmount.Text = CurrentEntity.TotalSettlementAmount.ToString("C2");
                    lblCapitalTSAmount.Text = CurrentEntity.TotalSettlementAmount.ToString().ConvertToChineseMoney();
                }

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
                .GetList(x => x.WorkflowID == CurrentWorkFlowID && x.PaymentTypeID == (int)EPaymentType.Expend
                    && x.ApplicationID == CurrentEntity.ID).Select(x => x.Amount).ToList();

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
            rcbxClientUser.Enabled = false;
            rcbxClientCompany.Enabled = false;
            txtReceiveAmount.Enabled = false;
            rcbxToAccount.Enabled = false;
            rcbxOtherCostType.Enabled = false;
            txtOtherCostAmount.Enabled = false;
            rdpConfirmDate.Enabled = false;
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
            //divComment.Visible = false;
            divCancel.Visible = false;
            //divComments.Visible = false;
            divOtherSections.Visible = false;

            rdpConfirmDate.SelectedDate = DateTime.Now;
            rdpSettlementDate.SelectedDate = DateTime.Now;
        }

        private void BindClientInvoices(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchClientAttachedInvoiceSettlementDetail
            {
                ClientAttachedInvoiceSettlementID = this.CurrentEntityID,
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

            var uiEntities = PageClientAttachedInvoiceSDRepository.GetUIList(uiSearchObj);

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
                UIClientAttachedInvoiceSettlementDetail uiEntity = (UIClientAttachedInvoiceSettlementDetail)gridDataItem.DataItem;

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
                                && !uiEntity.InvoiceSettlementRatio.HasValue)
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

                var txtSettlementQty = (RadNumericTextBox)gridDataItem.FindControl("txtSettlementQty");
                if (txtSettlementQty != null)
                {
                    txtSettlementQty.DbValue = uiEntity.SettlementQty;

                    if (this.CurrentEntity == null || (this.CurrentEntity != null
                        && (this.CurrentEntity.CreatedBy == CurrentUser.UserID || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                        && (this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.TemporarySave
                            || this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ReturnBasicInfo)))
                    {
                        txtSettlementQty.Enabled = true;
                    }
                    else
                    {
                        txtSettlementQty.Enabled = false;
                    }
                }

                var txtSettlementAmount = (RadNumericTextBox)gridDataItem.FindControl("txtSettlementAmount");
                if (txtSettlementAmount != null)
                {
                    txtSettlementAmount.DbValue = uiEntity.SettlementAmount;

                    if (this.CurrentEntity == null || (this.CurrentEntity != null
                        && (this.CurrentEntity.CreatedBy == CurrentUser.UserID || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                        && (this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.TemporarySave
                            || this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ReturnBasicInfo)))
                    {
                        txtSettlementAmount.Enabled = true;
                    }
                    else
                    {
                        txtSettlementAmount.Enabled = false;
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
                PaymentTypeID = (int)EPaymentType.Expend,
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
                if (rdpPayDate != null)
                    rdpPayDate.MinDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
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

        protected void rcbxClientUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindClientCompanies();
        }

        protected void rcbxClientCompany_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindClientInvoices(true);
        }

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
            if (!string.IsNullOrEmpty(rcbxOtherCostType.SelectedValue)
                && !txtOtherCostAmount.Value.HasValue)
            {
                cvOtherCostAmount.IsValid = false;
            }

            if (txtOtherCostAmount.Value.HasValue
                && string.IsNullOrEmpty(rcbxOtherCostType.SelectedValue))
            {
                cvOtherCostType.IsValid = false;
            }

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

                IClientInvoiceDetailRepository cidRepository = new ClientInvoiceDetailRepository();
                IClientAttachedInvoiceSettlementRepository caisRepository = new ClientAttachedInvoiceSettlementRepository();
                IClientAttachedInvoiceSettlementDetailRepository caisdRepository = new ClientAttachedInvoiceSettlementDetailRepository();
                IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();

                cidRepository.SetDbModel(db);
                caisRepository.SetDbModel(db);
                caisdRepository.SetDbModel(db);
                appPaymentRepository.SetDbModel(db);

                ClientAttachedInvoiceSettlement clientAttachedInvoiceSettlement = caisRepository.GetByID(this.CurrentEntityID);

                if (clientAttachedInvoiceSettlement == null)
                {
                    clientAttachedInvoiceSettlement = new ClientAttachedInvoiceSettlement()
                    {
                        CompanyID = CurrentUser.CompanyID,
                        WorkflowStatusID = (int)EWorkflowStatus.TemporarySave,
                    };

                    caisRepository.Add(clientAttachedInvoiceSettlement);
                }

                clientAttachedInvoiceSettlement.ClientUserID = Convert.ToInt32(rcbxClientUser.SelectedValue);
                clientAttachedInvoiceSettlement.ClientCompanyID = Convert.ToInt32(rcbxClientCompany.SelectedValue);
                clientAttachedInvoiceSettlement.ReceiveBankAccountID = Convert.ToInt32(rcbxToAccount.SelectedValue);
                clientAttachedInvoiceSettlement.ReceiveAccount = rcbxToAccount.SelectedItem.Text;
                clientAttachedInvoiceSettlement.ConfirmDate = rdpConfirmDate.SelectedDate ?? DateTime.Now;
                clientAttachedInvoiceSettlement.SettlementDate = rdpSettlementDate.SelectedDate ?? DateTime.Now;

                if (!string.IsNullOrEmpty(rcbxOtherCostType.SelectedValue))
                    clientAttachedInvoiceSettlement.OtherCostTypeID = Convert.ToInt32(rcbxOtherCostType.SelectedValue);
                clientAttachedInvoiceSettlement.OtherCostAmount = (decimal?)txtOtherCostAmount.Value;

                //总的结算发票税额
                decimal totalInvoiceRatioAmount = 0M;

                List<int> clientInvoiceDetailIDs = new List<int>();

                foreach (var item in selectedItems)
                {
                    var editableItem = ((GridEditableItem)item);

                    int clientInvoiceDetailID = Convert.ToInt32(editableItem.GetDataKeyValue("ClientInvoiceDetailID").ToString());
                    int stockOutDetailID = Convert.ToInt32(editableItem.GetDataKeyValue("StockOutDetailID").ToString());
                    int invoiceQty = Convert.ToInt32(editableItem.GetDataKeyValue("InvoiceQty").ToString());
                    decimal salesPrice = Convert.ToDecimal(editableItem.GetDataKeyValue("SalesPrice").ToString());
                    decimal invoicePrice = Convert.ToDecimal(editableItem.GetDataKeyValue("InvoicePrice").ToString());

                    decimal invoiceSettlementRatio = 0;
                    if (editableItem.GetDataKeyValue("InvoiceSettlementRatio") != null)
                        invoiceSettlementRatio = Convert.ToDecimal(editableItem.GetDataKeyValue("InvoiceSettlementRatio").ToString());

                    var txtSettlementQty = (RadNumericTextBox)editableItem.FindControl("txtSettlementQty");
                    var settlementQty = (int?)txtSettlementQty.Value;

                    var clientAttachedInvoiceSettlementDetail = clientAttachedInvoiceSettlement.ClientAttachedInvoiceSettlementDetail
                        .FirstOrDefault(x => x.IsDeleted == false && x.ClientInvoiceDetailID == clientInvoiceDetailID
                            && x.StockOutDetailID == stockOutDetailID);

                    if (clientAttachedInvoiceSettlementDetail == null)
                    {
                        clientAttachedInvoiceSettlementDetail = new ClientAttachedInvoiceSettlementDetail();
                        clientAttachedInvoiceSettlement.ClientAttachedInvoiceSettlementDetail.Add(clientAttachedInvoiceSettlementDetail);
                    }

                    var settlementAmount = settlementQty * invoicePrice;

                    clientAttachedInvoiceSettlementDetail.ClientInvoiceDetailID = clientInvoiceDetailID;
                    clientAttachedInvoiceSettlementDetail.StockOutDetailID = stockOutDetailID;
                    clientAttachedInvoiceSettlementDetail.InvoiceQty = invoiceQty;
                    clientAttachedInvoiceSettlementDetail.SettlementQty = settlementQty;
                    clientAttachedInvoiceSettlementDetail.SettlementAmount = settlementAmount;
                    clientAttachedInvoiceSettlementDetail.SalesAmount = settlementQty * salesPrice;

                    totalInvoiceRatioAmount += (settlementAmount ?? 0) * invoiceSettlementRatio;

                    clientInvoiceDetailIDs.Add(clientInvoiceDetailID);
                }

                foreach (var item in clientAttachedInvoiceSettlement.ClientAttachedInvoiceSettlementDetail
                    .Where(x => x.IsDeleted == false && !clientInvoiceDetailIDs.Contains(x.ClientInvoiceDetailID)))
                {
                    item.IsDeleted = true;
                }

                clientAttachedInvoiceSettlement.TotalSettlementAmount = clientAttachedInvoiceSettlement.ClientAttachedInvoiceSettlementDetail
                    .Where(x => x.IsDeleted == false)
                    .Sum(x => x.SettlementAmount);

                var totalSalesAmount = clientAttachedInvoiceSettlement.ClientAttachedInvoiceSettlementDetail
                    .Where(x => x.IsDeleted == false)
                    .Sum(x => x.SalesAmount ?? 0);

                clientAttachedInvoiceSettlement.ReceiveAmount = (clientAttachedInvoiceSettlement.TotalSettlementAmount ?? 0)
                    - (clientAttachedInvoiceSettlement.OtherCostAmount ?? 0);

                clientAttachedInvoiceSettlement.TotalRefundAmount = clientAttachedInvoiceSettlement.ReceiveAmount - totalInvoiceRatioAmount
                    + totalSalesAmount * 0.08M;

                ApplicationPayment appPayment = clientAttachedInvoiceSettlement.ApplicationPayment;

                if (appPayment == null)
                {
                    appPayment = new ApplicationPayment()
                    {
                        WorkflowID = CurrentWorkFlowID,
                        PaymentTypeID = (int)EPaymentType.Income,
                        PaymentStatusID = (int)EPaymentStatus.Paid,
                    };
                }

                appPayment.ToBankAccountID = clientAttachedInvoiceSettlement.ReceiveBankAccountID;
                appPayment.ToAccount = clientAttachedInvoiceSettlement.ReceiveAccount;
                appPayment.Amount = clientAttachedInvoiceSettlement.ReceiveAmount;
                appPayment.PayDate = DateTime.Now;

                clientAttachedInvoiceSettlement.ApplicationPayment = appPayment;

                unitOfWork.SaveChanges();

                hdnCurrentEntityID.Value = clientAttachedInvoiceSettlement.ID.ToString();

                if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
                {
                    var appNote = new ApplicationNote();
                    appNote.WorkflowID = CurrentWorkFlowID;
                    appNote.WorkflowStepID = (int)EWorkflowStep.NewCAISettlement;
                    appNote.NoteTypeID = (int)EAppNoteType.Comment;
                    appNote.ApplicationID = clientAttachedInvoiceSettlement.ID;
                    appNote.Note = txtComment.Text.Trim();

                    PageAppNoteRepository.Add(appNote);

                    PageAppNoteRepository.Save();
                }

                this.Master.BaseNotification.OnClientHidden = "refreshMaintenancePage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REFRESH);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(rcbxOtherCostType.SelectedValue)
                && !txtOtherCostAmount.Value.HasValue)
            {
                cvOtherCostAmount.IsValid = false;
            }

            if (txtOtherCostAmount.Value.HasValue
                && string.IsNullOrEmpty(rcbxOtherCostType.SelectedValue))
            {
                cvOtherCostType.IsValid = false;
            }

            if (!IsValid) return;

            var selectedItems = rgClientInvoices.SelectedItems;

            if (selectedItems.Count == 0)
            {
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.AutoCloseDelay = 1000;
                this.Master.BaseNotification.Show("请选择要结算的发票");

                return;
            }

            if (this.CurrentEntity != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IClientInvoiceDetailRepository cidRepository = new ClientInvoiceDetailRepository();
                    IClientAttachedInvoiceSettlementRepository caisRepository = new ClientAttachedInvoiceSettlementRepository();
                    IClientAttachedInvoiceSettlementDetailRepository caisdRepository = new ClientAttachedInvoiceSettlementDetailRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();

                    cidRepository.SetDbModel(db);
                    caisRepository.SetDbModel(db);
                    caisdRepository.SetDbModel(db);
                    appPaymentRepository.SetDbModel(db);

                    ClientAttachedInvoiceSettlement clientAttachedInvoiceSettlement = caisRepository.GetByID(this.CurrentEntityID);

                    if (clientAttachedInvoiceSettlement != null)
                    {
                        clientAttachedInvoiceSettlement.WorkflowStatusID = (int)EWorkflowStatus.Submit;
                        clientAttachedInvoiceSettlement.ClientUserID = Convert.ToInt32(rcbxClientUser.SelectedValue);
                        clientAttachedInvoiceSettlement.ClientCompanyID = Convert.ToInt32(rcbxClientCompany.SelectedValue);
                        clientAttachedInvoiceSettlement.ReceiveBankAccountID = Convert.ToInt32(rcbxToAccount.SelectedValue);
                        clientAttachedInvoiceSettlement.ReceiveAccount = rcbxToAccount.SelectedItem.Text;
                        clientAttachedInvoiceSettlement.ConfirmDate = rdpConfirmDate.SelectedDate ?? DateTime.Now;
                        clientAttachedInvoiceSettlement.SettlementDate = rdpSettlementDate.SelectedDate ?? DateTime.Now;

                        if (!string.IsNullOrEmpty(rcbxOtherCostType.SelectedValue))
                            clientAttachedInvoiceSettlement.OtherCostTypeID = Convert.ToInt32(rcbxOtherCostType.SelectedValue);
                        clientAttachedInvoiceSettlement.OtherCostAmount = (decimal?)txtOtherCostAmount.Value;

                        //总的结算发票税额
                        decimal totalInvoiceRatioAmount = 0M;

                        List<int> clientInvoiceDetailIDs = new List<int>();

                        foreach (var item in selectedItems)
                        {
                            var editableItem = ((GridEditableItem)item);

                            int clientInvoiceDetailID = Convert.ToInt32(editableItem.GetDataKeyValue("ClientInvoiceDetailID").ToString());
                            int stockOutDetailID = Convert.ToInt32(editableItem.GetDataKeyValue("StockOutDetailID").ToString());
                            int invoiceQty = Convert.ToInt32(editableItem.GetDataKeyValue("InvoiceQty").ToString());
                            decimal salesPrice = Convert.ToDecimal(editableItem.GetDataKeyValue("SalesPrice").ToString());
                            decimal invoicePrice = Convert.ToDecimal(editableItem.GetDataKeyValue("InvoicePrice").ToString());

                            decimal invoiceSettlementRatio = 0;
                            if (editableItem.GetDataKeyValue("InvoiceSettlementRatio") != null)
                                invoiceSettlementRatio = Convert.ToDecimal(editableItem.GetDataKeyValue("InvoiceSettlementRatio").ToString());

                            var txtSettlementQty = (RadNumericTextBox)editableItem.FindControl("txtSettlementQty");
                            var settlementQty = (int?)txtSettlementQty.Value;

                            var clientAttachedInvoiceSettlementDetail = clientAttachedInvoiceSettlement.ClientAttachedInvoiceSettlementDetail
                                .FirstOrDefault(x => x.IsDeleted == false && x.ClientInvoiceDetailID == clientInvoiceDetailID
                                    && x.StockOutDetailID == stockOutDetailID);

                            if (clientAttachedInvoiceSettlementDetail == null)
                            {
                                clientAttachedInvoiceSettlementDetail = new ClientAttachedInvoiceSettlementDetail();
                                clientAttachedInvoiceSettlement.ClientAttachedInvoiceSettlementDetail.Add(clientAttachedInvoiceSettlementDetail);
                            }

                            var settlementAmount = settlementQty * invoicePrice;

                            clientAttachedInvoiceSettlementDetail.ClientInvoiceDetailID = clientInvoiceDetailID;
                            clientAttachedInvoiceSettlementDetail.StockOutDetailID = stockOutDetailID;
                            clientAttachedInvoiceSettlementDetail.InvoiceQty = invoiceQty;
                            clientAttachedInvoiceSettlementDetail.SettlementQty = settlementQty;
                            clientAttachedInvoiceSettlementDetail.SettlementAmount = settlementAmount;
                            clientAttachedInvoiceSettlementDetail.SalesAmount = settlementQty * salesPrice;

                            totalInvoiceRatioAmount += (settlementAmount ?? 0) * invoiceSettlementRatio;

                            clientInvoiceDetailIDs.Add(clientInvoiceDetailID);
                        }

                        foreach (var item in clientAttachedInvoiceSettlement.ClientAttachedInvoiceSettlementDetail
                            .Where(x => x.IsDeleted == false && !clientInvoiceDetailIDs.Contains(x.ClientInvoiceDetailID)))
                        {
                            item.IsDeleted = true;
                        }

                        clientAttachedInvoiceSettlement.TotalSettlementAmount = clientAttachedInvoiceSettlement.ClientAttachedInvoiceSettlementDetail
                            .Where(x => x.IsDeleted == false)
                            .Sum(x => x.SettlementAmount);

                        var totalSalesAmount = clientAttachedInvoiceSettlement.ClientAttachedInvoiceSettlementDetail
                            .Where(x => x.IsDeleted == false)
                            .Sum(x => x.SalesAmount ?? 0);

                        clientAttachedInvoiceSettlement.ReceiveAmount = (clientAttachedInvoiceSettlement.TotalSettlementAmount ?? 0)
                            - (clientAttachedInvoiceSettlement.OtherCostAmount ?? 0);

                        clientAttachedInvoiceSettlement.TotalRefundAmount = clientAttachedInvoiceSettlement.ReceiveAmount - totalInvoiceRatioAmount
                            + totalSalesAmount * 0.08M;

                        ApplicationPayment appPayment = clientAttachedInvoiceSettlement.ApplicationPayment;

                        if (appPayment == null)
                        {
                            appPayment = new ApplicationPayment()
                            {
                                WorkflowID = CurrentWorkFlowID,
                                PaymentTypeID = (int)EPaymentType.Income,
                                PaymentStatusID = (int)EPaymentStatus.Paid,
                            };
                        }

                        appPayment.ToBankAccountID = clientAttachedInvoiceSettlement.ReceiveBankAccountID;
                        appPayment.ToAccount = clientAttachedInvoiceSettlement.ReceiveAccount;
                        appPayment.Amount = clientAttachedInvoiceSettlement.ReceiveAmount;
                        appPayment.PayDate = DateTime.Now;

                        clientAttachedInvoiceSettlement.ApplicationPayment = appPayment;

                        unitOfWork.SaveChanges();

                        hdnCurrentEntityID.Value = clientAttachedInvoiceSettlement.ID.ToString();

                        if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
                        {
                            var appNote = new ApplicationNote();
                            appNote.WorkflowID = CurrentWorkFlowID;
                            appNote.WorkflowStepID = (int)EWorkflowStep.NewCAISettlement;
                            appNote.NoteTypeID = (int)EAppNoteType.Comment;
                            appNote.ApplicationID = clientAttachedInvoiceSettlement.ID;
                            appNote.Note = txtComment.Text.Trim();

                            PageAppNoteRepository.Add(appNote);

                            PageAppNoteRepository.Save();
                        }

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
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

                    IClientAttachedInvoiceSettlementRepository caisRepository = new ClientAttachedInvoiceSettlementRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    caisRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = caisRepository.GetByID(this.CurrentEntityID);

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
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditCAISettlementByTreasurers;
                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByTreasurers;

                                break;

                            case (int)EWorkflowStatus.ApprovedByTreasurers:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditCAISettlementByDeptManagers;
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

                    IClientAttachedInvoiceSettlementRepository caisRepository = new ClientAttachedInvoiceSettlementRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    caisRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = caisRepository.GetByID(this.CurrentEntityID);

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
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditCAISettlementByTreasurers;
                                break;

                            case (int)EWorkflowStatus.ApprovedByTreasurers:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditCAISettlementByDeptManagers;
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

                    IClientAttachedInvoiceSettlementRepository caisRepository = new ClientAttachedInvoiceSettlementRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();

                    caisRepository.SetDbModel(db);
                    appPaymentRepository.SetDbModel(db);

                    var currentEntity = caisRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appPayments = appPaymentRepository.GetList(x => x.WorkflowID == CurrentWorkFlowID
                            && x.ApplicationID == CurrentEntity.ID && x.PaymentTypeID == (int)EPaymentType.Expend).ToList();

                        var totalNeedRefundAmount = CurrentEntity.TotalRefundAmount;

                        var totalPayAmount = appPayments.Sum(x => x.Amount.HasValue ? x.Amount.Value : 0M);

                        if (totalPayAmount != totalNeedRefundAmount)
                        {
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                            this.Master.BaseNotification.AutoCloseDelay = 1000;
                            this.Master.BaseNotification.Show("支付总额不等于应应返款总额，不能确认支付");
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

                    IClientAttachedInvoiceSettlementRepository caisRepository = new ClientAttachedInvoiceSettlementRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();

                    caisRepository.SetDbModel(db);
                    appPaymentRepository.SetDbModel(db);

                    var currentEntity = caisRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        foreach (var item in currentEntity.ClientAttachedInvoiceSettlementDetail.Where(x => x.IsDeleted == false))
                        {
                            item.IsDeleted = true;
                        }

                        currentEntity.IsDeleted = true;

                        currentEntity.IsCanceled = true;
                        currentEntity.CanceledReason = txtCancelComment.Text.Trim();
                        currentEntity.CanceledDate = DateTime.Now;
                        currentEntity.CanceledBy = CurrentUser.UserID;

                        ApplicationPayment canceledAppPayment = currentEntity.ApplicationPayment1;

                        if (canceledAppPayment == null)
                        {
                            canceledAppPayment = new ApplicationPayment()
                            {
                                ApplicationID = currentEntity.ID,
                                WorkflowID = this.CurrentWorkFlowID,
                            };
                        }

                        canceledAppPayment.PaymentStatusID = currentEntity.ApplicationPayment.PaymentStatusID;
                        canceledAppPayment.PaymentTypeID = currentEntity.ApplicationPayment.PaymentTypeID;
                        canceledAppPayment.PayDate = DateTime.Now;
                        canceledAppPayment.FromBankAccountID = currentEntity.ApplicationPayment.FromBankAccountID;
                        canceledAppPayment.FromAccount = currentEntity.ApplicationPayment.FromAccount;
                        canceledAppPayment.Amount = -currentEntity.ApplicationPayment.Amount;
                        canceledAppPayment.Fee = -currentEntity.ApplicationPayment.Fee;

                        currentEntity.ApplicationPayment1 = canceledAppPayment;

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
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.AutoCloseDelay = 1000;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_REDIRECT);
            }
        }



        protected override EPermission PagePermissionID()
        {
            return EPermission.InvoiceManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}