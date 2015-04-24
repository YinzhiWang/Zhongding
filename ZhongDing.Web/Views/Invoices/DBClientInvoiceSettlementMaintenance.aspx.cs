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
    public partial class DBClientInvoiceSettlementMaintenance : WorkflowBasePage
    {

        #region Members
        private IDBClientInvoiceSettlementRepository _PageDBClientInvoiceSettlementRepository;
        private IDBClientInvoiceSettlementRepository PageDBClientInvoiceSettlementRepository
        {
            get
            {
                if (_PageDBClientInvoiceSettlementRepository == null)
                    _PageDBClientInvoiceSettlementRepository = new DBClientInvoiceSettlementRepository();

                return _PageDBClientInvoiceSettlementRepository;
            }
        }

        private IDBClientInvoiceSettlementDetailRepository _PageDBClientInvoiceSDRepository;
        private IDBClientInvoiceSettlementDetailRepository PageDBClientInvoiceSDRepository
        {
            get
            {
                if (_PageDBClientInvoiceSDRepository == null)
                    _PageDBClientInvoiceSDRepository = new DBClientInvoiceSettlementDetailRepository();

                return _PageDBClientInvoiceSDRepository;
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

        private DBClientInvoiceSettlement _CurrentEntity;
        private DBClientInvoiceSettlement CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageDBClientInvoiceSettlementRepository.GetByID(this.CurrentEntityID);

                return _CurrentEntity;
            }
        }

        private IList<int> _CanAddUserIDs;
        private IList<int> CanAddUserIDs
        {
            get
            {
                if (_CanAddUserIDs == null)
                    _CanAddUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewDBClientInvoiceSettlement);

                return _CanAddUserIDs;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditDBClientInvoiceSettlement);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.DBClientInvoiceSettlement;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DBClientInvoiceSettlementManage;

            if (!IsPostBack)
            {
                BindDistributionCompanies();

                BindBankAccount();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindDistributionCompanies()
        {
            var distributionCompanies = PageDistributionCompanyRepository.GetDropdownItems();

            rcbxDistributionCompany.DataSource = distributionCompanies;
            rcbxDistributionCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDistributionCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDistributionCompany.DataBind();

            rcbxDistributionCompany.Items.Insert(0, new RadComboBoxItem("", ""));
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

                rdpReceiveDate.SelectedDate = CurrentEntity.ReceiveDate;
                rdpConfirmDate.SelectedDate = CurrentEntity.ConfirmDate;
                rcbxToAccount.SelectedValue = CurrentEntity.ReceiveBankAccountID.ToString();
                rcbxDistributionCompany.SelectedValue = CurrentEntity.DistributionCompanyID.ToString();

                lblTotalAmount.Text = lblTotalReceiveAmount.Text = CurrentEntity.TotalReceiveAmount.ToString("C2");
                lblCapitalTotalReceiveAmount.Text = CurrentEntity.TotalReceiveAmount.ToString().ConvertToChineseMoney();

                tblSearch.Visible = false;
                btnSubmit.Visible = false;
                rdpReceiveDate.Enabled = false;
                rdpConfirmDate.Enabled = false;
                rcbxToAccount.Enabled = false;
                rcbxDistributionCompany.Enabled = false;

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
                    this.Master.BaseNotification.Show("您没有权限新增大包收款");
                }
            }
        }

        private void InitDefaultData()
        {
            rdpReceiveDate.SelectedDate = DateTime.Now;
            rdpConfirmDate.SelectedDate = DateTime.Now;

            divCancel.Visible = false;
            btnCancel.Visible = false;
        }

        private void BindDBClientInvoices(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchDBClientInvoiceSettlementDetail
            {
                DBClientInvoiceSettlementID = this.CurrentEntityID.HasValue
                ? CurrentEntityID.Value : GlobalConst.INVALID_INT,

                CompanyID = CurrentUser.CompanyID,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                InvoiceNumber = txtInvoiceNumber.Text.Trim()
            };

            if (this.CurrentEntity == null)
            {
                uiSearchObj.OnlyIncludeChecked = false;

                if (!string.IsNullOrEmpty(rcbxDistributionCompany.SelectedValue))
                    uiSearchObj.DistributionCompanyID = Convert.ToInt32(rcbxDistributionCompany.SelectedValue);
                else
                    uiSearchObj.DistributionCompanyID = GlobalConst.INVALID_INT;
            }
            else
                uiSearchObj.OnlyIncludeChecked = true;

            IList<UIDBClientInvoiceSettlementDetail> uiEntities = new List<UIDBClientInvoiceSettlementDetail>();


            if (this.CurrentEntity == null)
            {
                uiEntities = PageDBClientInvoiceSDRepository.GetNeedSettleUIList(uiSearchObj);
            }
            else
            {
                int totalRecords;

                uiEntities = PageDBClientInvoiceSDRepository.GetUIList(uiSearchObj, rgDBClientInvoices.CurrentPageIndex, rgDBClientInvoices.PageSize, out totalRecords);

                rgDBClientInvoices.VirtualItemCount = totalRecords;
            }

            rgDBClientInvoices.DataSource = uiEntities;

            if (isNeedRebind)
                rgDBClientInvoices.Rebind();
        }

        #endregion

        protected void rgDBClientInvoices_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindDBClientInvoices(false);
        }

        protected void rgDBClientInvoices_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            //if (this.CurrentEntity == null)
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_CLIENT_SELECT).Visible = true;
            //else
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_CLIENT_SELECT).Visible = false;
        }

        protected void rgDBClientInvoices_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UIDBClientInvoiceSettlementDetail)gridDataItem.DataItem;

                var txtCurrentReceiveAmount = (RadNumericTextBox)gridDataItem.FindControl("txtCurrentReceiveAmount");

                if (this.CurrentEntity == null)
                {
                    txtCurrentReceiveAmount.Enabled = true;
                }
                else
                {
                    txtCurrentReceiveAmount.Enabled = false;

                    gridDataItem.SelectableMode = GridItemSelectableMode.None;

                    if (txtCurrentReceiveAmount != null && uiEntity != null)
                        txtCurrentReceiveAmount.DbValue = uiEntity.CurrentReceiveAmount;
                }
            }
        }

        protected void rcbxDistributionCompany_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindDBClientInvoices(true);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindDBClientInvoices(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpBeginDate.Clear();
            rdpEndDate.Clear();

            BindDBClientInvoices(true);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            var selectedItems = rgDBClientInvoices.SelectedItems;

            if (selectedItems.Count == 0)
            {
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.AutoCloseDelay = 1000;
                this.Master.BaseNotification.Show("请选择要收款的大包客户发票");

                return;
            }

            if (CurrentEntity == null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IDBClientInvoiceRepository dbciRepository = new DBClientInvoiceRepository();
                    IDBClientInvoiceSettlementRepository dbcisRepository = new DBClientInvoiceSettlementRepository();
                    IDBClientInvoiceSettlementDetailRepository dbcisdRepository = new DBClientInvoiceSettlementDetailRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();

                    dbciRepository.SetDbModel(db);
                    dbcisRepository.SetDbModel(db);
                    dbcisdRepository.SetDbModel(db);
                    appPaymentRepository.SetDbModel(db);

                    var company = PageCompanyRepository.GetByID(CurrentUser.CompanyID);

                    var dbClientInvoiceSettlement = new DBClientInvoiceSettlement()
                    {
                        CompanyID = company.ID,
                        ReceiveDate = rdpReceiveDate.SelectedDate ?? DateTime.Now,
                        ConfirmDate = rdpConfirmDate.SelectedDate ?? DateTime.Now,
                        ReceiveBankAccountID = Convert.ToInt32(rcbxToAccount.SelectedValue),
                        ReceiveAccount = rcbxToAccount.SelectedItem.Text,
                        DistributionCompanyID = Convert.ToInt32(rcbxDistributionCompany.SelectedValue)
                    };

                    Dictionary<int, decimal> dictDBClientInvoiceIDs = new Dictionary<int, decimal>();

                    foreach (var item in selectedItems)
                    {
                        var editableItem = ((GridEditableItem)item);

                        int dbClientInvoiceID = Convert.ToInt32(editableItem.GetDataKeyValue("DBClientInvoiceID").ToString());
                        var txtCurrentReceiveAmount = (RadNumericTextBox)editableItem.FindControl("txtCurrentReceiveAmount");

                        var currentReceiveAmount = (decimal?)txtCurrentReceiveAmount.Value;

                        if (!dictDBClientInvoiceIDs.ContainsKey(dbClientInvoiceID))
                            dictDBClientInvoiceIDs.Add(dbClientInvoiceID, currentReceiveAmount ?? 0);
                    }

                    var dbClientInvoices = dbciRepository.GetList(x => dictDBClientInvoiceIDs.Keys.Contains(x.ID));

                    foreach (var dictDBClientInvoice in dictDBClientInvoiceIDs)
                    {
                        var dbClientInvoice = dbClientInvoices.FirstOrDefault(x => x.ID == dictDBClientInvoice.Key);

                        decimal receivedAmount = dbcisdRepository.GetList(x => x.DBClientInvoiceID == dbClientInvoice.ID)
                            .Sum(x => x.ReceiveAmount);

                        if ((receivedAmount + dictDBClientInvoice.Value) == dbClientInvoice.Amount)
                        {
                            dbClientInvoice.IsSettled = true;
                            dbClientInvoice.SettledDate = DateTime.Now;
                        }

                        var dbClientInvoiceSettlementDetail = new DBClientInvoiceSettlementDetail()
                        {
                            DBClientInvoiceID = dbClientInvoice.ID,
                            InvoiceDate = dbClientInvoice.InvoiceDate,
                            InvoiceNumber = dbClientInvoice.InvoiceNumber,
                            InvoiceAmount = dbClientInvoice.Amount,
                            ReceiveAmount = dictDBClientInvoice.Value
                        };

                        var appPayment = new ApplicationPayment()
                        {
                            WorkflowID = CurrentWorkFlowID,
                            ApplicationID = GlobalConst.INVALID_INT,
                            ToBankAccountID = dbClientInvoiceSettlement.ReceiveBankAccountID,
                            ToAccount = dbClientInvoiceSettlement.ReceiveAccount,
                            Amount = dbClientInvoiceSettlementDetail.ReceiveAmount,
                            PaymentTypeID = (int)EPaymentType.Income,
                            PaymentStatusID = (int)EPaymentStatus.Paid,
                            PayDate = DateTime.Now,
                        };

                        dbClientInvoiceSettlementDetail.ApplicationPayment = appPayment;

                        dbClientInvoiceSettlement.DBClientInvoiceSettlementDetail.Add(dbClientInvoiceSettlementDetail);
                    }

                    dbClientInvoiceSettlement.TotalInvoiceAmount = dbClientInvoiceSettlement.DBClientInvoiceSettlementDetail
                        .Where(x => x.IsDeleted == false).Sum(x => x.InvoiceAmount);

                    dbClientInvoiceSettlement.TotalReceiveAmount = dbClientInvoiceSettlement.DBClientInvoiceSettlementDetail
                        .Where(x => x.IsDeleted == false).Sum(x => x.ReceiveAmount);

                    dbcisRepository.Add(dbClientInvoiceSettlement);

                    unitOfWork.SaveChanges();

                    //更新apppayment application id
                    foreach (var item in dbClientInvoiceSettlement.DBClientInvoiceSettlementDetail)
                    {
                        item.ApplicationPayment.ApplicationID = dbClientInvoiceSettlement.ID;
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
                foreach (var item in CurrentEntity.DBClientInvoiceSettlementDetail.Where(x => x.IsDeleted == false))
                {
                    var appPayment = new ApplicationPayment()
                    {
                        WorkflowID = CurrentWorkFlowID,
                        ApplicationID = CurrentEntity.ID,
                        ToBankAccountID = CurrentEntity.ReceiveBankAccountID,
                        ToAccount = CurrentEntity.ReceiveAccount,
                        Amount = -item.ReceiveAmount,
                        PaymentTypeID = (int)EPaymentType.Income,
                        PaymentStatusID = (int)EPaymentStatus.Paid,
                        PayDate = DateTime.Now,
                    };

                    item.ApplicationPayment1 = appPayment;

                    item.IsDeleted = true;

                    item.DBClientInvoice.IsSettled = null;
                    item.DBClientInvoice.SettledDate = null;
                }

                CurrentEntity.IsDeleted = true;
                CurrentEntity.IsCanceled = true;

                CurrentEntity.CanceledReason = txtCancelComment.Text.Trim();
                CurrentEntity.CanceledDate = DateTime.Now;
                CurrentEntity.CanceledBy = CurrentUser.UserID;

                PageDBClientInvoiceSettlementRepository.Save();

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