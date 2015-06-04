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
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.Sales.Editors
{
    public partial class ClientOrderPaymentMaintain : WorkflowBasePage
    {
        #region Fields

        /// <summary>
        /// 所属的实体ID
        /// </summary>
        private int? OwnerEntityID
        {
            get
            {
                return WebUtility.GetIntFromQueryString("OwnerEntityID");
            }
        }

        /// <summary>
        /// 收款方式ID
        /// </summary>
        private int? PaymentMethodID
        {
            get
            {
                return WebUtility.GetIntFromQueryString("PaymentMethodID");
            }
        }

        #endregion

        #region Members

        private IClientSaleApplicationRepository _PageClientSaleAppRepository;
        private IClientSaleApplicationRepository PageClientSaleAppRepository
        {
            get
            {
                if (_PageClientSaleAppRepository == null)
                    _PageClientSaleAppRepository = new ClientSaleApplicationRepository();

                return _PageClientSaleAppRepository;
            }
        }

        private ClientSaleApplication _CurrentOwnerEntity;
        private ClientSaleApplication CurrentOwnerEntity
        {
            get
            {
                if (_CurrentOwnerEntity == null)
                    if (this.OwnerEntityID.HasValue && this.OwnerEntityID > 0)
                        _CurrentOwnerEntity = PageClientSaleAppRepository.GetByID(this.OwnerEntityID);

                return _CurrentOwnerEntity;
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

        private IClientInfoRepository _PageClientInfoRepository;
        private IClientInfoRepository PageClientInfoRepository
        {
            get
            {
                if (_PageClientInfoRepository == null)
                    _PageClientInfoRepository = new ClientInfoRepository();

                return _PageClientInfoRepository;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditClientOrder);

                return _CanEditUserIDs;
            }
        }

        private IList<int> _CanAuditUserIDs;
        private IList<int> CanAuditUserIDs
        {
            get
            {
                if (_CanAuditUserIDs == null)
                    _CanAuditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.AuditClientOrder);

                return _CanAuditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.ClientOrder;
        }
        protected override EWorkflow PagePermissionWorkflowID()
        {
            return EWorkflow.ClientOrder;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentOwnerEntity == null
                || CurrentOwnerEntity.DeliveryModeID == (int)EDeliveryMode.GuaranteeDelivery)
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

                return;
            }

            if (!IsPostBack)
            {
                hdnGridClientID.Value = GridClientID;

                rdpPayDate.SelectedDate = DateTime.Now;

                BindClientBankAccounts();

                BindClientSalesBankAccounts();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindClientBankAccounts()
        {
            if (CurrentOwnerEntity != null)
            {
                var clientInfo = PageClientInfoRepository.GetByConditions(CurrentOwnerEntity.ClientUserID, CurrentOwnerEntity.ClientCompanyID);

                if (clientInfo != null)
                {
                    var bankAccountIDs = clientInfo.ClientInfoBankAccount
                        .Where(x => x.IsDeleted == false)
                        .Select(x => x.BankAccountID.HasValue ? x.BankAccountID.Value : GlobalConst.INVALID_INT)
                        .ToList();

                    if (bankAccountIDs.Count == 0)
                        bankAccountIDs.Add(GlobalConst.INVALID_INT);

                    var uiSearchObj = new UISearchDropdownItem
                    {
                        IncludeItemValues = bankAccountIDs
                    };

                    var bankAccounts = PageBankAccountRepository.GetDropdownItems(uiSearchObj);

                    rcbxFromAccount.DataSource = bankAccounts;
                    rcbxFromAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    rcbxFromAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    rcbxFromAccount.DataBind();

                    rcbxFromAccount.Items.Insert(0, new RadComboBoxItem("", ""));
                }
            }
        }

        private void BindClientSalesBankAccounts()
        {
            if (CurrentOwnerEntity != null)
            {
                var bankAccountIDs = CurrentOwnerEntity.ClientSaleAppBankAccount
                    .Where(x => x.IsDeleted == false)
                    .Select(x => x.ReceiverBankAccountID).ToList();

                if (bankAccountIDs.Count == 0)
                    bankAccountIDs.Add(GlobalConst.INVALID_INT);

                var uiSearchObj = new UISearchDropdownItem
                {
                    IncludeItemValues = bankAccountIDs
                };

                var bankAccounts = PageBankAccountRepository.GetDropdownItems(uiSearchObj);

                rcbxToAccount.DataSource = bankAccounts;
                rcbxToAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                rcbxToAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                rcbxToAccount.DataBind();

                rcbxToAccount.Items.Insert(0, new RadComboBoxItem("", ""));
            }
        }

        private void LoadCurrentEntity()
        {
            if (CurrentOwnerEntity == null) return;

            if (CanEditUserIDs.Contains(CurrentUser.UserID))
            {
                btnSave.Enabled = true;
            }
            else
            {
                var eWorkflowStatus = (EWorkflowStatus)CurrentOwnerEntity.WorkflowStatusID;

                switch (eWorkflowStatus)
                {
                    case EWorkflowStatus.Submit:
                        if (CanAuditUserIDs.Contains(CurrentUser.UserID))
                            btnSave.Enabled = true;

                        break;

                    default:
                        DisabledBasicInfoControls();
                        break;
                }
            }

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                if (this.PaymentMethodID == (int)EPaymentMethod.BankTransfer)
                {
                    radioIsBankTransfer.Checked = true;

                    var appPayment = PageAppPaymentRepository.GetByID(this.CurrentEntityID);

                    if (appPayment != null)
                    {
                        rcbxFromAccount.SelectedValue = appPayment.FromBankAccountID.ToString();
                        rcbxToAccount.SelectedValue = appPayment.ToBankAccountID.ToString();
                        rdpPayDate.SelectedDate = appPayment.PayDate;
                        txtAmount.DbValue = appPayment.Amount;
                        txtFee.DbValue = appPayment.Fee;
                        txtComment.Text = appPayment.Comment;
                    }
                }
                else if (this.PaymentMethodID == (int)EPaymentMethod.Deduction)
                {
                    radioIsDeduction.Checked = true;
                }
            }
        }

        /// <summary>
        /// 禁用相关控件
        /// </summary>
        private void DisabledBasicInfoControls()
        {
            rcbxFromAccount.Enabled = false;
            rcbxToAccount.Enabled = false;
            rdpPayDate.Enabled = false;
            txtAmount.Enabled = false;

            btnSave.Enabled = false;
        }

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (radioIsBankTransfer.Checked)
            {
                if (string.IsNullOrEmpty(rcbxFromAccount.SelectedValue))
                    cvFromAccount.IsValid = false;

                if (string.IsNullOrEmpty(rcbxToAccount.SelectedValue))
                    cvToAccount.IsValid = false;

                if (!rdpPayDate.SelectedDate.HasValue)
                    cvPayDate.IsValid = false;
            }
            else if (radioIsDeduction.Checked)
            {
                if (txtAmount.Value > txtBalanceAmount.Value)
                {
                    cvAmount.IsValid = false;
                    cvAmount.ErrorMessage = "抵扣金额不能大于客户余额";
                }
            }

            if (!IsValid) return;

            if (this.CurrentOwnerEntity != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();
                    //需要补充删除抵扣数据的类定义

                    appPaymentRepository.SetDbModel(db);

                    if (radioIsBankTransfer.Checked)
                    {
                        //删除抵扣的数据
                        if (this.PaymentMethodID == (int)EPaymentMethod.Deduction)
                        {

                        }

                        var appPayment = appPaymentRepository.GetByID(this.CurrentEntityID);

                        if (appPayment == null)
                        {
                            appPayment = new ApplicationPayment()
                            {
                                WorkflowID = CurrentWorkFlowID,
                                ApplicationID = CurrentOwnerEntity.ID,
                                PaymentTypeID = (int)EPaymentType.Income,
                                PaymentStatusID = (int)EPaymentStatus.ToBePaid
                            };

                            appPaymentRepository.Add(appPayment);
                        }

                        appPayment.FromBankAccountID = Convert.ToInt32(rcbxFromAccount.SelectedValue);
                        appPayment.FromAccount = rcbxFromAccount.SelectedItem.Text;
                        appPayment.ToBankAccountID = Convert.ToInt32(rcbxToAccount.SelectedValue);
                        appPayment.ToAccount = rcbxToAccount.SelectedItem.Text;
                        appPayment.Amount = (decimal?)txtAmount.Value;
                        appPayment.Fee = (decimal?)txtFee.Value;
                        appPayment.PayDate = rdpPayDate.SelectedDate.HasValue
                            ? rdpPayDate.SelectedDate.Value : DateTime.Now;
                        appPayment.Comment = txtComment.Text.Trim();

                    }
                    else if (radioIsDeduction.Checked)
                    {
                        //删除之前保存的收款信息
                        if (this.PaymentMethodID == (int)EPaymentMethod.BankTransfer)
                        {
                            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                            {
                                appPaymentRepository.DeleteByID(this.CurrentEntityID);
                            }

                        }

                        //保存抵扣的数据
                    }

                    unitOfWork.SaveChanges();

                    this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
                }
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);
            }
        }
    }
}