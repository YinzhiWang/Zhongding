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
    public partial class SupplierInvoiceSettlementMaintenance : WorkflowBasePage
    {
        #region Members

        private ISupplierInvoiceSettlementRepository _PageSupplierInvoiceSettlementRepository;
        private ISupplierInvoiceSettlementRepository PageSupplierInvoiceSettlementRepository
        {
            get
            {
                if (_PageSupplierInvoiceSettlementRepository == null)
                    _PageSupplierInvoiceSettlementRepository = new SupplierInvoiceSettlementRepository();

                return _PageSupplierInvoiceSettlementRepository;
            }
        }

        private ISupplierInvoiceSettlementDetailRepository _PageSupplierInvoiceSDRepository;
        private ISupplierInvoiceSettlementDetailRepository PageSupplierInvoiceSDRepository
        {
            get
            {
                if (_PageSupplierInvoiceSDRepository == null)
                    _PageSupplierInvoiceSDRepository = new SupplierInvoiceSettlementDetailRepository();

                return _PageSupplierInvoiceSDRepository;
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

        private IList<int> _CanAddUserIDs;
        private IList<int> CanAddUserIDs
        {
            get
            {
                if (_CanAddUserIDs == null)
                    _CanAddUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewSupplierInvoiceSettlement);

                return _CanAddUserIDs;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditSupplierInvoiceSettlement);

                return _CanEditUserIDs;
            }
        }

        private SupplierInvoiceSettlement _CurrentEntity;
        private SupplierInvoiceSettlement CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageSupplierInvoiceSettlementRepository.GetByID(this.CurrentEntityID);

                return _CurrentEntity;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.SupplierInvoiceSettlement;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.SupplierInvoiceSettlementManage;

            if (!IsPostBack)
            {
                BindBankAccount();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

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

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                if (this.CurrentEntity.IsCanceled == true)
                {
                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_REDIRECT);

                    return;
                }

                rdpSettlementDate.SelectedDate = CurrentEntity.SettlementDate;
                lblCompanyName.Text = CurrentEntity.Company.CompanyName;
                lblTaxRatio.Text = CurrentEntity.TaxRatio.ToString("P2");

                rcbxToAccount.SelectedValue = CurrentEntity.ToBankAccountID.ToString();

                lblTotalPayAmount.Text = CurrentEntity.TotalPayAmount.ToString("C2");
                lblCapitalAmount.Text = CurrentEntity.TotalPayAmount.ToString().ConvertToChineseMoney();

                tblSearch.Visible = false;
                btnSubmit.Visible = false;
                rdpSettlementDate.Enabled = false;
                rcbxToAccount.Enabled = false;

                if (CanAddUserIDs.Contains(CurrentUser.UserID)
                    || CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    divCancel.Visible = true;
                    btnCancel.Visible = true;
                }
                else
                {
                    divCancel.Visible = false;
                    btnCancel.Visible = false;
                }
            }
            else
            {
                InitDefaultData();

                if (!this.CanAddUserIDs.Contains(CurrentUser.UserID))
                {
                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show("您没有权限新增供应商发票结算");
                }
            }
        }

        private void InitDefaultData()
        {
            var company = PageCompanyRepository.GetByID(CurrentUser.CompanyID);
            if (company != null)
            {
                lblCompanyName.Text = company.CompanyName;
                lblTaxRatio.Text = (company.ProviderTexRatio ?? 0).ToString("P2");
            }

            rdpSettlementDate.SelectedDate = DateTime.Now;

            divCancel.Visible = false;
            btnCancel.Visible = false;
        }

        private void BindSupplierInvoices(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchSupplierInvoiceSettlementDetail
            {
                SupplierInvoiceSettlementID = this.CurrentEntityID.HasValue
                ? CurrentEntityID.Value : GlobalConst.INVALID_INT,

                CompanyID = CurrentUser.CompanyID,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate
            };

            IList<UISupplierInvoiceSettlementDetail> uiEntities = new List<UISupplierInvoiceSettlementDetail>();

            if (this.CurrentEntity == null)
            {
                uiEntities = PageSupplierInvoiceSDRepository.GetNeedSettleUIList(uiSearchObj);
            }
            else
            {
                int totalRecords;

                uiEntities = PageSupplierInvoiceSDRepository.GetUIList(uiSearchObj, rgSupplierInvoices.CurrentPageIndex, rgSupplierInvoices.PageSize, out totalRecords);

                rgSupplierInvoices.VirtualItemCount = totalRecords;
            }

            rgSupplierInvoices.DataSource = uiEntities;

            if (isNeedRebind)
                rgSupplierInvoices.Rebind();
        }

        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindSupplierInvoices(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpBeginDate.Clear();
            rdpEndDate.Clear();

            BindSupplierInvoices(true);
        }

        protected void rgSupplierInvoices_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindSupplierInvoices(false);
        }

        protected void rgSupplierInvoices_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (this.CurrentEntity == null)
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_CLIENT_SELECT).Visible = true;
            else
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_CLIENT_SELECT).Visible = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            var selectedItems = rgSupplierInvoices.SelectedItems;

            if (selectedItems.Count == 0)
            {
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show("请选择要结算的供应商发票");

                return;
            }

            if (CurrentEntity == null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    ISupplierInvoiceRepository siRepository = new SupplierInvoiceRepository();
                    ISupplierInvoiceSettlementRepository sisRepository = new SupplierInvoiceSettlementRepository();
                    ISupplierInvoiceSettlementDetailRepository sisdRepository = new SupplierInvoiceSettlementDetailRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();

                    siRepository.SetDbModel(db);
                    sisRepository.SetDbModel(db);
                    sisdRepository.SetDbModel(db);
                    appPaymentRepository.SetDbModel(db);

                    var company = PageCompanyRepository.GetByID(CurrentUser.CompanyID);

                    var supplierInvoiceSettlement = new SupplierInvoiceSettlement()
                    {
                        CompanyID = company.ID,
                        TaxRatio = company.ProviderTexRatio ?? 0,
                        SettlementDate = rdpSettlementDate.SelectedDate ?? DateTime.Now,
                        ToBankAccountID = Convert.ToInt32(rcbxToAccount.SelectedValue),
                        ToAccount = rcbxToAccount.SelectedItem.Text
                    };

                    List<int> supplierInvoiceIDs = new List<int>();

                    foreach (var item in selectedItems)
                    {
                        var editableItem = ((GridEditableItem)item);

                        int supplierInvoiceID = Convert.ToInt32(editableItem.GetDataKeyValue("SupplierInvoiceID").ToString());

                        supplierInvoiceIDs.Add(supplierInvoiceID);
                    }

                    var supplierInvoices = siRepository.GetList(x => supplierInvoiceIDs.Contains(x.ID));

                    foreach (var supplierInvoice in supplierInvoices)
                    {
                        supplierInvoice.IsSettled = true;
                        supplierInvoice.SettledDate = DateTime.Now;

                        var supplierInvoiceSettlementDetail = new SupplierInvoiceSettlementDetail()
                        {
                            SupplierInvoiceID = supplierInvoice.ID,
                            SupplierID = supplierInvoice.SupplierID,
                            InvoiceDate = supplierInvoice.InvoiceDate,
                            InvoiceNumber = supplierInvoice.InvoiceNumber,
                            InvoiceAmount = supplierInvoice.Amount,
                            PayAmount = supplierInvoice.Amount * supplierInvoiceSettlement.TaxRatio
                        };

                        var appPayment = new ApplicationPayment()
                        {
                            WorkflowID = CurrentWorkFlowID,
                            ApplicationID = GlobalConst.INVALID_INT,
                            ToBankAccountID = supplierInvoiceSettlement.ToBankAccountID,
                            ToAccount = supplierInvoiceSettlement.ToAccount,
                            Amount = supplierInvoiceSettlementDetail.PayAmount,
                            PaymentTypeID = (int)EPaymentType.Income,
                            PaymentStatusID = (int)EPaymentStatus.Paid,
                            PayDate = DateTime.Now,
                        };

                        supplierInvoiceSettlementDetail.ApplicationPayment = appPayment;

                        supplierInvoiceSettlement.SupplierInvoiceSettlementDetail.Add(supplierInvoiceSettlementDetail);
                    }

                    supplierInvoiceSettlement.TotalInvoiceAmount = supplierInvoiceSettlement.SupplierInvoiceSettlementDetail
                        .Where(x => x.IsDeleted == false).Sum(x => x.InvoiceAmount);

                    supplierInvoiceSettlement.TotalPayAmount = supplierInvoiceSettlement.SupplierInvoiceSettlementDetail
                        .Where(x => x.IsDeleted == false).Sum(x => x.PayAmount);

                    sisRepository.Add(supplierInvoiceSettlement);

                    unitOfWork.SaveChanges();

                    //更新apppayment application id
                    foreach (var item in supplierInvoiceSettlement.SupplierInvoiceSettlementDetail)
                    {
                        item.ApplicationPayment.ApplicationID = supplierInvoiceSettlement.ID;
                    }

                    unitOfWork.SaveChanges();

                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);

                }

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
                foreach (var item in CurrentEntity.SupplierInvoiceSettlementDetail.Where(x => x.IsDeleted == false))
                {
                    var appPayment = new ApplicationPayment()
                    {
                        WorkflowID = CurrentWorkFlowID,
                        ApplicationID = CurrentEntity.ID,
                        ToBankAccountID = CurrentEntity.ToBankAccountID,
                        ToAccount = CurrentEntity.ToAccount,
                        Amount = -item.PayAmount,
                        PaymentTypeID = (int)EPaymentType.Income,
                        PaymentStatusID = (int)EPaymentStatus.Paid,
                        PayDate = DateTime.Now,
                    };

                    item.ApplicationPayment1 = appPayment;

                    item.IsDeleted = true;
                    item.SupplierInvoice.IsSettled = null;
                    item.SupplierInvoice.SettledDate = null;
                }

                CurrentEntity.IsDeleted = true;

                CurrentEntity.IsCanceled = true;
                CurrentEntity.CanceledReason = txtCancelComment.Text.Trim();
                CurrentEntity.CanceledDate = DateTime.Now;
                CurrentEntity.CanceledBy = CurrentUser.UserID;

                PageSupplierInvoiceSettlementRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
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