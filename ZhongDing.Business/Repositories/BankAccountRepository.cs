﻿using System;
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

                if (uiSearchObj.OwnerTypeID > 0)
                    whereFuncs.Add(x => x.OwnerTypeID == uiSearchObj.OwnerTypeID);

                if (!string.IsNullOrEmpty(uiSearchObj.AccountName))
                    whereFuncs.Add(x => x.AccountName.Contains(uiSearchObj.AccountName));

                if (!string.IsNullOrEmpty(uiSearchObj.BankBranchName))
                    whereFuncs.Add(x => x.BankBranchName.Contains(uiSearchObj.BankBranchName));

                if (!string.IsNullOrEmpty(uiSearchObj.Account))
                    whereFuncs.Add(x => x.Account.Contains(uiSearchObj.Account));

                if (uiSearchObj.AccountTypeID > 0)
                {
                    switch (uiSearchObj.AccountTypeID)
                    {
                        case (int)EAccountType.Company:
                            if (uiSearchObj.CompanyID > 0)
                                whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);
                            break;

                        case (int)EAccountType.Personal:
                            whereFuncs.Add(x => x.AccountTypeID == uiSearchObj.AccountTypeID);
                            break;
                    }
                }
                else
                {
                    if (uiSearchObj.CompanyID > 0)
                        whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID || x.AccountTypeID == (int)EAccountType.Personal);
                }
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiBankAccounts = (from q in query
                                  //join cu in this.DB.Users on q.CreatedBy equals cu.UserID into tempCU
                                  //from tcu in tempCU.DefaultIfEmpty()
                                  //join mu in this.DB.Users on q.LastModifiedBy equals mu.UserID into tempMU
                                  //from tmu in tempMU.DefaultIfEmpty()
                                  join at in this.DB.AccountType on q.AccountTypeID equals at.ID into tempAT
                                  from tat in tempAT.DefaultIfEmpty()
                                  join ot in this.DB.OwnerType on q.OwnerTypeID equals ot.ID into tempOT
                                  from tot in tempOT.DefaultIfEmpty()
                                  join c in this.DB.Company on q.CompanyID equals c.ID into tempC
                                  from tc in tempC.DefaultIfEmpty()
                                  //orderby q.CreatedOn descending
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
                                      //CreatedOn = q.CreatedOn,
                                      //CreatedBy = tcu == null ? string.Empty : tcu.UserName,
                                      //LastModifiedOn = q.LastModifiedOn,
                                      //LastModifiedBy = tmu == null ? string.Empty : tmu.UserName
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

                if (uiSearchObj.OwnerTypeID > 0)
                    whereFuncs.Add(x => x.OwnerTypeID == uiSearchObj.OwnerTypeID);

                if (!string.IsNullOrEmpty(uiSearchObj.AccountName))
                    whereFuncs.Add(x => x.AccountName.Contains(uiSearchObj.AccountName));

                if (!string.IsNullOrEmpty(uiSearchObj.BankBranchName))
                    whereFuncs.Add(x => x.BankBranchName.Contains(uiSearchObj.BankBranchName));

                if (!string.IsNullOrEmpty(uiSearchObj.Account))
                    whereFuncs.Add(x => x.Account.Contains(uiSearchObj.Account));

                if (uiSearchObj.AccountTypeID > 0)
                {
                    switch (uiSearchObj.AccountTypeID)
                    {
                        case (int)EAccountType.Company:
                            if (uiSearchObj.CompanyID > 0)
                                whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);
                            break;

                        case (int)EAccountType.Personal:
                            whereFuncs.Add(x => x.AccountTypeID == uiSearchObj.AccountTypeID);
                            break;
                    }
                }
                else
                {
                    if (uiSearchObj.CompanyID > 0)
                        whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID || x.AccountTypeID == (int)EAccountType.Personal);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiBankAccounts = (from q in query
                                  //join cu in this.DB.Users on q.CreatedBy equals cu.UserID into tempCU
                                  //from tcu in tempCU.DefaultIfEmpty()
                                  //join mu in this.DB.Users on q.LastModifiedBy equals mu.UserID into tempMU
                                  //from tmu in tempMU.DefaultIfEmpty()
                                  join at in this.DB.AccountType on q.AccountTypeID equals at.ID into tempAT
                                  from tat in tempAT.DefaultIfEmpty()
                                  join ot in this.DB.OwnerType on q.OwnerTypeID equals ot.ID into tempOT
                                  from tot in tempOT.DefaultIfEmpty()
                                  join c in this.DB.Company on q.CompanyID equals c.ID into tempC
                                  from tc in tempC.DefaultIfEmpty()
                                  //orderby q.CreatedOn descending
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
                                      //CreatedOn = q.CreatedOn,
                                      //CreatedBy = tcu == null ? string.Empty : tcu.UserName,
                                      //LastModifiedOn = q.LastModifiedOn,
                                      //LastModifiedBy = tmu == null ? string.Empty : tmu.UserName
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

        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<BankAccount, bool>>> whereFuncs = new List<Expression<Func<BankAccount, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (uiSearchObj.ExcludeItemValues != null
                    && uiSearchObj.ExcludeItemValues.Count > 0)
                    whereFuncs.Add(x => !uiSearchObj.ExcludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => (x.AccountName + x.BankBranchName + x.Account).Contains(uiSearchObj.ItemText));

                if (uiSearchObj.Extension != null)
                {
                    if (uiSearchObj.Extension.OwnerTypeID > 0)
                        whereFuncs.Add(x => x.OwnerTypeID == uiSearchObj.Extension.OwnerTypeID);

                    if (uiSearchObj.Extension.AccountTypeID > 0)
                    {
                        switch (uiSearchObj.Extension.AccountTypeID)
                        {
                            case (int)EAccountType.Company:
                                if (uiSearchObj.Extension.CompanyID > 0)
                                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.Extension.CompanyID);
                                break;

                            case (int)EAccountType.Personal:
                                whereFuncs.Add(x => x.AccountTypeID == uiSearchObj.Extension.AccountTypeID);
                                break;
                        }
                    }
                    else
                    {
                        if (uiSearchObj.Extension.CompanyID > 0)
                            whereFuncs.Add(x => x.CompanyID == uiSearchObj.Extension.CompanyID || x.AccountTypeID == (int)EAccountType.Personal);
                    }
                }
            }

            var query = GetList(whereFuncs).ToList();

            uiDropdownItems = query.Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.AccountName + " " + x.BankBranchName + " " + Utility.FormatAccountNumber(x.Account)
            }).ToList();

            return uiDropdownItems;
        }

        /// <summary>
        /// 年月
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public decimal GetMoneyBalanceAll(DateTime month, EAccountType? accountType = null)
        {
            IApplicationPaymentRepository applicationPaymentRepository = new ApplicationPaymentRepository();
            applicationPaymentRepository.SetDbModel(DB);

            var query = GetList(x => x.OwnerTypeID == (int)EOwnerType.Company);
            if (accountType.HasValue)
            {
                query = query.Where(x => x.AccountTypeID == (int)accountType);
            }
            var bankAccountIDs = query.Select(x => x.ID).ToList();
            decimal balance = 0;
            bankAccountIDs.ForEach(bankAccountID =>
            {
                decimal tempBalance = applicationPaymentRepository.GetRealTimeBalance(bankAccountID, month);
                balance += tempBalance;
            });
            return balance;
        }




    }
}
