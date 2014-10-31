using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class BankAccountRepository : BaseRepository<BankAccount>, IBankAccountRepository
    {

        public IList<UIBankAccount> GetUIList(UISearchBankAccount uiSearchObj = null)
        {
            IList<UIBankAccount> uiBankAccounts = new List<UIBankAccount>();

            IQueryable<BankAccount> query = null;

            List<Expression<Func<BankAccount, bool>>> whereFuncs = new List<Expression<Func<BankAccount, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.AccountName))
                    whereFuncs.Add(x => x.AccountName.Contains(uiSearchObj.AccountName));

                if (!string.IsNullOrEmpty(uiSearchObj.BankBranchName))
                    whereFuncs.Add(x => x.BankBranchName.Contains(uiSearchObj.BankBranchName));

                if (!string.IsNullOrEmpty(uiSearchObj.Account))
                    whereFuncs.Add(x => x.Account.Contains(uiSearchObj.Account));

                if (uiSearchObj.AccountTypeID > 0)
                    whereFuncs.Add(x => x.AccountTypeID == uiSearchObj.AccountTypeID);

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiBankAccounts = query.ToList().Select(x => new UIBankAccount()
                {
                    ID = x.ID,
                    AccountName = x.AccountName,
                    BankBranchName = x.BankBranchName,
                    Account = uiSearchObj.IsNeedMaskedAccount ? Utility.MaskAccount(x.Account) : x.Account,
                    AccountType = x.AccountType == null ? string.Empty : x.AccountType.AccountTypeName,
                    OwnerType = x.OwnerType == null ? string.Empty : x.OwnerType.OwnerTypeName,
                    CompanyName = x.Company == null ? string.Empty : x.Company.CompanyName,
                    Comments = x.Comments,
                    CreatedOn = x.CreatedOn,
                    CreatedBy = x.CreatedBy.HasValue ? UsersRepository.Instance.GetUserNameByID(x.CreatedBy.Value) : string.Empty,
                    LastModifiedOn = x.LastModifiedOn,
                    LastModifiedBy = x.LastModifiedBy.HasValue ? UsersRepository.Instance.GetUserNameByID(x.LastModifiedBy.Value) : string.Empty,
                }).ToList();
            }

            return uiBankAccounts;
        }

        public IList<UIBankAccount> GetUIList(UISearchBankAccount uiSearchObj, out int totalRecords)
        {
            IList<UIBankAccount> uiBankAccounts = new List<UIBankAccount>();

            int total = 0;

            IQueryable<BankAccount> query = null;

            List<Expression<Func<BankAccount, bool>>> whereFuncs = new List<Expression<Func<BankAccount, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.AccountName))
                    whereFuncs.Add(x => x.AccountName.Contains(uiSearchObj.AccountName));

                if (!string.IsNullOrEmpty(uiSearchObj.BankBranchName))
                    whereFuncs.Add(x => x.BankBranchName.Contains(uiSearchObj.BankBranchName));

                if (!string.IsNullOrEmpty(uiSearchObj.Account))
                    whereFuncs.Add(x => x.Account.Contains(uiSearchObj.Account));

                if (uiSearchObj.AccountTypeID > 0)
                    whereFuncs.Add(x => x.AccountTypeID == uiSearchObj.AccountTypeID);

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.IsNeedPaging)
                    query = GetList(uiSearchObj.PageIndex, uiSearchObj.PageSize, whereFuncs, out total);
                else
                    query = GetList(whereFuncs);
            }
            else
                query = GetList(whereFuncs);

            if (query != null)
            {
                uiBankAccounts = query.ToList().Select(x => new UIBankAccount()
                {
                    ID = x.ID,
                    AccountName = x.AccountName,
                    BankBranchName = x.BankBranchName,
                    Account = uiSearchObj.IsNeedMaskedAccount ? Utility.MaskAccount(x.Account) : x.Account,
                    AccountType = x.AccountType == null ? string.Empty : x.AccountType.AccountTypeName,
                    OwnerType = x.OwnerType == null ? string.Empty : x.OwnerType.OwnerTypeName,
                    CompanyName = x.Company == null ? string.Empty : x.Company.CompanyName,
                    Comments = x.Comments,
                    CreatedOn = x.CreatedOn,
                    CreatedBy = x.CreatedBy.HasValue ? UsersRepository.Instance.GetUserNameByID(x.CreatedBy.Value) : string.Empty,
                    LastModifiedOn = x.LastModifiedOn,
                    LastModifiedBy = x.LastModifiedBy.HasValue ? UsersRepository.Instance.GetUserNameByID(x.LastModifiedBy.Value) : string.Empty,
                }).ToList();
            }

            totalRecords = total;

            return uiBankAccounts;
        }
    }
}
