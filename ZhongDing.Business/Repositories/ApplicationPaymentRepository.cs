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
                    whereFuncs.Add(x => x.ID == uiSearchObj.ID);

                if (uiSearchObj.WorkflowID > 0)
                    whereFuncs.Add(x => x.WorkflowID == uiSearchObj.WorkflowID);

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

                if (uiSearchObj.PaymentTypeID > 0)
                {
                    whereFuncs.Add(x => x.PaymentTypeID == uiSearchObj.PaymentTypeID);
                }
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
                                  CreatedBy = tcu == null ? string.Empty : tcu.FullName,
                                  PaymentTypeID = q.PaymentTypeID
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
                        Comment = (pageIndex == 0) ? "期初余额" : "上页的余额",
                        Balance = blance,
                        //PayDate = new DateTime(first.PayDate.Value.Year, first.PayDate.Value.Month, 1)
                    };
                    uiEntities.Insert(0, pre);
                }
            }
            int tempCount = pageIndex + 1;
            totalRecords = total + tempCount;

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
            whereFuncs.Add(x => (x.PayDate <= end && (start == null || x.PayDate >= start) && x.ID <= applicationPaymentID) ||
                (x.PayDate < end && (start == null || x.PayDate >= start)));
            //whereFuncs.Add(x => x.ID <= applicationPaymentID);


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


        public decimal GetRealTimeBalance(int bankAccountID, DateTime month)
        {
            IQueryable<ApplicationPayment> query = null;

            List<Expression<Func<ApplicationPayment, bool>>> whereFuncs = new List<Expression<Func<ApplicationPayment, bool>>>();
            whereFuncs.Add(x => x.PaymentStatusID == (int)EPaymentStatus.Paid);
            whereFuncs.Add(x => x.FromBankAccountID == bankAccountID || x.ToBankAccountID == bankAccountID);
            DateTime endLinePayDate = month.AddMonths(1);
            whereFuncs.Add(x => x.PayDate < endLinePayDate);
            query = GetList(whereFuncs, "PayDate desc , ID desc");

            ApplicationPayment applicationPayment = query.FirstOrDefault();
            if (applicationPayment == null)
            {
                return 0;
            }
            decimal balance = GetRealTimeBalance(applicationPayment.ID, bankAccountID, applicationPayment.PayDate.Value);

            return balance;
        }

        /// <summary>
        ///  获取 指定帐套，指定年月 销售收入 （招商模式本月的入账）
        ///  这里提取订单收款的信息，按照实际收款金额 来计算
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        public decimal GetSaleIncomeAttractBusinessMode(int companyID, DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            var query = from clientSaleApplication in DB.ClientSaleApplication.Where(x => x.SalesOrderApplication.SaleOrderTypeID == (int)ESaleOrderType.AttractBusinessMode
                            && x.IsDeleted == false && x.CompanyID == companyID)
                        join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.ClientOrder
                            && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid) on clientSaleApplication.ID equals applicationPayment.ApplicationID
                        where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                        select applicationPayment.Amount;
            decimal? income = query.Any() ? query.Sum() : 0M;

            return income.Value;
        }

        public decimal GetSupplierRefund(int companyID, DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            var querySupplierRefund = from sra in DB.SupplierRefundApplication
                                      join applicationPayment in DB.ApplicationPayment.Where(x => x.IsDeleted == false && x.WorkflowID == (int)EWorkflow.SupplierRefunds
                                                                             && x.PaymentStatusID == (int)EPaymentStatus.Paid)
                                      on sra.ID equals applicationPayment.ApplicationID
                                      where
                                      sra.IsDeleted == false
                                      && sra.CompanyID == companyID
                                      && sra.WorkflowID == (int)EWorkflow.SupplierRefunds
                                      && applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                                      select new { applicationPayment.Amount };
            var amountSupplierRefund = querySupplierRefund.Any() ?
                                           querySupplierRefund.Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0))
                                           : 0M;


            var querySupplierTaskRefund = from sra in DB.SupplierRefundApplication
                                          join applicationPayment in DB.ApplicationPayment.Where(x => x.IsDeleted == false && x.WorkflowID == (int)EWorkflow.SupplierTaskRefunds
                                                                                 && x.PaymentStatusID == (int)EPaymentStatus.Paid)
                                          on sra.ID equals applicationPayment.ApplicationID
                                          where
                                          sra.IsDeleted == false
                                          && sra.CompanyID == companyID
                                          && sra.WorkflowID == (int)EWorkflow.SupplierTaskRefunds
                                          && applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                                          select new { applicationPayment.Amount };
            var amountSupplierTaskRefund = querySupplierTaskRefund.Any() ?
                                          querySupplierTaskRefund.Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0))
                                          : 0M;

            return amountSupplierRefund + amountSupplierTaskRefund;
        }


        public decimal GetSaleIncomeDaBaoMode(int distributionCompanyID, DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            var query = from daBaoApplication in DB.DaBaoApplication.Where(x =>
                            x.SalesOrderApplication.SaleOrderTypeID == (int)ESaleOrderType.DaBaoMode
                            && x.IsDeleted == false && x.DistributionCompanyID == distributionCompanyID)
                        join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.ClientOrder
                            && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid) on daBaoApplication.ID equals applicationPayment.ApplicationID
                        where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                        select applicationPayment.Amount;
            decimal? income = query.Any() ? query.Sum() : 0M;

            return income.Value;
        }


        public decimal GetSaleIncomeAttachedMode(DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            var query = from clientSaleApplication in DB.ClientSaleApplication.Where(x => x.SalesOrderApplication.SaleOrderTypeID == (int)ESaleOrderType.AttachedMode
                            && x.IsDeleted == false)
                        join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.ClientOrder
                            && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid) on clientSaleApplication.ID equals applicationPayment.ApplicationID
                        where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                        select applicationPayment.Amount;
            decimal? income = query.Any() ? query.Sum() : 0M;

            return income.Value;
        }


        public decimal GetInvoiceIncomeSupplierInvoice(int companyID, DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            var query = from supplierInvoiceSettlement in DB.SupplierInvoiceSettlement.Where(x => x.IsDeleted == false
                            && x.CompanyID == companyID)
                        join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.SupplierInvoiceSettlement
                            && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid) on supplierInvoiceSettlement.ID equals applicationPayment.ApplicationID
                        where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                        select applicationPayment.Amount;
            decimal? income = query.Any() ? query.Sum() : 0M;

            return income.Value;
        }


        public decimal GetPurchaseAmount(int companyID, DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            var query = from procureOrderApplication in DB.ProcureOrderApplication.Where(x => x.IsDeleted == false
                            && x.Supplier.CompanyID == companyID)
                        join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.ProcureOrder
                            && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid) on procureOrderApplication.ID equals applicationPayment.ApplicationID
                        where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                        select new { applicationPayment.Amount, applicationPayment.Fee };
            decimal? income = query.Any() ? query.Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0) + (x.Fee.HasValue ? x.Fee.Value : 0)) : 0M;

            return income.Value;
        }


        public decimal GetClientRefund(int companyID, DateTime generateDate)
        {

            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            var querySupplierRefund = from clientRefundApplication in DB.ClientRefundApplication
                                      join applicationPayment in DB.ApplicationPayment.Where(x => x.IsDeleted == false && x.WorkflowID == (int)EWorkflow.ClientRefunds
                                                                             && x.PaymentStatusID == (int)EPaymentStatus.Paid)
                                      on clientRefundApplication.ID equals applicationPayment.ApplicationID
                                      where
                                      clientRefundApplication.IsDeleted == false
                                      && clientRefundApplication.CompanyID == companyID
                                      && applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                                      select new { applicationPayment.Amount, applicationPayment.Fee };
            var amountSupplierRefund = querySupplierRefund.Any() ?
                                           querySupplierRefund.Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0) + (x.Fee.HasValue ? x.Fee.Value : 0))
                                           : 0M;

            var querySupplierTaskRefund = from clientTaskRefundApplication in DB.ClientTaskRefundApplication
                                          join applicationPayment in DB.ApplicationPayment.Where(x => x.IsDeleted == false && x.WorkflowID == (int)EWorkflow.ClientTaskRefunds
                                                                                 && x.PaymentStatusID == (int)EPaymentStatus.Paid)
                                          on clientTaskRefundApplication.ID equals applicationPayment.ApplicationID
                                          where
                                          clientTaskRefundApplication.IsDeleted == false
                                          && clientTaskRefundApplication.CompanyID == companyID
                                          && applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                                          select new { applicationPayment.Amount, applicationPayment.Fee };
            var amountSupplierTaskRefund = querySupplierTaskRefund.Any() ?
                                       querySupplierTaskRefund.Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0) + (x.Fee.HasValue ? x.Fee.Value : 0))
                                       : 0M;


            return amountSupplierRefund + amountSupplierTaskRefund;
        }

        /// <summary>
        /// 基本工资
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        public decimal GetSalary(DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            var query = from applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.SalarySettleManagement
                            && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid)
                        where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                        select new { applicationPayment.Amount, applicationPayment.Fee };
            decimal? income = query.Any() ? query.Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0) + (x.Fee.HasValue ? x.Fee.Value : 0)) : 0M;

            return income.Value;
        }

        /// <summary>
        /// 费用报销	不含发货费，不含托管配送费	物流的 排除，托管配送费 排除
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        public decimal GetReimbursement(DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            //总计
            var queryReimbursement = from reimbursement in DB.Reimbursement.Where(x => x.IsDeleted == false)
                                     join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.ReimbursementManagement
                                         && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid) on reimbursement.ID equals applicationPayment.ApplicationID
                                     where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                                     select new { applicationPayment.Amount, applicationPayment.Fee };
            decimal? allReimbursement = queryReimbursement.Any() ? queryReimbursement.Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0) + (x.Fee.HasValue ? x.Fee.Value : 0)) : 0M;


            //排除
            var queryExcludeReimbursementIDs = from reimbursement in DB.Reimbursement.Where(x => x.IsDeleted == false)
                                               join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.ReimbursementManagement
                                                   && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid) on reimbursement.ID equals applicationPayment.ApplicationID
                                               where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                                               select reimbursement.ID;
            var queryExcludeReimbursement = from queryExcludeReimbursementID in queryExcludeReimbursementIDs
                                            join reimbursementDetail in DB.ReimbursementDetail on queryExcludeReimbursementID equals reimbursementDetail.ReimbursementID
                                            where reimbursementDetail.ReimbursementID == (int)EReimbursementType.ManagedDistributionFee ||
                                            reimbursementDetail.ReimbursementID == (int)EReimbursementType.TransportFee
                                            select reimbursementDetail.Amount;
            decimal? allExcludeReimbursement = queryExcludeReimbursement.Any() ? queryExcludeReimbursement.Sum() : 0M;

            //减除
            return allReimbursement.Value - allExcludeReimbursement.Value;
        }

        /// <summary>
        /// 借款支出
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        public decimal GetBorrowMoneyExpend(DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            //总计
            var queryAmount = from borrowMoney in DB.BorrowMoney.Where(x => x.IsDeleted == false)
                              join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.BorrowMoneyManagement
                                  && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid
                                  && x.PaymentTypeID == (int)EPaymentType.Expend) on borrowMoney.ID equals applicationPayment.ApplicationID
                              where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                              select new { applicationPayment.Amount, applicationPayment.Fee };
            decimal? amount = queryAmount.Any() ? queryAmount.Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0) + (x.Fee.HasValue ? x.Fee.Value : 0)) : 0M;

            return amount.Value;
        }


        public decimal GetSupplierCautionMoneyIncome(DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            //总计
            var queryAmount = from supplierCautionMoney in DB.SupplierCautionMoney.Where(x => x.IsDeleted == false)
                              join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.SupplierCautionMoneyApply
                                  && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid
                                  && x.PaymentTypeID == (int)EPaymentType.Income) on supplierCautionMoney.ID equals applicationPayment.ApplicationID
                              where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                              select applicationPayment.Amount;
            decimal? amount = queryAmount.Any() ? queryAmount.Sum() : 0M;

            return amount.Value;
        }

        public decimal GetClientCautionMoneyIncome(DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            //总计
            var queryAmount = from supplierCautionMoney in DB.SupplierCautionMoney.Where(x => x.IsDeleted == false)
                              join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.ClientCautionMoney
                                  && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid
                                  && x.PaymentTypeID == (int)EPaymentType.Income) on supplierCautionMoney.ID equals applicationPayment.ApplicationID
                              where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                              select applicationPayment.Amount;
            decimal? amount = queryAmount.Any() ? queryAmount.Sum() : 0M;

            return amount.Value;
        }


        public decimal GetBorrowMoneyIncome(DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            //总计
            var queryAmount = from borrowMoney in DB.BorrowMoney.Where(x => x.IsDeleted == false)
                              join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.BorrowMoneyManagement
                                  && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid
                                  && x.PaymentTypeID == (int)EPaymentType.Income) on borrowMoney.ID equals applicationPayment.ApplicationID
                              where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                              select applicationPayment.Amount;
            decimal? amount = queryAmount.Any() ? queryAmount.Sum() : 0M;

            return amount.Value;
        }


        public decimal GetCautionMoneyReturnToClient(DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            //总计
            var queryAmount = from supplierCautionMoney in DB.SupplierCautionMoney.Where(x => x.IsDeleted == false)
                              join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.ClientCautionMoney
                                  && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid
                                  && x.PaymentTypeID == (int)EPaymentType.Expend) on supplierCautionMoney.ID equals applicationPayment.ApplicationID
                              where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                              select new { applicationPayment.Amount, applicationPayment.Fee };
            decimal? amount = queryAmount.Any() ? queryAmount.Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0) + (x.Fee.HasValue ? x.Fee.Value : 0)) : 0M;

            return amount.Value;
        }

        public decimal GetCautionMoneyPayToSupplier(DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            //总计
            var queryAmount = from supplierCautionMoney in DB.SupplierCautionMoney.Where(x => x.IsDeleted == false)
                              join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.ClientCautionMoney
                                  && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid
                                  && x.PaymentTypeID == (int)EPaymentType.Expend) on supplierCautionMoney.ID equals applicationPayment.ApplicationID
                              where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                              select new { applicationPayment.Amount, applicationPayment.Fee };
            decimal? amount = queryAmount.Any() ? queryAmount.Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0) + (x.Fee.HasValue ? x.Fee.Value : 0)) : 0M;

            return amount.Value;
        }

        /// <summary>
        /// 销项票税支出-万国康	 	 发票模块的： 客户发票结算管理 和 挂靠发票结算管理
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        public decimal GetInvoiceExpend(int companyID, DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            var queryClientInvoiceSettlement = from clientInvoiceSettlement in DB.ClientInvoiceSettlement
                                               join applicationPayment in DB.ApplicationPayment.Where(x => x.IsDeleted == false
                                                   && x.WorkflowID == (int)EWorkflow.ClientInvoiceSettlement
                                                   && x.PaymentStatusID == (int)EPaymentStatus.Paid
                                                   && x.PaymentTypeID == (int)EPaymentType.Expend)
                                               on clientInvoiceSettlement.ID equals applicationPayment.ApplicationID
                                               where
                                               clientInvoiceSettlement.IsDeleted == false
                                               && clientInvoiceSettlement.CompanyID == companyID
                                               && applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                                               select new { applicationPayment.Amount, applicationPayment.Fee };
            var amountClientInvoiceSettlement = queryClientInvoiceSettlement.Any() ?
                                       queryClientInvoiceSettlement.Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0) + (x.Fee.HasValue ? x.Fee.Value : 0))
                                       : 0M;

            var queryClientAttachedInvoiceSettlement = from clientAttachedInvoiceSettlement in DB.ClientAttachedInvoiceSettlement
                                                       join applicationPayment in DB.ApplicationPayment.Where(x => x.IsDeleted == false
                                                           && x.WorkflowID == (int)EWorkflow.ClientAttachedInvoiceSettlement
                                                           && x.PaymentStatusID == (int)EPaymentStatus.Paid
                                                           && x.PaymentTypeID == (int)EPaymentType.Expend)
                                                       on clientAttachedInvoiceSettlement.ID equals applicationPayment.ApplicationID
                                                       where
                                                       clientAttachedInvoiceSettlement.IsDeleted == false
                                                       && clientAttachedInvoiceSettlement.CompanyID == companyID
                                                       && applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                                                       select new { applicationPayment.Amount, applicationPayment.Fee };
            var amountClientAttachedInvoiceSettlement = queryClientAttachedInvoiceSettlement.Any() ?
                                    queryClientAttachedInvoiceSettlement.Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0) + (x.Fee.HasValue ? x.Fee.Value : 0))
                                    : 0M;
            return amountClientInvoiceSettlement + amountClientAttachedInvoiceSettlement;
        }


        public decimal GetShippingFee(int companyID, DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            //排除
            var queryExcludeReimbursementIDs = from reimbursement in DB.Reimbursement.Where(x => x.IsDeleted == false)
                                               join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.ReimbursementManagement
                                                   && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid) on reimbursement.ID equals applicationPayment.ApplicationID
                                               where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                                               select reimbursement.ID;
            var queryExcludeReimbursement = from queryExcludeReimbursementID in queryExcludeReimbursementIDs
                                            join reimbursementDetail in DB.ReimbursementDetail on queryExcludeReimbursementID equals reimbursementDetail.ReimbursementID
                                            where
                                            reimbursementDetail.ReimbursementID == (int)EReimbursementType.TransportFee
                                            select reimbursementDetail.Amount;
            decimal? allExcludeReimbursement = queryExcludeReimbursement.Any() ? queryExcludeReimbursement.Sum() : 0M;

            return allExcludeReimbursement.Value;
        }

        /// <summary>
        /// 托管配送费	费用报销中的托管配送费	 费用报销中的 添加托管配送费类型
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        public decimal GetManagedDistributionFee(DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            //排除
            var queryExcludeReimbursementIDs = from reimbursement in DB.Reimbursement.Where(x => x.IsDeleted == false)
                                               join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.ReimbursementManagement
                                                   && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid) on reimbursement.ID equals applicationPayment.ApplicationID
                                               where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                                               select reimbursement.ID;
            var queryExcludeReimbursement = from queryExcludeReimbursementID in queryExcludeReimbursementIDs
                                            join reimbursementDetail in DB.ReimbursementDetail on queryExcludeReimbursementID equals reimbursementDetail.ReimbursementID
                                            where reimbursementDetail.ReimbursementID == (int)EReimbursementType.ManagedDistributionFee
                                            select reimbursementDetail.Amount;
            decimal? allExcludeReimbursement = queryExcludeReimbursement.Any() ? queryExcludeReimbursement.Sum() : 0M;

            //减除
            return allExcludeReimbursement.Value;
        }


        public decimal GetOther(DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            //排除
            var queryExcludeReimbursementIDs = from reimbursement in DB.Reimbursement.Where(x => x.IsDeleted == false)
                                               join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.ReimbursementManagement
                                                   && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid) on reimbursement.ID equals applicationPayment.ApplicationID
                                               where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                                               select reimbursement.ID;
            var queryExcludeReimbursement = from queryExcludeReimbursementID in queryExcludeReimbursementIDs
                                            join reimbursementDetail in DB.ReimbursementDetail on queryExcludeReimbursementID equals reimbursementDetail.ReimbursementID
                                            where reimbursementDetail.ReimbursementID == (int)EReimbursementType.Other
                                            select reimbursementDetail.Amount;
            decimal? allExcludeReimbursement = queryExcludeReimbursement.Any() ? queryExcludeReimbursement.Sum() : 0M;

            //减除
            return allExcludeReimbursement.Value;
        }

        /// <summary>
        /// 厂家经理返款	付给厂家经理	 厂家经理返款管理
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        public decimal GetFMRefund(DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            //总计
            var queryAmount = from factoryManagerRefundApplication in DB.FactoryManagerRefundApplication.Where(x => x.IsDeleted == false)
                              join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.FactoryManagerRefunds
                                  && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid
                                  && x.PaymentTypeID == (int)EPaymentType.Expend) on factoryManagerRefundApplication.ID equals applicationPayment.ApplicationID
                              where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                              select new { applicationPayment.Amount, applicationPayment.Fee };
            decimal? amount = queryAmount.Any() ? queryAmount.Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0) + (x.Fee.HasValue ? x.Fee.Value : 0)) : 0M;

            return amount.Value;
        }

        /// <summary>
        /// 大包客户返款-四个配送公司	 	 工作流中的 大包客户提成结算
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        public decimal GetDaBaoRefund(DateTime generateDate)
        {
            DateTime start = generateDate;
            DateTime end = generateDate.AddMonths(1);
            var query = from applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.DBClientSettleBonus
                            && x.IsDeleted == false && x.PaymentStatusID == (int)EPaymentStatus.Paid)
                        where applicationPayment.PayDate >= start && applicationPayment.PayDate < end
                        select new { applicationPayment.Amount, applicationPayment.Fee };
            decimal? income = query.Any() ? query.Sum(x => (x.Amount.HasValue ? x.Amount.Value : 0) + (x.Fee.HasValue ? x.Fee.Value : 0)) : 0M;

            return income.Value;
        }
    }
}
