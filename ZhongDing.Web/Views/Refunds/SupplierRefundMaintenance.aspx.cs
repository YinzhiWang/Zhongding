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
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.Refunds
{
    public partial class SupplierRefundMaintenance : WorkflowBasePage
    {
        #region Fields

        protected int? CompanyID
        {
            get
            {
                return WebUtility.GetIntFromQueryString("CompanyID");
            }
        }

        protected int? SupplierID
        {
            get
            {
                return WebUtility.GetIntFromQueryString("SupplierID");
            }
        }

        protected int? ProductID
        {
            get
            {
                return WebUtility.GetIntFromQueryString("ProductID");
            }
        }

        protected int? ProductSpecificationID
        {
            get
            {
                return WebUtility.GetIntFromQueryString("ProductSpecificationID");
            }
        }

        #endregion

        #region Members

        private ISupplierRefundApplicationRepository _PageSupplierRefundAppRepository;
        private ISupplierRefundApplicationRepository PageSupplierRefundAppRepository
        {
            get
            {
                if (_PageSupplierRefundAppRepository == null)
                    _PageSupplierRefundAppRepository = new SupplierRefundApplicationRepository();

                return _PageSupplierRefundAppRepository;
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

        private SupplierRefundApplication _CurrentEntity;
        private SupplierRefundApplication CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageSupplierRefundAppRepository.GetByID(this.CurrentEntityID);

                return _CurrentEntity;
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

        private ISupplierDeductionRepository _PageSupplierDeductionRepository;
        private ISupplierDeductionRepository PageSupplierDeductionRepository
        {
            get
            {
                if (_PageSupplierDeductionRepository == null)
                    _PageSupplierDeductionRepository = new SupplierDeductionRepository();

                return _PageSupplierDeductionRepository;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditSupplierRefund);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.SupplierRefunds;
        }
        protected override EWorkflow PagePermissionWorkflowID()
        {
            return EWorkflow.SupplierRefunds;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.SupplierRefundsManage;

            if ((CurrentEntityID.HasValue && CurrentEntityID > 0)
                || (CurrentEntityID <= 0 && CompanyID.HasValue && CompanyID > 0 && SupplierID.HasValue && SupplierID > 0
                    && ProductID.HasValue && ProductID > 0 && ProductSpecificationID.HasValue && ProductSpecificationID > 0))
            {
                if (!IsPostBack)
                {
                    LoadCurrentEntity();
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

        #region Private Methods

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                lblSupplierName.Text = CurrentEntity.Supplier.SupplierName;
                lblProductName.Text = CurrentEntity.Product.ProductName;
            }
            else
            {
                var supplier = PageSupplierRepository.GetByID(SupplierID);
                if (supplier != null)
                    lblSupplierName.Text = supplier.SupplierName;

                var product = PageProductRepository.GetByID(ProductID);
                if (product != null)
                    lblProductName.Text = product.ProductName;
            }

            var uiSearchObj = new UISearchSupplierRefundApp()
            {
                CompanyID = this.CompanyID.Value,
                SupplierID = this.SupplierID.Value,
                ProductID = this.ProductID.Value,
                ProductSpecificationID = this.ProductSpecificationID.Value
            };

            int totalRecords;

            var uiSupplierRefundApp = PageSupplierRefundAppRepository.GetUIList(uiSearchObj, 0, 1, out totalRecords).FirstOrDefault();

            if (uiSupplierRefundApp != null)
            {
                lblNeedRefundAmount.Text = uiSupplierRefundApp.TotalNeedRefundAmount.ToString("C2");
                lblRefundedAmount.Text = uiSupplierRefundApp.TotalRefundedAmount.ToString("C2");
                lblToBeRefundAmount.Text = uiSupplierRefundApp.TotalToBeRefundAmount.ToString("C2");
            }

            if (!CanEditUserIDs.Contains(CurrentUser.UserID))
            {
                rgSupplierRefunds.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                rgSupplierDeduction.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
            }
        }

        private void BindProcureOrderAppDetails(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchSupplierRefundApp()
            {
                CompanyID = this.CompanyID.Value,
                SupplierID = this.SupplierID.Value,
                ProductID = this.ProductID.Value,
                ProductSpecificationID = this.ProductSpecificationID.Value,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate
            };

            int totalRecords;

            var uiProcureOrderAppDetails = PageSupplierRefundAppRepository.GetDetails(uiSearchObj, rgProcureOrderAppDetails.CurrentPageIndex, rgProcureOrderAppDetails.PageSize, out totalRecords);

            rgProcureOrderAppDetails.DataSource = uiProcureOrderAppDetails;
            rgProcureOrderAppDetails.VirtualItemCount = totalRecords;

            if (isNeedRebind)
                rgProcureOrderAppDetails.Rebind();
        }

        private void BindSupplierRefunds(bool isNeedRebind)
        {
            IList<UIApplicationPayment> appPayments = new List<UIApplicationPayment>();

            int totalRecords = 0;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                var uiSearchObj = new UISearchApplicationPayment
                {
                    WorkflowID = this.CurrentWorkFlowID,
                    ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
                    BeginDate = rdpRefundBeginDate.SelectedDate,
                    EndDate = rdpRefundEndDate.SelectedDate,
                };

                appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj, rgSupplierRefunds.CurrentPageIndex, rgSupplierRefunds.PageSize, out totalRecords);
            }

            rgSupplierRefunds.DataSource = appPayments;
            rgSupplierRefunds.VirtualItemCount = totalRecords;

            if (isNeedRebind)
                rgSupplierRefunds.Rebind();
        }

        private void BindSupplierDeductions(bool isNeedRebind)
        {
            IList<UISupplierDeduction> supplierDeductions = new List<UISupplierDeduction>();

            int totalRecords = 0;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                var uiSearchObj = new UISearchSupplierDeduction
                {
                    SupplierRefundAppID = this.CurrentEntityID.Value,
                    BeginDate = rdpDeductionBeginDate.SelectedDate,
                    EndDate = rdpDeductionEndDate.SelectedDate,
                };

                supplierDeductions = PageSupplierDeductionRepository.GetUIList(uiSearchObj, rgSupplierDeduction.CurrentPageIndex, rgSupplierDeduction.PageSize, out totalRecords);
            }


            rgSupplierDeduction.DataSource = supplierDeductions;
            rgSupplierDeduction.VirtualItemCount = totalRecords;

            if (isNeedRebind)
                rgSupplierDeduction.Rebind();
        }

        #endregion

        protected void rgProcureOrderAppDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindProcureOrderAppDetails(false);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindProcureOrderAppDetails(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpBeginDate.Clear();
            rdpEndDate.Clear();

            BindProcureOrderAppDetails(true);
        }

        protected void rgSupplierRefunds_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindSupplierRefunds(false);
        }

        protected void rgSupplierRefunds_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;

                UIApplicationPayment uiEntity = null;

                if (e.Item.ItemIndex >= 0)
                    uiEntity = (UIApplicationPayment)gridDataItem.DataItem;

                var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");

                if (rcbxToAccount != null)
                {
                    var uiSearchObj = new UISearchDropdownItem
                    {
                        Extension = new UISearchExtension
                        {
                            OwnerTypeID = (int)EOwnerType.Company,
                            CompanyID = this.CompanyID.HasValue ? this.CompanyID.Value : GlobalConst.INVALID_INT
                        }
                    };

                    //if (e.Item.ItemIndex < 0)
                    //{
                    //    var excludeItemValues = PageAppPaymentRepository
                    //        .GetList(x => x.ApplicationID == this.CurrentEntityID)
                    //        .Select(x => x.ToBankAccountID.HasValue ? x.ToBankAccountID.Value : GlobalConst.INVALID_INT)
                    //        .ToList();

                    //    if (excludeItemValues.Count > 0)
                    //        uiSearchObj.ExcludeItemValues = excludeItemValues;
                    //}

                    var bankAccounts = PageBankAccountRepository.GetDropdownItems(uiSearchObj);
                    rcbxToAccount.DataSource = bankAccounts;
                    rcbxToAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    rcbxToAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    rcbxToAccount.DataBind();

                    if (uiEntity != null)
                        rcbxToAccount.SelectedValue = uiEntity.ToBankAccountID.ToString();
                }

                var rdpPayDate = (RadDatePicker)e.Item.FindControl("rdpPayDate");

                if (rdpPayDate != null)
                    rdpPayDate.MaxDate = DateTime.Now;

                if (uiEntity != null)
                {
                    if (rdpPayDate != null)
                        rdpPayDate.SelectedDate = uiEntity.PayDate;

                    var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                    if (txtAmount != null)
                        txtAmount.DbValue = uiEntity.Amount;

                    var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                    if (txtFee != null)
                        txtFee.DbValue = uiEntity.Fee;

                    var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                    if (txtComment != null)
                        txtComment.Text = uiEntity.Comment;
                }
            }
        }

        protected void rgSupplierRefunds_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Insert")
            {
                if (!IsValid)
                {
                    e.Canceled = true;
                }
                else
                {
                    if (e.Item is GridDataItem)
                    {
                        GridDataItem dataItem = e.Item as GridDataItem;

                        ApplicationPayment appPayment = new ApplicationPayment();

                        var rdpPayDate = (RadDatePicker)e.Item.FindControl("rdpPayDate");
                        if (rdpPayDate != null)
                            appPayment.PayDate = rdpPayDate.SelectedDate;

                        var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");

                        if (!string.IsNullOrEmpty(rcbxToAccount.SelectedValue))
                        {
                            int toAccountID;
                            if (int.TryParse(rcbxToAccount.SelectedValue, out toAccountID))
                                appPayment.ToBankAccountID = toAccountID;
                            appPayment.ToAccount = rcbxToAccount.SelectedItem.Text;
                        }

                        var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                        if (txtAmount != null)
                            appPayment.Amount = (decimal?)txtAmount.Value;

                        var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                        if (txtFee != null)
                            appPayment.Fee = (decimal?)txtFee.Value;

                        {
                            var uiSearchObj = new UISearchSupplierRefundApp()
                            {
                                CompanyID = this.CompanyID.Value,
                                SupplierID = this.SupplierID.Value,
                                ProductID = this.ProductID.Value,
                                ProductSpecificationID = this.ProductSpecificationID.Value
                            };

                            int totalRecords;

                            var uiSupplierRefundApp = PageSupplierRefundAppRepository.GetUIList(uiSearchObj, 0, 1, out totalRecords).FirstOrDefault();

                            if (uiSupplierRefundApp != null)
                            {
                                if ((uiSupplierRefundApp.TotalRefundedAmount
                                     + appPayment.Amount.GetValueOrDefault(0)
                                    + appPayment.Fee.GetValueOrDefault(0)) > uiSupplierRefundApp.TotalNeedRefundAmount)
                                {
                                    ((CustomValidator)e.Item.FindControl("cvAmount")).IsValid = false;
                                    ((CustomValidator)e.Item.FindControl("cvFee")).IsValid = false;
                                    e.Canceled = true;
                                    return;
                                }
                            }
                        }

                        var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                        if (txtComment != null)
                            appPayment.Comment = txtComment.Text;

                        int currentEntityID;
                        int.TryParse(hdnCurrentEntityID.Value, out currentEntityID);

                        if (currentEntityID <= 0)
                        {
                            var supplierRefundApp = PageSupplierRefundAppRepository.GetList(x => x.CompanyID == this.CompanyID.Value
                                && x.SupplierID == this.SupplierID.Value && x.ProductID == this.ProductID.Value
                                && x.ProductSpecificationID == this.ProductSpecificationID.Value).FirstOrDefault();

                            if (supplierRefundApp == null)
                            {
                                supplierRefundApp = new SupplierRefundApplication
                                {
                                    CompanyID = this.CompanyID.Value,
                                    SupplierID = this.SupplierID.Value,
                                    ProductID = this.ProductID.Value,
                                    ProductSpecificationID = this.ProductSpecificationID.Value,
                                    WorkflowID = this.CurrentWorkFlowID
                                };

                                PageSupplierRefundAppRepository.Add(supplierRefundApp);
                                PageSupplierRefundAppRepository.Save();
                            }

                            appPayment.ApplicationID = supplierRefundApp.ID;

                            hdnCurrentEntityID.Value = supplierRefundApp.ID.ToString();
                        }
                        else
                            appPayment.ApplicationID = this.CurrentEntityID.Value;

                        appPayment.WorkflowID = this.CurrentWorkFlowID;
                        appPayment.PaymentStatusID = (int)EPaymentStatus.Paid;
                        appPayment.PaymentTypeID = (int)EPaymentType.Income;

                        PageAppPaymentRepository.Add(appPayment);

                        PageAppPaymentRepository.Save();

                        hdnNeedRefreshPage.Value = true.ToString();

                        rgSupplierRefunds.Rebind();
                    }
                }
            }
        }




        protected void btnSearchRefund_Click(object sender, EventArgs e)
        {
            BindSupplierRefunds(true);
        }

        protected void btnResetRefund_Click(object sender, EventArgs e)
        {
            rdpRefundBeginDate.Clear();
            rdpRefundEndDate.Clear();

            BindSupplierRefunds(true);
        }

        protected void rgSupplierDeduction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindSupplierDeductions(false);
        }

        protected void rgSupplierDeduction_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;

                UISupplierDeduction uiEntity = null;

                if (e.Item.ItemIndex >= 0)
                    uiEntity = (UISupplierDeduction)gridDataItem.DataItem;

                var rcbxSupplier = (RadComboBox)e.Item.FindControl("rcbxSupplier");

                if (rcbxSupplier != null)
                {
                    var uiSearchObj = new UISearchDropdownItem
                    {
                        Extension = new UISearchExtension
                        {
                            CompanyID = this.CompanyID.HasValue ? this.CompanyID.Value : GlobalConst.INVALID_INT
                        }
                    };

                    if (e.Item.ItemIndex < 0)
                    {
                        var excludeItemValues = PageSupplierDeductionRepository
                            .GetList(x => x.SupplierRefundAppID == this.CurrentEntityID)
                            .Select(x => x.SupplierID)
                            .ToList();

                        if (excludeItemValues.Count > 0)
                            uiSearchObj.ExcludeItemValues = excludeItemValues;
                    }

                    var suppliers = PageSupplierRepository.GetDropdownItems(uiSearchObj);
                    rcbxSupplier.DataSource = suppliers;
                    rcbxSupplier.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    rcbxSupplier.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    rcbxSupplier.DataBind();

                    if (uiEntity != null)
                        rcbxSupplier.SelectedValue = uiEntity.SupplierID.ToString();
                }

                var rdpDeductedDate = (RadDatePicker)e.Item.FindControl("rdpDeductedDate");

                if (rdpDeductedDate != null)
                    rdpDeductedDate.MaxDate = DateTime.Now;

                if (uiEntity != null)
                {
                    if (rdpDeductedDate != null)
                        rdpDeductedDate.SelectedDate = uiEntity.DeductedDate;

                    var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                    if (txtAmount != null)
                        txtAmount.DbValue = uiEntity.Amount;

                    var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                    if (txtComment != null)
                        txtComment.Text = uiEntity.Comment;
                }
            }
        }

        protected void rgSupplierDeduction_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Insert")
            {
                if (!IsValid)
                {
                    e.Canceled = true;
                }
                else
                {
                    if (e.Item is GridDataItem)
                    {
                        GridDataItem dataItem = e.Item as GridDataItem;

                        SupplierDeduction supplierDeduction = new SupplierDeduction();

                        var rdpDeductedDate = (RadDatePicker)e.Item.FindControl("rdpDeductedDate");
                        if (rdpDeductedDate != null)
                            supplierDeduction.DeductedDate = rdpDeductedDate.SelectedDate;

                        var rcbxSupplier = (RadComboBox)e.Item.FindControl("rcbxSupplier");

                        if (!string.IsNullOrEmpty(rcbxSupplier.SelectedValue))
                        {
                            int supplierID;
                            if (int.TryParse(rcbxSupplier.SelectedValue, out supplierID))
                                supplierDeduction.SupplierID = supplierID;
                        }

                        var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                        if (txtAmount != null && txtAmount.Value.HasValue)
                            supplierDeduction.Amount = (decimal)txtAmount.Value;

                        {
                            var uiSearchObj = new UISearchSupplierRefundApp()
                            {
                                CompanyID = this.CompanyID.Value,
                                SupplierID = this.SupplierID.Value,
                                ProductID = this.ProductID.Value,
                                ProductSpecificationID = this.ProductSpecificationID.Value
                            };

                            int totalRecords;

                            var uiSupplierRefundApp = PageSupplierRefundAppRepository.GetUIList(uiSearchObj, 0, 1, out totalRecords).FirstOrDefault();

                            if ((uiSupplierRefundApp.TotalRefundedAmount + supplierDeduction.Amount) > uiSupplierRefundApp.TotalNeedRefundAmount)
                            {
                                ((CustomValidator)e.Item.FindControl("cvAmount")).IsValid = false;
                                e.Canceled = true;
                                return;
                            }
                        }

                        var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                        if (txtComment != null)
                            supplierDeduction.Comment = txtComment.Text;

                        int currentEntityID;
                        int.TryParse(hdnCurrentEntityID.Value, out currentEntityID);

                        var supplierRefundApp = PageSupplierRefundAppRepository.GetByID(currentEntityID);

                        if (supplierRefundApp == null)
                        {
                            supplierRefundApp = PageSupplierRefundAppRepository.GetList(x => x.CompanyID == this.CompanyID.Value
                                && x.SupplierID == this.SupplierID.Value && x.ProductID == this.ProductID.Value
                                && x.ProductSpecificationID == this.ProductSpecificationID.Value).FirstOrDefault();

                            if (supplierRefundApp == null)
                            {
                                supplierRefundApp = new SupplierRefundApplication
                                {
                                    CompanyID = this.CompanyID.Value,
                                    SupplierID = this.SupplierID.Value,
                                    ProductID = this.ProductID.Value,
                                    ProductSpecificationID = this.ProductSpecificationID.Value,
                                    WorkflowID = this.CurrentWorkFlowID
                                };

                                PageSupplierRefundAppRepository.Add(supplierRefundApp);
                            }
                        }

                        supplierRefundApp.SupplierDeduction.Add(supplierDeduction);

                        PageSupplierRefundAppRepository.Save();

                        hdnCurrentEntityID.Value = supplierRefundApp.ID.ToString();

                        hdnNeedRefreshPage.Value = true.ToString();

                        rgSupplierRefunds.Rebind();
                    }
                }
            }
        }

        protected void btnSearchDeduction_Click(object sender, EventArgs e)
        {
            BindSupplierDeductions(true);
        }

        protected void btnResetDeduction_Click(object sender, EventArgs e)
        {
            rdpDeductionBeginDate.Clear();
            rdpDeductionEndDate.Clear();

            BindSupplierDeductions(true);
        }

    }
}