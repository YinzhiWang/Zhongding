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

namespace ZhongDing.Web.Views.Refunds
{
    public partial class SupplierTaskRefundMaintenance : WorkflowBasePage
    {
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

        private ICompanyRepository _PageCompanyRepository;
        private ICompanyRepository PageCompanyRepository
        {
            get
            {
                if (_PageCompanyRepository == null)
                {
                    _PageCompanyRepository = new CompanyRepository();
                }
                return _PageCompanyRepository;
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

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditSupplierTaskRefund);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.SupplierTaskRefunds;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.SupplierTaskRefundsManage;

            if (!IsPostBack)
            {
                BindCompany();

                BindPaymentMethods();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindCompany()
        {
            var companies = PageCompanyRepository.GetDropdownItems();
            rcbxCompany.DataSource = companies;
            rcbxCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxCompany.DataBind();

            rcbxCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindSuppliers()
        {
            rcbxSupplier.ClearSelection();
            rcbxDeductSupplier.ClearSelection();

            IList<UIDropdownItem> suppliers = new List<UIDropdownItem>();

            if (!string.IsNullOrEmpty(rcbxCompany.SelectedValue))
            {
                int companyID;

                if (int.TryParse(rcbxCompany.SelectedValue, out companyID))
                {
                    suppliers = PageSupplierRepository.GetDropdownItems(new UISearchDropdownItem
                    {
                        Extension = new UISearchExtension
                        {
                            CompanyID = companyID
                        }
                    });
                }
            }

            rcbxSupplier.DataSource = suppliers;
            rcbxSupplier.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxSupplier.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxSupplier.DataBind();

            rcbxSupplier.Items.Insert(0, new RadComboBoxItem("", ""));

            rcbxDeductSupplier.DataSource = suppliers;
            rcbxDeductSupplier.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDeductSupplier.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDeductSupplier.DataBind();

            rcbxDeductSupplier.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindProducts()
        {
            rcbxProduct.ClearSelection();

            IList<UIDropdownItem> products = new List<UIDropdownItem>();

            var uiSearchObj = new UISearchDropdownItem { Extension = new UISearchExtension() };

            int companyID = 0;
            int supplierID = 0;

            int.TryParse(rcbxCompany.SelectedValue, out companyID);
            int.TryParse(rcbxSupplier.SelectedValue, out supplierID);

            if (companyID > 0 || supplierID > 0)
            {
                if (companyID > 0)
                    uiSearchObj.Extension.CompanyID = companyID;

                if (supplierID > 0)
                    uiSearchObj.Extension.SupplierID = supplierID;

                products = PageProductRepository.GetDropdownItems(uiSearchObj);
            }

            rcbxProduct.DataSource = products;
            rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxProduct.DataBind();

            rcbxProduct.Items.Insert(0, new RadComboBoxItem("", ""));
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

        private void BindPaymentMethods()
        {
            ddlPaymentMethod.Items.Add(new DropDownListItem(GlobalConst.PaymentMethods.BANK_TRANSFER,
                ((int)EPaymentMethod.BankTransfer).ToString()));
            ddlPaymentMethod.Items.Add(new DropDownListItem(GlobalConst.PaymentMethods.DEDUCATION,
                ((int)EPaymentMethod.Deduction).ToString()));

            ddlPaymentMethod.DataBind();
        }

        private void BindBankAccounts()
        {
            rcbxToAccount.ClearSelection();
            rcbxToAccount.Items.Clear();

            IList<UIDropdownItem> bankAccounts = new List<UIDropdownItem>();

            int companyID = 0;
            int.TryParse(rcbxCompany.SelectedValue, out companyID);
            if (companyID > 0)
            {
                var uiSearchObj = new UISearchDropdownItem
                {
                    Extension = new UISearchExtension
                    {
                        OwnerTypeID = (int)EOwnerType.Company,
                        CompanyID = companyID
                    }
                };

                bankAccounts = PageBankAccountRepository.GetDropdownItems(uiSearchObj);
            }

            rcbxToAccount.DataSource = bankAccounts;
            rcbxToAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxToAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxToAccount.DataBind();
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                if (CurrentEntity.WorkflowID != (int)EWorkflow.SupplierTaskRefunds)
                {
                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_REDIRECT);

                    return;
                }

                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                rcbxCompany.SelectedValue = CurrentEntity.CompanyID.ToString();

                BindSuppliers();
                rcbxSupplier.SelectedValue = CurrentEntity.SupplierID.ToString();

                BindProducts();
                rcbxProduct.SelectedValue = CurrentEntity.ProductID.ToString();

                BindProductSpecifications();
                ddlProductSpecification.SelectedValue = CurrentEntity.ProductSpecificationID.ToString();

                rdpBeginDate.SelectedDate = CurrentEntity.BeginDate;
                rdpEndDate.SelectedDate = CurrentEntity.EndDate;

                rdpRefundDate.SelectedDate = CurrentEntity.RefundDate;
                txtRefundAmount.DbValue = CurrentEntity.RefundAmount;

                int paymentMethodID = CurrentEntity.PaymentMethodID.HasValue
                    ? CurrentEntity.PaymentMethodID.Value : (int)EPaymentMethod.BankTransfer;

                BindBankAccounts();

                ddlPaymentMethod.SelectedValue = paymentMethodID.ToString();

                if (paymentMethodID == (int)EPaymentMethod.BankTransfer)
                {
                    divRefundAccount.Visible = true;
                    divDeductSupplier.Visible = false;

                    var appPayment = PageAppPaymentRepository.GetList(x => x.WorkflowID == this.CurrentWorkFlowID
                        && x.ApplicationID == this.CurrentEntityID).FirstOrDefault();

                    if (appPayment != null)
                        rcbxToAccount.SelectedValue = appPayment.ToBankAccountID.ToString();
                }
                else if (paymentMethodID == (int)EPaymentMethod.Deduction)
                {
                    divRefundAccount.Visible = false;
                    divDeductSupplier.Visible = true;

                    var supplierDeduction = CurrentEntity.SupplierDeduction.FirstOrDefault(x => x.IsDeleted == false);

                    if (supplierDeduction != null)
                        rcbxDeductSupplier.SelectedValue = supplierDeduction.SupplierID.ToString();
                }

                //不允许修改
                rcbxCompany.Enabled = false;
                rcbxSupplier.Enabled = false;
                rcbxProduct.Enabled = false;
                ddlProductSpecification.Enabled = false;
                rdpBeginDate.Enabled = false;
                rdpEndDate.Enabled = false;
                rdpRefundDate.Enabled = false;
                ddlPaymentMethod.Enabled = false;
                txtRefundAmount.Enabled = false;
                rcbxToAccount.Enabled = false;
                rcbxDeductSupplier.Enabled = false;
                txtComment.Enabled = false;
                btnSave.Visible = false;
            }
            else
            {
                int paymentMethodID = (int)EPaymentMethod.BankTransfer;
                ddlPaymentMethod.SelectedValue = paymentMethodID.ToString();

                divComments.Visible = false;

                if (!this.CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show("您没有权限新增厂家经理返款");
                }
            }
        }

        #endregion

        protected void rcbxCompany_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindSuppliers();

            BindProducts();

            BindProductSpecifications();

            BindBankAccounts();
        }

        protected void rcbxSupplier_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindProducts();

            BindProductSpecifications();
        }

        protected void rcbxProduct_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindProductSpecifications();
        }

