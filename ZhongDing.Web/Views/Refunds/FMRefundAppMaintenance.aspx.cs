using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Refunds
{
    public partial class FMRefundAppMaintenance : WorkflowBasePage
    {

        #region Members

        private IFactoryManagerRefundAppRepository _PageFMRefundAppRepository;
        private IFactoryManagerRefundAppRepository PageFMRefundAppRepository
        {
            get
            {
                if (_PageFMRefundAppRepository == null)
                    _PageFMRefundAppRepository = new FactoryManagerRefundAppRepository();

                return _PageFMRefundAppRepository;
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

        private IProductRepository _PageProductRepository;
        private IProductRepository PageProductRepository
        {
            get
            {
                if (_PageProductRepository == null)
                    _PageProductRepository = new ProductRepository();

                return _PageProductRepository;
            }
        }

        private IProductSpecificationRepository _PageProductSpecificationRepository;
        private IProductSpecificationRepository PageProductSpecificationRepository
        {
            get
            {
                if (_PageProductSpecificationRepository == null)
                    _PageProductSpecificationRepository = new ProductSpecificationRepository();

                return _PageProductSpecificationRepository;
            }
        }

        private IStockInDetailRepository _PageStockInDetailRepository;
        private IStockInDetailRepository PageStockInDetailRepository
        {
            get
            {
                if (_PageStockInDetailRepository == null)
                    _PageStockInDetailRepository = new StockInDetailRepository();

                return _PageStockInDetailRepository;
            }
        }

        private IStockOutDetailRepository _PageStockOutDetailRepository;
        private IStockOutDetailRepository PageStockOutDetailRepository
        {
            get
            {
                if (_PageStockOutDetailRepository == null)
                    _PageStockOutDetailRepository = new StockOutDetailRepository();

                return _PageStockOutDetailRepository;
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

        private FactoryManagerRefundApplication _CurrentEntity;
        private FactoryManagerRefundApplication CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageFMRefundAppRepository.GetByID(this.CurrentEntityID);

                return _CurrentEntity;
            }
        }

        private IList<int> _CanAccessUserIDs;
        private IList<int> CanAccessUserIDs
        {
            get
            {
                if (_CanAccessUserIDs == null || _CanAccessUserIDs.Count == 0)
                {
                    if (this.CurrentEntity == null)
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewFMRefund);
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
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditFMRefund);

                return _CanEditUserIDs;
            }
        }

        private IList<int> _CanAuditUserIDs;
        private IList<int> CanAuditUserIDs
        {
            get
            {
                if (_CanAuditUserIDs == null)
                    _CanAuditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByIDs(new List<int> {
                        (int)EWorkflowStep.AuditFMRefundByTreasurers,
                        (int)EWorkflowStep.AuditFMRefundByDeptManagers
                    });

                return _CanAuditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.FactoryManagerRefunds;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.FactoryManagerRefundsManage;

            if (!IsPostBack)
            {
                BindCompanies();

                BindClientUsers();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindCompanies()
        {
            var companies = PageCompanyRepository.GetDropdownItems();
            rcbxCompany.DataSource = companies;
            rcbxCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxCompany.DataBind();

            rcbxCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindClientUsers()
        {
            var clientUsers = PageClientUserRepository.GetDropdownItems();
            rcbxClientUser.DataSource = clientUsers;
            rcbxClientUser.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientUser.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientUser.DataBind();

            rcbxClientUser.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindProducts()
        {
            rcbxProduct.ClearSelection();
            rcbxProduct.Items.Clear();

            int companyID;

            if (int.TryParse(rcbxCompany.SelectedValue, out companyID))
            {
                var products = PageProductRepository.GetDropdownItems(new UISearchDropdownItem()
                {
                    Extension = new UISearchExtension { CompanyID = companyID }
                });

                rcbxProduct.DataSource = products;
                rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                rcbxProduct.DataBind();

                rcbxProduct.Items.Insert(0, new RadComboBoxItem("", ""));
            }
        }

        private void BindProductSpecifications()
        {
            ddlProductSpecification.ClearSelection();
            ddlProductSpecification.Items.Clear();

            if (!string.IsNullOrEmpty(rcbxProduct.SelectedValue))
            {
                int productID;
                if (int.TryParse(rcbxProduct.SelectedValue, out productID))
                {
                    var productSpecifications = PageProductSpecificationRepository.GetDropdownItems(new UISearchDropdownItem()
                    {
                        Extension = new UISearchExtension { ProductID = productID }
                    });

                    ddlProductSpecification.DataSource = productSpecifications;
                    ddlProductSpecification.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    ddlProductSpecification.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    ddlProductSpecification.DataBind();
                }
            }
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                lblCreatedOn.Text = CurrentEntity.CreatedOn.ToString("yyyy/MM/dd");
                lblCreateBy.Text = PageUsersRepository.GetUserFullNameByID(this.CurrentEntity.CreatedBy.HasValue
                    ? this.CurrentEntity.CreatedBy.Value : GlobalConst.INVALID_INT);

                rcbxCompany.SelectedValue = CurrentEntity.CompanyID.ToString();

                if (CurrentEntity.ClientUserID.HasValue && CurrentEntity.ClientUserID > 0)
                    rcbxClientUser.SelectedValue = CurrentEntity.ClientUserID.ToString();

                BindProducts();
                rcbxProduct.SelectedValue = CurrentEntity.ProductID.ToString();

                BindProductSpecifications();
                ddlProductSpecification.SelectedValue = CurrentEntity.ProductSpecificationID.ToString();

                rdpBeginDate.SelectedDate = CurrentEntity.BeginDate;
                rdpEndDate.SelectedDate = CurrentEntity.EndDate;
                lblStockInQty.Text = CurrentEntity.StockInQty.ToString();

                if (CurrentEntity.StockOutQty.HasValue)
                    lblStockOutQty.Text = CurrentEntity.StockOutQty.ToString();

                txtRefundPrice.DbValue = CurrentEntity.RefundPrice;
                txtRefundAmount.DbValue = CurrentEntity.RefundAmount;

                EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                switch (workfolwStatus)
                {
                    case EWorkflowStatus.Paid:
                        btnPrint.Visible = true;
                        break;
                }

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    btnSave.Visible = true;
                    btnSubmit.Visible = false;
                    ShowAuditControls(false);
                }
                else
                {
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
                        case EWorkflowStatus.ApprovedByTreasurers:
                            #region 已提交或财务审核同，待审核

                            DisabledBasicInfoControls();
                            divAppPayments.Visible = false;

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                            {
                                ShowAuditControls(true);

                                btnSave.Visible = true;
                                txtRefundPrice.Enabled = true;
                            }
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
                                btnSave.Visible = true;
                                btnSubmit.Visible = false;
                                btnPay.Visible = true;
                            }
                            else
                                rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                            #endregion
                            break;

                        case EWorkflowStatus.Paid:
                            #region 已支付的订单，不能修改

                            DisabledBasicInfoControls();

                            ShowSaveButtons(false);

                            ShowAuditControls(false);

                            rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;

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
                    this.Master.BaseNotification.Show("您没有权限新增厂家经理返款");
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
            btnAudit.Visible = isShow;
            btnReturn.Visible = isShow;
        }

        /// <summary>
        /// 禁用基础信息相关控件
        /// </summary>
        private void DisabledBasicInfoControls()
        {
            rcbxCompany.Enabled = false;
            rcbxClientUser.Enabled = false;
            rcbxProduct.Enabled = false;
            ddlProductSpecification.Enabled = false;
            rdpBeginDate.Enabled = false;
            rdpEndDate.Enabled = false;
            txtRefundPrice.Enabled = false;

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
            divComment.Visible = false;
            divComments.Visible = false;
            divOtherSections.Visible = false;

            lblCreatedOn.Text = DateTime.Now.ToString("yyyy/MM/dd");
            lblCreateBy.Text = CurrentUser.FullName;
        }

        /// <summary>
        /// Cals the stock qty and refund amount.
        /// </summary>
        private void CalStockQtyAndRefundAmount()
        {
            if (!string.IsNullOrEmpty(rcbxCompany.SelectedValue)
                && !string.IsNullOrEmpty(rcbxProduct.SelectedValue)
                && !string.IsNullOrEmpty(ddlProductSpecification.SelectedValue)
                && rdpBeginDate.SelectedDate.HasValue
                && rdpEndDate.SelectedDate.HasValue)
            {
                int companyID = int.Parse(rcbxCompany.SelectedValue);
                int productID = int.Parse(rcbxProduct.SelectedValue);
                int productSpecificationID = int.Parse(ddlProductSpecification.SelectedValue);

                DateTime beginDate = rdpBeginDate.SelectedDate.Value;
                DateTime endDate = rdpEndDate.SelectedDate.Value.AddDays(1);

                int stockInQty = 0;
                int stockOutQty = 0;

                var queryStockInQty = PageStockInDetailRepository.GetList(x => x.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse
                    && x.StockIn.Supplier.CompanyID == companyID && x.ProductID == productID && x.ProductSpecificationID == productSpecificationID
                    && x.StockIn.EntryDate >= beginDate && x.StockIn.EntryDate < endDate);

                if (queryStockInQty.Count() > 0)
                    stockInQty = queryStockInQty.Sum(x => x.InQty);

                lblStockInQty.Text = stockInQty.ToString();

                if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
                {
                    int clientUserID = int.Parse(rcbxClientUser.SelectedValue);

                    var saleOrderTypeIDs = new List<int>
                    {
                        (int)ESaleOrderType.AttachedMode,
                        (int)ESaleOrderType.AttractBusinessMode
                    };

                    var queryStockOutQty = PageStockOutDetailRepository.GetList(x => x.StockOut.CompanyID == companyID
                        && x.ProductID == productID && x.ProductSpecificationID == productSpecificationID
                        && x.StockOut.WorkflowStatusID == (int)EWorkflowStatus.OutWarehouse
                        && x.StockOut.OutDate >= beginDate && x.StockOut.OutDate < endDate
                        && saleOrderTypeIDs.Contains(x.SalesOrderApplication.SaleOrderTypeID)
                        && x.SalesOrderApplication.ClientSaleApplication.Any(y => y.ClientUserID == clientUserID));

                    if (queryStockOutQty.Count() > 0)
                        stockOutQty = queryStockOutQty.Sum(x => x.OutQty);
                }

                lblStockOutQty.Text = stockOutQty.ToString();

                if (txtRefundPrice.Value.HasValue)
                {
                    decimal refundAmount = (stockInQty - stockOutQty) * (decimal)(txtRefundPrice.Value ?? 0D);

                    txtRefundAmount.DbValue = refundAmount;
                }
            }
        }

        /// <summary>
        /// 保存客户订单基本信息
        /// </summary>
        private bool SaveFMRefundAppBasicData(FactoryManagerRefundApplication currentEntity)
        {
            bool isSucceedSaved = false;

            int companyID = int.Parse(rcbxCompany.SelectedValue);
            int productID = int.Parse(rcbxProduct.SelectedValue);
            int productSpecificationID = int.Parse(ddlProductSpecification.SelectedValue);
            decimal refundPrice = (decimal)(txtRefundPrice.Value ?? 0D);

            DateTime beginDate = rdpBeginDate.SelectedDate.Value;
            DateTime endDate = rdpEndDate.SelectedDate.Value.AddDays(1);

            int stockInQty = 0;
            int stockOutQty = 0;
            decimal refundAmount = 0M;

            var queryStockInQty = PageStockInDetailRepository.GetList(x => x.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse
                && x.StockIn.Supplier.CompanyID == companyID && x.ProductID == productID && x.ProductSpecificationID == productSpecificationID
                && x.StockIn.EntryDate >= beginDate && x.StockIn.EntryDate < endDate);

            if (queryStockInQty.Count() > 0)
                stockInQty = queryStockInQty.Sum(x => x.InQty);

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
            {
                int clientUserID = int.Parse(rcbxClientUser.SelectedValue);

                currentEntity.ClientUserID = clientUserID;

                var saleOrderTypeIDs = new List<int>
                    {
                        (int)ESaleOrderType.AttachedMode,
                        (int)ESaleOrderType.AttractBusinessMode
                    };

                var queryStockOutQty = PageStockOutDetailRepository.GetList(x => x.StockOut.CompanyID == companyID
                    && x.ProductID == productID && x.ProductSpecificationID == productSpecificationID
                    && x.StockOut.WorkflowStatusID == (int)EWorkflowStatus.OutWarehouse
                    && x.StockOut.OutDate >= beginDate && x.StockOut.OutDate < endDate
                    && saleOrderTypeIDs.Contains(x.SalesOrderApplication.SaleOrderTypeID)
                    && x.SalesOrderApplication.ClientSaleApplication.Any(y => y.ClientUserID == clientUserID));

                if (queryStockOutQty.Count() > 0)
                    stockOutQty = queryStockOutQty.Sum(x => x.OutQty);
            }

            refundAmount = (stockInQty - stockOutQty) * refundPrice;

            if (refundAmount <= 0)
            {
                cvRefundAmount.IsValid = false;
                cvRefundAmount.ErrorMessage = "返款金额不正确，请修改条件";
                return isSucceedSaved;
            }

            var tempFMRefundAppCount = PageFMRefundAppRepository.GetList(x => x.ID != currentEntity.ID && x.CompanyID == companyID
                && x.ProductID == productID && x.ProductSpecificationID == productSpecificationID
                && ((beginDate >= x.BeginDate && beginDate <= x.EndDate)
                || (endDate >= x.BeginDate && endDate <= x.EndDate))).Count();

            if (tempFMRefundAppCount > 0)
            {
                cvBeginDate.IsValid = false;
                cvBeginDate.ErrorMessage = "结算日期区间跟系统已有数据有重叠，请重新选择日期和其他条件";

                return isSucceedSaved;
            }

            currentEntity.CompanyID = companyID;
            currentEntity.ProductID = productID;
            currentEntity.ProductSpecificationID = productSpecificationID;
            currentEntity.BeginDate = rdpBeginDate.SelectedDate.Value;
            currentEntity.EndDate = rdpEndDate.SelectedDate.Value;
            currentEntity.StockInQty = stockInQty;
            currentEntity.StockOutQty = stockOutQty;
            currentEntity.RefundPrice = refundPrice;
            currentEntity.RefundAmount = refundAmount;

            PageFMRefundAppRepository.Save();

            if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = (int)EWorkflow.FactoryManagerRefunds;
                appNote.NoteTypeID = (int)EAppNoteType.Comment;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                    appNote.WorkflowStepID = (int)EWorkflowStep.EditFMRefund;
                else
                    appNote.WorkflowStepID = (int)EWorkflowStep.NewFMRefund;

                appNote.ApplicationID = currentEntity.ID;
                appNote.Note = txtComment.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }

            if (!string.IsNullOrEmpty(txtAuditComment.Text.Trim())
                && CanAuditUserIDs.Contains(CurrentUser.UserID))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = (int)EWorkflow.FactoryManagerRefunds;
                appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;

                if (currentEntity.WorkflowStatusID == (int)EWorkflowStatus.Submit)
                    appNote.WorkflowStepID = (int)EWorkflowStep.AuditFMRefundByTreasurers;
                else
                    appNote.WorkflowStepID = (int)EWorkflowStep.AuditFMRefundByDeptManagers;

                appNote.ApplicationID = currentEntity.ID;
                appNote.Note = txtAuditComment.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }

            isSucceedSaved = true;

            return isSucceedSaved;
        }


        #endregion

        protected void rcbxCompany_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindProducts();

            CalStockQtyAndRefundAmount();
        }

        protected void rcbxProduct_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindProductSpecifications();

            CalStockQtyAndRefundAmount();
        }

        protected void rcbxClientUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            CalStockQtyAndRefundAmount();
        }

        protected void rdpBeginDate_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
        {
            if (e.NewDate.HasValue)
                rdpEndDate.MinDate = e.NewDate.Value;

            CalStockQtyAndRefundAmount();
        }

        protected void rdpEndDate_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
        {
            if (e.NewDate.HasValue)
                rdpBeginDate.MaxDate = e.NewDate.Value;

            CalStockQtyAndRefundAmount();
        }

        protected void txtRefundPrice_TextChanged(object sender, EventArgs e)
        {
            CalStockQtyAndRefundAmount();
        }

        protected void ddlProductSpecification_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            CalStockQtyAndRefundAmount();
        }

        #region Grid events

        #region App notes events

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

                    if (e.Item.ItemIndex < 0)
                    {
                        var excludeItemValues = PageAppPaymentRepository
                        .GetList(x => x.WorkflowID == CurrentWorkFlowID && x.ApplicationID == this.CurrentEntityID)
                        .Select(x => x.FromBankAccountID.HasValue ? x.FromBankAccountID.Value : GlobalConst.INVALID_INT)
                        .ToList();

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

                var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                if (txtAmount != null && uiEntity != null)
                    txtAmount.DbValue = uiEntity.Amount;

                var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                if (txtFee != null && uiEntity != null)
                    txtFee.DbValue = uiEntity.Fee;

                var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                if (txtComment != null && uiEntity != null)
                    txtComment.Text = uiEntity.Comment;
            }
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

                var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                appPayment.Comment = txtComment.Text;

                appPayment.ApplicationID = this.CurrentEntityID.Value;
                appPayment.WorkflowID = this.CurrentWorkFlowID;
                appPayment.PaymentStatusID = (int)EPaymentStatus.ToBePaid;
                appPayment.PaymentTypeID = (int)EPaymentType.Expend;

                PageAppPaymentRepository.Add(appPayment);

                PageAppPaymentRepository.Save();

                rgAppPayments.Rebind();
            }
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

                    var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                    appPayment.Comment = txtComment.Text;

                    appPayment.ApplicationID = this.CurrentEntityID.Value;
                    appPayment.WorkflowID = this.CurrentWorkFlowID;
                    appPayment.PaymentStatusID = (int)EPaymentStatus.ToBePaid;
                    appPayment.PaymentTypeID = (int)EPaymentType.Expend;

                    PageAppPaymentRepository.Save();

                    rgAppPayments.Rebind();
                }
            }
        }
        #endregion

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            FactoryManagerRefundApplication currentEntity = this.CurrentEntity;

            if (currentEntity == null)
            {
                currentEntity = new FactoryManagerRefundApplication();
                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.TemporarySave;

                PageFMRefundAppRepository.Add(currentEntity);
            }

            bool isSaved = SaveFMRefundAppBasicData(currentEntity);

            if (isSaved)
            {
                hdnCurrentEntityID.Value = currentEntity.ID.ToString();

                if (this.CurrentEntity != null)
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
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            if (this.CurrentEntity != null)
            {
                this.CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.Submit;

                bool isSaved = SaveFMRefundAppBasicData(this.CurrentEntity);

                if (isSaved)
                {
                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
                }
            }
        }

        protected void btnAudit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAuditComment.Text.Trim()))
                cvAuditComment.IsValid = false;

            if (!IsValid) return;

            switch (this.CurrentEntity.WorkflowStatusID)
            {
                case (int)EWorkflowStatus.Submit:
                    CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByTreasurers;
                    break;

                case (int)EWorkflowStatus.ApprovedByTreasurers:
                    CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByDeptManagers;
                    break;
            }

            bool isSaved = SaveFMRefundAppBasicData(CurrentEntity);

            if (isSaved)
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAuditComment.Text.Trim()))
                cvAuditComment.IsValid = false;

            if (!IsValid) return;

            CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

            bool isSaved = SaveFMRefundAppBasicData(CurrentEntity);

            if (isSaved)
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {

            if (CurrentEntity != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IFactoryManagerRefundAppRepository fmRefundAppRepository = new FactoryManagerRefundAppRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();

                    fmRefundAppRepository.SetDbModel(db);
                    appPaymentRepository.SetDbModel(db);

                    var currentEntity = fmRefundAppRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {

                        var appPayments = appPaymentRepository.GetList(x => x.WorkflowID == CurrentWorkFlowID
                            && x.ApplicationID == CurrentEntity.ID).ToList();

                        var totalRefundAmount = CurrentEntity.RefundAmount;

                        var totalPayAmount = appPayments.Sum(x => (x.Amount ?? 0M) + (x.Fee ?? 0M));

                        if (totalPayAmount != totalRefundAmount)
                        {
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                            this.Master.BaseNotification.AutoCloseDelay = 1000;
                            this.Master.BaseNotification.Show("支付总额不等于应返款总额，不能确认支付");
                        }
                        else
                        {
                            foreach (var item in appPayments)
                            {
                                item.PaymentStatusID = (int)EPaymentStatus.Paid;
                            }

                            currentEntity.WorkflowStatusID = (int)EWorkflowStatus.Paid;

                            unitOfWork.SaveChanges();

                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                        }
                    }
                }
            }
        }
    }
}