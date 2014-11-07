using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Web.WebServices
{
    /// <summary>
    /// Summary description for SupplierWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class SupplierWS : System.Web.Services.WebService
    {
        #region Members

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

        #endregion

        IList<UISupplierBankAccount> bankAccountList = null;

        public SupplierWS()
        {
        }

        [WebMethod(EnableSession = true)]
        public IList<UISupplierBankAccount> GetBankAccounts(int supplierID)
        {
            if (HttpContext.Current.Session["SupplierBankAccounts"] != null)
                return (List<UISupplierBankAccount>)HttpContext.Current.Session["SupplierBankAccounts"];

            bankAccountList = PageSupplierRepository.GetBankAccounts(supplierID);

            if (HttpContext.Current.Session["SupplierBankAccounts"] == null)
                HttpContext.Current.Session["SupplierBankAccounts"] = bankAccountList;

            return bankAccountList;
        }

        [WebMethod(EnableSession = true)]
        public IList<UISupplierBankAccount> UpdateBankAccount(UISupplierBankAccount uiBankAccount)
        {
            if (HttpContext.Current.Session["SupplierBankAccounts"] != null)
            {
                IList<UISupplierBankAccount> bankAccountList = (List<UISupplierBankAccount>)HttpContext.Current.Session["SupplierBankAccounts"];

                if (uiBankAccount != null)
                {
                    var bankAccountToUpdate = bankAccountList.Where(x => x.ID == uiBankAccount.ID).FirstOrDefault();

                    if (bankAccountToUpdate == null)
                    {
                        if (uiBankAccount.ID <= 0)
                        {
                            bankAccountToUpdate = bankAccountList.Where(x => x.ID <= 0 && x.AccountName == uiBankAccount.AccountName.Trim()
                                && x.BankBranchName == uiBankAccount.BankBranchName.Trim()
                                && x.Account == uiBankAccount.Account.Trim()).FirstOrDefault();
                        }

                        if (bankAccountToUpdate == null)
                        {
                            bankAccountToUpdate = new UISupplierBankAccount();
                            bankAccountToUpdate.SupplierID = uiBankAccount.SupplierID;

                            bankAccountList.Add(bankAccountToUpdate);
                        }
                    }

                    bankAccountToUpdate.AccountName = uiBankAccount.AccountName;
                    bankAccountToUpdate.BankBranchName = uiBankAccount.BankBranchName;
                    bankAccountToUpdate.Account = uiBankAccount.Account;
                    bankAccountToUpdate.Comment = uiBankAccount.Comment;
                    bankAccountToUpdate.CreatedOn = DateTime.Now;
                    bankAccountToUpdate.CreatedBy = "System Admin";

                    HttpContext.Current.Session["SupplierBankAccounts"] = bankAccountList;
                }

                return bankAccountList;
            }
            else
                return new List<UISupplierBankAccount>();
        }
    }
}
