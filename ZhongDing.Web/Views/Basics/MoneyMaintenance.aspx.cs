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
using ZhongDing.Common.Extension;

namespace ZhongDing.Web.Views.Basics
{
    public partial class MoneyMaintenance : BasePage
    {
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

        private IApplicationPaymentRepository _PageApplicationPaymentRepository;
        private IApplicationPaymentRepository PageApplicationPaymentRepository
        {
            get
            {
                if (_PageApplicationPaymentRepository == null)
                {
                    _PageApplicationPaymentRepository = new ApplicationPaymentRepository();
                }

                return _PageApplicationPaymentRepository;
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

        private IAccountTypeRepository _PageAccountTypeRepository;
        private IAccountTypeRepository PageAccountTypeRepository
        {
            get
            {
                if (_PageAccountTypeRepository == null)
                {
                    _PageAccountTypeRepository = new AccountTypeRepository();
                }

                return _PageAccountTypeRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.MoneyManage;

            if (!IsPostBack)
            {
                //base.PermissionOptionCheckButtonDelete(btnDelete);
                BindBankAccounts();
            }
        }

        #region Private Methods
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

        #endregion

        #region Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            ApplicationPayment payment = new ApplicationPayment()
            {
                Amount = txtAmount.Value.ToDecimalOrNull(),
                Comment = txtComment.Text.Trim(),
                //FromBankAccountID = rcbxFromAccount.SelectedValue.ToIntOrNull(),
                //FromAccount = rcbxFromAccount.SelectedItem.Text,
                PayDate = DateTime.Now,
                PaymentStatusID = (int)EPaymentStatus.Paid,
                PaymentTypeID = rddPaymentType.SelectedValue.ToInt(),
            };

            if (rddPaymentType.SelectedItem.Text == "收款")
            {
                payment.ToBankAccountID = rcbxFromAccount.SelectedValue.ToIntOrNull();
                payment.ToAccount = rcbxFromAccount.SelectedItem.Text;
            }
            else if (rddPaymentType.SelectedItem.Text == "付款")
            {
                payment.FromBankAccountID = rcbxFromAccount.SelectedValue.ToIntOrNull();
                payment.FromAccount = rcbxFromAccount.SelectedItem.Text;
            }
            else
            {
                payment.FromBankAccountID = rcbxFromAccount.SelectedValue.ToIntOrNull();
                payment.FromAccount = rcbxFromAccount.SelectedItem.Text;

                payment.ToAccount = txtAccountName.Text.Trim() + " " + txtBank.Text.Trim() + " " + txtAccount.Text.Trim();
            }
            PageApplicationPaymentRepository.Add(payment);

            PageApplicationPaymentRepository.Save();

            this.Master.BaseNotification.OnClientHidden = "onClientHidden";
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);


        }



        protected void cvAccount_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string inputAccountNo = txtAccount.Text.Trim();

            if (!string.IsNullOrEmpty(inputAccountNo))
            {
                inputAccountNo = inputAccountNo.Replace("-", "");

                if (!Utility.IsValidAccountNumber(inputAccountNo))
                    args.IsValid = false;
            }
        }
        #endregion




        protected override EPermission PagePermissionID()
        {
            return EPermission.MoneyManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }

        protected void rddPaymentType_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            var value = e.Value.ToIntOrNull();
            if (value.BiggerThanZero())
            {

                if (e.Text == "收款")
                {
                    lblAccount.InnerText = "收款账号";
                    rfvFromAccount.ErrorMessage = "收款账号必填";
                    receiveAccountInfo.Visible = false;
                }
                else if (e.Text == "付款")
                {
                    lblAccount.InnerText = "付款账号";
                    rfvFromAccount.ErrorMessage = "付款账号必填";
                    receiveAccountInfo.Visible = false;
                }
                else
                {
                    lblAccount.InnerText = "付款账号";
                    rfvFromAccount.ErrorMessage = "付款账号必填";
                    receiveAccountInfo.Visible = true;
                }
            }
        }
    }
}