using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class SupplierBankAccountRepository : BaseRepository<SupplierBankAccount>, ISupplierBankAccountRepository
    {
        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();


            var query = from supplierBankAccount in DB.SupplierBankAccount
                        join bankAccount in DB.BankAccount on supplierBankAccount.BankAccountID equals bankAccount.ID
                        select new
              {
                  ItemValue = bankAccount.ID,
                  SupplierID = supplierBankAccount.SupplierID,
                  AccountName = bankAccount.AccountName,
                  BankBranchName = bankAccount.BankBranchName,
                  Account = bankAccount.Account
                  // ItemText = bankAccount.AccountName + " " + bankAccount.BankBranchName + " " + Utility.FormatAccountNumber(bankAccount.Account)
              };


            if (uiSearchObj != null)
            {

                if (uiSearchObj.Extension != null)
                {
                    if (uiSearchObj.Extension.SupplierID > 0)
                    {
                        query = query.Where(x => x.SupplierID == uiSearchObj.Extension.SupplierID);
                    }

                }
            }
            uiDropdownItems = query.ToList().Select(x => new UIDropdownItem()
            {
                ItemValue = x.ItemValue,
                ItemText = x.AccountName + " " + x.BankBranchName + " " + Utility.FormatAccountNumber(x.Account)
            }).ToList();
            return uiDropdownItems;
        }
    }
}