        protected void ddlPaymentMethod_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                int paymentMethodID = int.Parse(e.Value);

                if (paymentMethodID == (int)EPaymentMethod.BankTransfer)
                {
                    divRefundAccount.Visible = true;
                    divDeductSupplier.Visible = false;
                }
                else if (paymentMethodID == (int)EPaymentMethod.Deduction)
                {
                    divRefundAccount.Visible = false;
                    divDeductSupplier.Visible = true;
                }
            }
        }

        protected void rdpBeginDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (e.NewDate.HasValue)
                rdpEndDate.MinDate = e.NewDate.Value;
        }

        protected void rdpEndDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (e.NewDate.HasValue)
                rdpBeginDate.MaxDate = e.NewDate.Value;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int paymentMethodID;
            if (int.TryParse(ddlPaymentMethod.SelectedValue, out paymentMethodID))
            {
                if (paymentMethodID == (int)EPaymentMethod.BankTransfer)
                {
                    if (string.IsNullOrEmpty(rcbxToAccount.SelectedValue))
                        cvToAccount.IsValid = false;
                }
                else if (paymentMethodID == (int)EPaymentMethod.Deduction)
                {
                    if (string.IsNullOrEmpty(rcbxDeductSupplier.SelectedValue))
                        cvDeductSupplier.IsValid = false;
                }
            }

            if (!IsValid) return;

            int companyID = int.Parse(rcbxCompany.SelectedValue);
            int supplierID = int.Parse(rcbxSupplier.SelectedValue);
            int productID = int.Parse(rcbxProduct.SelectedValue);
            int productSpecificationID = int.Parse(ddlProductSpecification.SelectedValue);
            DateTime beginDate = rdpBeginDate.SelectedDate.Value;
            DateTime endDate = rdpEndDate.SelectedDate.Value;

            var tempRefundAppCount = PageSupplierRefundAppRepository.GetList(x => x.ID != this.CurrentEntityID
                && x.CompanyID == companyID && x.SupplierID == supplierID && x.ProductID == productID
                && x.ProductSpecificationID == productSpecificationID
                && ((beginDate >= x.BeginDate && beginDate <= x.EndDate)
                || (endDate >= x.BeginDate && endDate <= x.EndDate))).Count();

            if (tempRefundAppCount > 0)
            {
                cvBeginDate.IsValid = false;
                cvBeginDate.ErrorMessage = "日期区间与系统已有数据有重叠，请重新选择日期和其他条件";

                return;
            }

            SupplierRefundApplication currentEntity = this.CurrentEntity;

            if (currentEntity == null)
            {
                currentEntity = new SupplierRefundApplication();
                currentEntity.WorkflowID = (int)EWorkflow.SupplierTaskRefunds;

                PageSupplierRefundAppRepository.Add(currentEntity);
            }

            currentEntity.CompanyID = companyID;
            currentEntity.SupplierID = supplierID;
            currentEntity.ProductID = productID;
            currentEntity.ProductSpecificationID = productSpecificationID;
            currentEntity.BeginDate = beginDate;
            currentEntity.EndDate = endDate;
            currentEntity.RefundDate = rdpRefundDate.SelectedDate;
            currentEntity.RefundAmount = (decimal?)txtRefundAmount.Value;

            int curPaymenyMethodID = int.Parse(ddlPaymentMethod.SelectedValue);
            currentEntity.PaymentMethodID = curPaymenyMethodID;

            if (curPaymenyMethodID == (int)EPaymentMethod.Deduction)
            {
                var supplierDeduction = currentEntity.SupplierDeduction.FirstOrDefault(x => x.IsDeleted == false);

                if (supplierDeduction == null)
                {
                    supplierDeduction = new SupplierDeduction();
                    currentEntity.SupplierDeduction.Add(supplierDeduction);
                }

                supplierDeduction.SupplierID = supplierID;
                supplierDeduction.DeductedDate = rdpRefundDate.SelectedDate;
                supplierDeduction.Amount = (decimal)(txtRefundAmount.Value ?? 0D);
            }

            PageSupplierRefundAppRepository.Save();

            if (curPaymenyMethodID == (int)EPaymentMethod.BankTransfer)
            {
                var appPayment = PageAppPaymentRepository.GetList(x => x.WorkflowID == (int)EWorkflow.SupplierTaskRefunds
                    && x.ApplicationID == currentEntity.ID).FirstOrDefault();

                if (appPayment == null)
                {
                    appPayment = new ApplicationPayment
                    {
                        WorkflowID = (int)EWorkflow.SupplierTaskRefunds,
                        ApplicationID = currentEntity.ID,
                        PaymentTypeID = (int)EPaymentType.Income,
                        PaymentStatusID = (int)EPaymentStatus.Paid,
                    };

                    PageAppPaymentRepository.Add(appPayment);
                }

                appPayment.ToBankAccountID = int.Parse(rcbxToAccount.SelectedValue);
                appPayment.ToAccount = rcbxToAccount.SelectedItem.Text;
                appPayment.Amount = (decimal)(txtRefundAmount.Value ?? 0D);
                appPayment.PayDate = rdpRefundDate.SelectedDate;

                PageAppPaymentRepository.Save();
            }

            if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = (int)EWorkflow.SupplierTaskRefunds;
                appNote.NoteTypeID = (int)EAppNoteType.Comment;
                appNote.WorkflowStepID = (int)EWorkflowStep.EditSupplierTaskRefund;
                appNote.ApplicationID = currentEntity.ID;
                appNote.Note = txtComment.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }

            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
        }

    }
}