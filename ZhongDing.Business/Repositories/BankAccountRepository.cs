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
                uiBankAccounts = (from q in query
                                  join cu in this.DB.Users on q.CreatedBy equals cu.UserID into tempCU
                                  from tcu in tempCU.DefaultIfEmpty()
                                  join mu in this.DB.Users on q.LastModifiedBy equals mu.UserID into tempMU
                                  from tmu in tempMU.DefaultIfEmpty()
                                  join at in this.DB.AccountType on q.AccountTypeID equals at.ID into tempAT
                                  from tat in tempAT.DefaultIfEmpty()
                                  join ot in this.DB.OwnerType on q.OwnerTypeID equals ot.ID into tempOT
                                  from tot in tempOT.DefaultIfEmpty()
                                  join c in this.DB.Company on q.CompanyID equals c.ID into tempC
                                  from tc in tempC.DefaultIfEmpty()
                                  orderby q.CreatedOn descending
                                  select new UIBankAccount()
                                  {
                                      ID = q.ID,
                                      AccountName = q.AccountName,
                                      BankBranchName = q.BankBranchName,
                                      Account = q.Account,
                                      AccountType = tat == null ? string.Empty : tat.AccountTypeName,
                                      OwnerType = tot == null ? string.Empty : tot.OwnerTypeName,
                                      CompanyName = tc == null ? string.Empty : tc.CompanyName,
                                      Comment = q.Comment,
                                      CreatedOn = q.CreatedOn,
                                      CreatedBy = tcu == null ? string.Empty : tcu.UserName,
                                      LastModifiedOn = q.LastModifiedOn,
                                      LastModifiedBy = tmu == null ? string.Empty : tmu.UserName
                                  }).ToList();

                if (uiSearchObj != null
                    && uiSearchObj.IsNeedMaskedAccount)
                {
                    foreach (var bankAccount in uiBankAccounts)
                    {
                        bankAccount.Account = Utility.MaskAccount(bankAccount.Account);
                    }
                }
            }

            return uiBankAccounts;
        }

        public IList<UIBankAccount> GetUIList(UISearchBankAccount uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
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
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiBankAccounts = (from q in query
                                  join cu in this.DB.Users on q.CreatedBy equals cu.UserID into tempCU
                                  from tcu in tempCU.DefaultIfEmpty()
                                  join mu in this.DB.Users on q.LastModifiedBy equals mu.UserID into tempMU
                                  from tmu in tempMU.DefaultIfEmpty()
                                  join at in this.DB.AccountType on q.AccountTypeID equals at.ID into tempAT
                                  from tat in tempAT.DefaultIfEmpty()
                                  join ot in this.DB.OwnerType on q.OwnerTypeID equals ot.ID into tempOT
                                  from tot in tempOT.DefaultIfEmpty()
                                  join c in this.DB.Company on q.CompanyID equals c.ID into tempC
                                  from tc in tempC.DefaultIfEmpty()
                                  orderby q.CreatedOn descending
                                  select new UIBankAccount()
                                  {
                                      ID = q.ID,
                                      AccountName = q.AccountName,
                                      BankBranchName = q.BankBranchName,
                                      Account = q.Account,
                                      AccountType = tat == null ? string.Empty : tat.AccountTypeName,
                                      OwnerType = tot == null ? string.Empty : tot.OwnerTypeName,
                                      CompanyName = tc == null ? string.Empty : tc.CompanyName,
                                      Comment = q.Comment,
                                      CreatedOn = q.CreatedOn,
                                      CreatedBy = tcu == null ? string.Empty : tcu.UserName,
                                      LastModifiedOn = q.LastModifiedOn,
                                      LastModifiedBy = tmu == null ? string.Empty : tmu.UserName
                                  }).ToList();

                if (uiSearchObj != null
                    && uiSearchObj.IsNeedMaskedAccount)
                {
                    foreach (var bankAccount in uiBankAccounts)
                    {
                        bankAccount.Account = Utility.MaskAccount(bankAccount.Account);
                    }
                }
            }

            totalRecords = total;

            return uiBankAccounts;
        }
    }
}
