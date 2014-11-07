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

namespace ZhongDing.Web.Views.Basics
{
    public partial class BankAccountMaintenance : BasePage
    {
        #region Members

        public int? BankAccountID
        {
            get
            {
                string sBankAccountID = Request.QueryString["BankAccountID"];

                int iBankAccountID;

                if (int.TryParse(sBankAccountID, out iBankAccountID))
                    return iBankAccountID;
                else
                    return null;
            }
        }

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
            this.Master.MenuItemID = 4;

            if (!IsPostBack)
            {
                BindAccountTypes();

                LoadBankAccount();
            }
        }

        #region Private Methods

        private void BindAccountTypes()
        {
            var accountTypes = PageAccountTypeRepository.GetUIList();

            ddlAccountType.DataSource = accountTypes;
            ddlAccountType.DataTextField = "AccountTypeName";
            ddlAccountType.DataValueField = "ID";
            ddlAccountType.DataBind();
        }

        private void LoadBankAccount()
        {
            if (this.BankAccountID.HasValue
                && this.BankAccountID > 0)
            {
                var bankAccount = PageBankAccountRepository.GetByID(this.BankAccountID);

                if (bankAccount != null)
                {
                    txtAccountName.Text = bankAccount.AccountName;
                    txtBankBranchName.Text = bankAccount.BankBranchName;
                    txtAccount.Text = bankAccount.Account;

                    if (bankAccount.AccountTypeID.HasValue && bankAccount.AccountTypeID > 0)
                        ddlAccountType.SelectedValue = bankAccount.AccountTypeID.ToString();

                    txtComment.Text = bankAccount.Comment;
                }
            }
            else
                btnDelete.Visible = false;
        }

        #endregion

        #region Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            BankAccount bankAccount = null;

            if (this.BankAccountID.HasValue
                && this.BankAccountID > 0)
                bankAccount = PageBankAccountRepository.GetByID(this.BankAccountID);
            else
            {
                bankAccount = new BankAccount()
                {
                    OwnerTypeID = (int)EOwnerType.Company
                };

                PageBankAccountRepository.Add(bankAccount);
            }

            if (bankAccount != null)
            {
                bankAccount.AccountName = txtAccountName.Text.Trim();
                bankAccount.BankBranchName = txtBankBranchName.Text.Trim();
                bankAccount.Account = txtAccount.Text.Trim();

                if (!string.IsNullOrEmpty(ddlAccountType.SelectedValue))
                    bankAccount.AccountTypeID = Convert.ToInt32(ddlAccountType.SelectedValue);

                if (bankAccount.AccountTypeID == (int)EAccountType.Company)
                    bankAccount.CompanyID = CurrentUser.CompanyID;
                else
                    bankAccount.CompanyID = null;

                bankAccount.Comment = txtComment.Text.Trim();

                PageBankAccountRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show("保存成功，页面将自动跳转");
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.BankAccountID.HasValue
                   && this.BankAccountID > 0)
            {
                PageBankAccountRepository.DeleteByID(this.BankAccountID);
                PageBankAccountRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show("删除成功，页面将自动跳转");
            }
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

    }
}