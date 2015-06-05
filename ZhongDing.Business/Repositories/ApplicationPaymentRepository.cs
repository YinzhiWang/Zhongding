using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Common.Extension;

namespace ZhongDing.Business.Repositories
{
    public class ApplicationPaymentRepository : BaseRepository<ApplicationPayment>, IApplicationPaymentRepository
    {
        public IList<UIApplicationPayment> GetUIList(UISearchApplicationPayment uiSearchObj = null)
        {
            IList<UIApplicationPayment> uiEntities = new List<UIApplicationPayment>();

            IQueryable<ApplicationPayment> query = null;

            List<Expression<Func<ApplicationPayment, bool>>> whereFuncs = new List<Expression<Func<ApplicationPayment, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.WorkflowID > 0)
                    whereFuncs.Add(x => x.WorkflowID.Equals(uiSearchObj.WorkflowID));

                if (uiSearchObj.ApplicationID > 0)
                    whereFuncs.Add(x => x.ApplicationID == uiSearchObj.ApplicationID);

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.PayDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.PayDate < uiSearchObj.EndDate);
                }

                if (uiSearchObj.PayDate.HasValue)
                    whereFuncs.Add(x => x.PayDate == uiSearchObj.PayDate);
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join cu in DB.Users on q.CreatedBy equals cu.UserID into tempCU
                              from tcu in tempCU.DefaultIfEmpty()
                              select new UIApplicationPayment()
                              {
                                  ID = q.ID,
                                  FromBankAccountID = q.FromBankAccountID,
                                  FromAccount = q.FromAccount,
                                  ToBankAccountID = q.ToBankAccountID,
                                  ToAccount = q.ToAccount,
                                  Amount = q.Amount,
                                  Fee = q.Fee,
                                  PayDate = q.PayDate,
                                  Comment = q.Comment,
                                  CreatedBy = tcu == null ? string.Empty : tcu.FullName
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIApplicationPayment> GetUIList(UISearchApplicationPayment uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIApplicationPayment> uiEntities = new List<UIApplicationPayment>();

            int total = 0;

            IQueryable<ApplicationPayment> query = null;

            List<Expression<Func<ApplicationPayment, bool>>> whereFuncs = new List<Expression<Func<ApplicationPayment, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID == uiSearchObj.ID);

                if (uiSearchObj.WorkflowID > 0)
                    whereFuncs.Add(x => x.WorkflowID == uiSearchObj.WorkflowID);

                if (uiSearchObj.ApplicationID > 0)
                    whereFuncs.Add(x => x.ApplicationID == uiSearchObj.ApplicationID);

                if (uiSearchObj.PaymentTypeID > 0)
                    whereFuncs.Add(x => x.PaymentTypeID == uiSearchObj.PaymentTypeID);

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.PayDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.PayDate < uiSearchObj.EndDate);
                }

                if (uiSearchObj.PayDate.HasValue)
                    whereFuncs.Add(x => x.PayDate == uiSearchObj.PayDate);

                if (uiSearchObj.PaymentTypeID > 0)
                {
                    whereFuncs.Add(x => x.PaymentTypeID == uiSearchObj.PaymentTypeID);

                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join cu in DB.Users on q.CreatedBy equals cu.UserID into tempCU
                              from tcu in tempCU.DefaultIfEmpty()
                              select new UIApplicationPayment()
                              {
                                  ID = q.ID,
                                  FromBankAccountID = q.FromBankAccountID,
                                  FromAccount = q.FromAccount,
                                  ToBankAccountID = q.ToBankAccountID,
                                  ToAccount = q.ToAccount,
                                  Amount = q.Amount,
                                  Fee = q.Fee,
                                  PayDate = q.PayDate,
                                  Comment = q.Comment,
                                  CreatedBy = tcu == null ? string.Empty : tcu.FullName
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }

        public IList<UIApplicationPayment> GetUIListForMoneyManagement(UISearchApplicationPayment uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIApplicationPayment> uiEntities = new List<UIApplicationPayment>();

            int total = 0;

            IQueryable<ApplicationPayment> query = null;

            List<Expression<Func<ApplicationPayment, bool>>> whereFuncs = new List<Expression<Func<ApplicationPayment, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.WorkflowID > 0)
                    whereFuncs.Add(x => x.WorkflowID == uiSearchObj.WorkflowID);

                if (uiSearchObj.ApplicationID > 0)
                    whereFuncs.Add(x => x.ApplicationID == uiSearchObj.ApplicationID);

                if (uiSearchObj.PaymentTypeID > 0)
                    whereFuncs.Add(x => x.PaymentTypeID == uiSearchObj.PaymentTypeID);

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.PayDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.PayDate < uiSearchObj.EndDate);
                }

                if (uiSearchObj.PayDate.HasValue)
                    whereFuncs.Add(x => x.PayDate == uiSearchObj.PayDate);

                whereFuncs.Add(x => x.PayDate != null);

                if (uiSearchObj.PaymentTypeID > 0)
                {
                    whereFuncs.Add(x => x.PaymentTypeID == uiSearchObj.PaymentTypeID);

                }
                if (uiSearchObj.PaymentStatusID > 0)
                {
                    whereFuncs.Add(x => x.PaymentStatusID == uiSearchObj.PaymentStatusID);
                }
                if (uiSearchObj.BankAccountID.BiggerThanZero())
                {
                    whereFuncs.Add(x => x.FromBankAccountID == uiSearchObj.BankAccountID || x.ToBankAccountID == uiSearchObj.BankAccountID);
                }
                if (uiSearchObj.IncludeBankAccountIDs != null && uiSearchObj.IncludeBankAccountIDs.Count > 0)
                {
                    whereFuncs.Add(x => uiSearchObj.IncludeBankAccountIDs.Contains(x.FromBankAccountID.Value) || uiSearchObj.IncludeBankAccountIDs.Contains(x.ToBankAccountID.Value));
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, "PayDate asc , ID asc", out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join cu in DB.Users on q.CreatedBy equals cu.UserID into tempCU
                              from tcu in tempCU.DefaultIfEmpty()
                              select new UIApplicationPayment()
                              {
                                  ID = q.ID,
                                  FromBankAccountID = q.FromBankAccountID,
                                  FromAccount = q.FromAccount,
                                  ToBankAccountID = q.ToBankAccountID,
                                  ToAccount = q.ToAccount,
                                  Amount = q.Amount,
                                  Fee = q.Fee,
                                  PayDate = q.PayDate,
                                  Comment = q.Comment,
                                  CreatedBy = tcu == null ? string.Empty : tcu.FullName,
                                  PaymentTypeID = q.PaymentTypeID
                              }).ToList();


                uiEntities.ForEach(x =>
                {
                    int? bankAccountID = null;
                    if (x.PaymentTypeID == (int)EPaymentType.Expend)
                    {
                        x.PaymentType = "借";
                        x.OutAmount = x.Amount;

                        bankAccountID = x.FromBankAccountID.Value;
                    }
                    else
                    {
                        x.PaymentType = "贷";
                        x.InAmount = x.Amount;
                        bankAccountID = x.ToBankAccountID.Value;
                    }

                    x.Balance = GetRealTimeBalance(x.ID, bankAccountID.Value, x.PayDate.Value);
                });

                if (uiEntities.Count > 0)
                {
                    var first = uiEntities[0];
                    decimal blance = first.Balance;
                    if (first.PaymentTypeID == (int)EPaymentType.Expend)
                    {
                        blance += first.Amount.Value;
                    }
                    else
                    {
                        blance -= first.Amount.Value;
                    }
                    UIApplicationPayment pre = new UIApplicationPayment()
                    {
                        ID = -1,
                        Comment = "期初余额",
                        Balance = blance,
                        PayDate = new DateTime(first.PayDate.Value.Year, first.PayDate.Value.Month, 1)
                    };
                    uiEntities.Insert(0, pre);
                }
            }

            totalRecords = total;

            return uiEntities;
        }


        private decimal GetRealTimeBalance(int applicationPaymentID, int bankAccountID, DateTime payDate)
        {
            DateTime? start = null;
            DateTime end = payDate;
            IBankAccountBalanceHistoryRepository bankAccountBalanceHistoryRepository = new BankAccountBalanceHistoryRepository();
            IBankAccountRepository bankAccountRepository = new BankAccountRepository();

            //最近一次的 月记录
            BankAccountBalanceHistory lastBankAccountBalanceHistory = bankAccountBalanceHistoryRepository.GetLastByBankAccountID(bankAccountID, payDate);
            if (lastBankAccountBalanceHistory == null)
            {

            }
            else
            {
                start = lastBankAccountBalanceHistory.BalanceDate.AddMonths(1);//要+1
            }


            List<Expression<Func<ApplicationPayment, bool>>> whereFuncs = new List<Expression<Func<ApplicationPayment, bool>>>();
            whereFuncs.Add(x => x.FromBankAccountID == bankAccountID || x.ToBankAccountID == bankAccountID);
            whereFuncs.Add(x => x.PaymentStatusID == (int)EPaymentStatus.Paid);
            whereFuncs.Add(x => x.PayDate <= end && (start == null || x.PayDate >= start));
            whereFuncs.Add(x => x.ID <= applicationPaymentID);

            //时间段内的所有 applicationPayments

            List<ApplicationPayment> applicationPayments = GetList(whereFuncs).OrderBy(x => x.PayDate).ToList();

            decimal preTimeSectionBlance = 0;
            if (lastBankAccountBalanceHistory == null)
            {
                var account = bankAccountRepository.GetByID(bankAccountID);
                preTimeSectionBlance = account.Balance.HasValue ? account.Balance.Value : 0M;
            }
            else
            {
                preTimeSectionBlance = lastBankAccountBalanceHistory.Balance;
            }

            decimal income = applicationPayments.Where(x => x.PaymentTypeID == (int)EPaymentType.Income).Any()
                                ? applicationPayments.Where(x => x.PaymentTypeID == (int)EPaymentType.Income).Sum(x => x.Amount).Value : 0M;
            decimal expend = applicationPayments.Where(x => x.PaymentTypeID == (int)EPaymentType.Expend).Any()
                ? applicationPayments.Where(x => x.PaymentTypeID == (int)EPaymentType.Expend).Sum(x => x.Amount).Value : 0M;
            decimal balance = income - expend + preTimeSectionBlance;

            return balance;
        }
    }
}
