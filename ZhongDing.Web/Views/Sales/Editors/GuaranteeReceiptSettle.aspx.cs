using System;
using System.Collections;
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
using ZhongDing.Common.Extension;
using System.Text;

namespace ZhongDing.Web.Views.Sales.Editors
{
    public partial class GuaranteeReceiptSettle : WorkflowBasePage
    {
        #region Fields

        /// <summary>
        /// 所属的实体ID
        /// </summary>
        /// <value>The owner entity ID.</value>
        private int? OwnerEntityID
        {
            get
            {
                return WebUtility.GetIntFromQueryString("OwnerEntityID");
            }
        }



        #endregion

        #region Members
        private IBankAccountRepository _PageBankAccountRepository;
        private IBankAccountRepository PageBankAccountRepository
        {
            get
            {
                if (_PageBankAccountRepository == null)
                {
                    _PageBankAccountRepository = new BankAccountRepository();
                }

                return _PageBankAccountRepository;
            }
        }

        private IGuaranteeReceiptRepository _PageGuaranteeReceiptRepository;
        private IGuaranteeReceiptRepository PageGuaranteeReceiptRepository
        {
            get
            {
                if (_PageGuaranteeReceiptRepository == null)
                    _PageGuaranteeReceiptRepository = new GuaranteeReceiptRepository();

                return _PageGuaranteeReceiptRepository;
            }
        }
        private IClientSaleApplicationRepository _PageClientSaleApplicationRepository;
        private IClientSaleApplicationRepository PageClientSaleApplicationRepository
        {
            get
            {
                if (_PageClientSaleApplicationRepository == null)
                    _PageClientSaleApplicationRepository = new ClientSaleApplicationRepository();

                return _PageClientSaleApplicationRepository;
            }
        }






        #endregion

        private void BindBankAccounts()
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
            rcbxFromAccount.DataSource = bankAccounts;
            rcbxFromAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxFromAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxFromAccount.DataBind();

        }
        public List<int> GetSelectedIDs
        {
            get
            {
                return (List<int>)Session[GlobalConst.SessionKeys.SELECTED_ClientSaleApplication_IDS];
            }
        }
        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.GuaranteeReceiptManagement;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (GetSelectedIDs == null || GetSelectedIDs.Count == 0)
                {
                    ShowErrorMessage("请选择要结算的订单");
                    return;
                }

                hdnGridClientID.Value = base.GridClientID;
                BindBankAccounts();
                decimal amount = PageClientSaleApplicationRepository.GetTotalAmount(GetSelectedIDs);
                txtReceiptAmount.Value = amount.ToDouble();

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            decimal amount = PageClientSaleApplicationRepository.GetTotalAmount(GetSelectedIDs);
            if (txtReceiptAmount.Value.ToDecimal() != amount)
            {
                rfvReceiptAmount.ErrorMessage = "收款金额错误";
                rfvReceiptAmount.IsValid = false;
            }

            if (!IsValid) return;

            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                DbModelContainer db = unitOfWork.GetDbModel();

                IApplicationPaymentRepository applicationPaymentRepository = new ApplicationPaymentRepository();
                IGuaranteeReceiptRepository guaranteeReceiptRepository = new GuaranteeReceiptRepository();

                applicationPaymentRepository.SetDbModel(db);
                guaranteeReceiptRepository.SetDbModel(db);


                ApplicationPayment payment = new ApplicationPayment()
                {
                    Amount = txtReceiptAmount.Value.ToDecimal(),
                    //Comment = txtComment.Text.Trim(),
                    ToBankAccountID = rcbxFromAccount.SelectedValue.ToIntOrNull(),
                    ToAccount = rcbxFromAccount.SelectedItem.Text,
                    PayDate = DateTime.Now,
                    PaymentStatusID = (int)EPaymentStatus.Paid,
                    PaymentTypeID = (int)EPaymentType.Income,
                    WorkflowID = CurrentWorkFlowID,
                    
                };

                payment.ToBankAccountID = rcbxFromAccount.SelectedValue.ToIntOrNull();
                payment.ToAccount = rcbxFromAccount.SelectedItem.Text;
                applicationPaymentRepository.Add(payment);


                GuaranteeReceipt guaranteeReceipt = new GuaranteeReceipt()
                {
                    ApplicationPayment = payment,
                    ReceiptAmount = txtReceiptAmount.Value.ToDecimal(),
                    ReceiptDate = rdpBorrowDate.SelectedDate.Value,
                };
                guaranteeReceipt.GuaranteeLog = new List<GuaranteeLog>();
                foreach (int id in GetSelectedIDs)
                {
                    var clientSaleApplicationRepository = PageClientSaleApplicationRepository.GetByID(id);
                    var oneAmount = clientSaleApplicationRepository.SalesOrderApplication.SalesOrderAppDetail.Where(x => x.IsDeleted == false).Any() ?
                        clientSaleApplicationRepository.SalesOrderApplication.SalesOrderAppDetail.Where(x => x.IsDeleted == false).Sum(x => x.TotalSalesAmount) : 0M;
                    guaranteeReceipt.GuaranteeLog.Add(new GuaranteeLog()
                    {
                        ClientSaleApplicationID = clientSaleApplicationRepository.ID,
                        GuaranteeAmount = oneAmount,
                        Guaranteeby = clientSaleApplicationRepository.Guaranteeby,
                        GuaranteeExpirationDate = clientSaleApplicationRepository.GuaranteeExpirationDate,
                        IsReceipted = true,
                        GuaranteeReceiptDate = guaranteeReceipt.ReceiptDate,
                    });
                }
                guaranteeReceiptRepository.Add(guaranteeReceipt);
                unitOfWork.SaveChanges();
                payment.ApplicationID = guaranteeReceipt.ID;
                unitOfWork.SaveChanges();
                Session[GlobalConst.SessionKeys.SELECTED_ClientSaleApplication_IDS] = null;
            }
            this.Master.BaseNotification.OnClientHidden = "onClientHidden";
            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_CLOSE_WIN);

        }
    }
}