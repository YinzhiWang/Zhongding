using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;

namespace ZhongDing.Web.Views.Basics.Editors
{
    public partial class SupplierBankAccountMaintain : BasePage
    {
        #region Members

        protected int? SupplierID
        {
            get
            {
                string sSupplierID = Request.QueryString["SupplierID"];

                int iSupplierID;

                if (int.TryParse(sSupplierID, out iSupplierID))
                    return iSupplierID;
                else
                    return null;
            }
        }

        private ISupplierRepository _PageSupplierRepository;
        private ISupplierRepository PageSupplierRepository
        {
            get
            {
                if (_PageSupplierRepository == null)
                {
                    _PageSupplierRepository = new SupplierRepository();
                }

                return _PageSupplierRepository;
            }
        }

        private ISupplierBankAccountRepository _PageSupplierBankAccountRepository;
        private ISupplierBankAccountRepository PageSupplierBankAccountRepository
        {
            get
            {
                if (_PageSupplierBankAccountRepository == null)
                {
                    _PageSupplierBankAccountRepository = new SupplierBankAccountRepository();
                }

                return _PageSupplierBankAccountRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.SupplierID.HasValue
                || this.SupplierID <= 0)
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

                return;
            }

            if (!IsPostBack)
            {
                hdnGridClientID.Value = GridClientID;

                LoadBankAccount();
            }
        }


        private void LoadBankAccount()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var supplierBankAccount = PageSupplierBankAccountRepository.GetByID(this.CurrentEntityID);

                if (supplierBankAccount != null)
                {
                    var bankAccount = supplierBankAccount.BankAccount;

                    if (bankAccount != null)
                    {
                        txtAccountName.Text = bankAccount.AccountName;
                        txtBankBranchName.Text = bankAccount.BankBranchName;
                        txtAccount.Text = bankAccount.Account;
                        txtComment.Text = bankAccount.Comment;
                    }
                }
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            var supplier = PageSupplierRepository.GetByID(this.SupplierID);

            if (supplier == null)
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show("无效的供应商，窗口将自动关闭");

                return;
            }

            BankAccount bankAccount = null;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                bankAccount = supplier.SupplierBankAccount.Where(x => x.ID == this.CurrentEntityID)
                    .Select(x => x.BankAccount).FirstOrDefault();
            else
            {
                bankAccount = new BankAccount()
                {
                    CompanyID = CurrentUser.CompanyID,
                    OwnerTypeID = (int)EOwnerType.Supplier
                };

                SupplierBankAccount supplierBankAccount = new SupplierBankAccount();

                supplierBankAccount.BankAccount = bankAccount;

                supplier.SupplierBankAccount.Add(supplierBankAccount);
            }

            if (bankAccount != null)
            {
                bankAccount.AccountName = txtAccountName.Text.Trim();
                bankAccount.BankBranchName = txtBankBranchName.Text.Trim();
                bankAccount.Account = txtAccount.Text.Trim();
                bankAccount.AccountTypeID = (int)EAccountType.Company;
                bankAccount.Comment = txtComment.Text.Trim();
            }

            supplier.LastModifiedOn = DateTime.Now;
            supplier.LastModifiedBy = CurrentUser.UserID;

            PageSupplierRepository.Save();

            this.Master.BaseNotification.OnClientHidden = "onClientHidden";
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
        }

        protected void cvAccount_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string inputAccountNo = txtAccount.Text.Trim();

            if (!string.IsNullOrEmpty(inputAccountNo))
            {
                if (!Utility.IsValidAccountNumber(inputAccountNo))
                    args.IsValid = false;
            }
        }



        protected override EPermission PagePermissionID()
        {
            return EPermission.SupplierManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    
    }
}