using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.WinService.Lib
{
    public class CalculateBankAccountBalanceService
    {
        /// <summary>
        /// Processes the work.
        /// </summary>
        public static void ProcessWork()
        {
            Utility.WriteTrace("Process CalculateBankAccountBalanceService Work begin at:" + DateTime.Now);
            //首次运行，直接计算前一天的库存
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                var db = unitOfWork.GetDbModel();
                IBankAccountRepository bankAccountRepository = new BankAccountRepository();
                IBankAccountBalanceHistoryRepository bankAccountBalanceHistoryRepository = new BankAccountBalanceHistoryRepository();
                IApplicationPaymentRepository applicationPaymentRepository = new ApplicationPaymentRepository();
                bankAccountRepository.SetDbModel(db);
                bankAccountBalanceHistoryRepository.SetDbModel(db);
                applicationPaymentRepository.SetDbModel(db);


                //获取所有银行账号ID
                List<int> bankAccountIDs = bankAccountRepository.GetList(x => x.OwnerTypeID == (int)EOwnerType.Company).Select(x => x.ID).ToList();
                bankAccountIDs.ForEach(bankAccountID =>
                {
                    //当前 银行账号的 最后一个 余额记录
                    BankAccountBalanceHistory lastBankAccountBalanceHistory = bankAccountBalanceHistoryRepository.GetLastByBankAccountID(bankAccountID);
                    DateTime? startOfCalculate = null;
                    if (lastBankAccountBalanceHistory == null)
                    {
                        startOfCalculate = null;
                    }
                    else
                    {
                        startOfCalculate = lastBankAccountBalanceHistory.BalanceDate.AddMonths(1);//要加1 因为存储的是 5.1日 其实代表的是挣个5月份
                    }
                    DateTime now = DateTime.Now;
                    //计算的截止日期  仅仅计算上个月 和以前的
                    DateTime timelineOfCalculate = new DateTime(now.Year, now.Month, 1);
                    List<Expression<Func<ApplicationPayment, bool>>> whereFuncs = new List<Expression<Func<ApplicationPayment, bool>>>();
                    whereFuncs.Add(x => x.FromBankAccountID == bankAccountID || x.ToBankAccountID == bankAccountID);
                    whereFuncs.Add(x => x.PaymentStatusID == (int)EPaymentStatus.Paid);
                    whereFuncs.Add(x => x.PayDate < timelineOfCalculate && (startOfCalculate == null || x.PayDate > startOfCalculate));

                    int applicationPaymentCount = applicationPaymentRepository.GetList(whereFuncs).Count();

                    if (applicationPaymentCount > 0)
                    {

                        //时间段内的所有 applicationPayments
                        List<ApplicationPayment> applicationPayments = applicationPaymentRepository.GetList(whereFuncs).OrderBy(x => x.PayDate).ToList();
                        //如果时间段内 包含多个月份的东西 那么要分割
                        while (applicationPayments.Count > 0)
                        {
                            DateTime firstPayDate = applicationPayments.First().PayDate.Value;
                            //DateTime lastPayDate = applicationPayments.Last().PayDate.Value;
                            DateTime startTimeSection = firstPayDate;
                            DateTime endTimeSection = new DateTime(startTimeSection.Year, startTimeSection.Month, 1).AddMonths(1);

                            List<ApplicationPayment> applicationPaymentsSection = applicationPayments.Where(x => x.PayDate >= startTimeSection
                                && x.PayDate < endTimeSection).ToList();

                            lastBankAccountBalanceHistory = bankAccountBalanceHistoryRepository.GetLastByBankAccountID(bankAccountID);
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

                            decimal income = applicationPaymentsSection.Where(x => x.PaymentTypeID == (int)EPaymentType.Income).Any()
                                  ? applicationPaymentsSection.Where(x => x.PaymentTypeID == (int)EPaymentType.Income).Sum(x => x.Amount).Value : 0M;
                            decimal expend = applicationPaymentsSection.Where(x => x.PaymentTypeID == (int)EPaymentType.Expend).Any()
                                ? applicationPaymentsSection.Where(x => x.PaymentTypeID == (int)EPaymentType.Expend).Sum(x => x.Amount).Value : 0M;
                            decimal balance = income - expend + preTimeSectionBlance;
                            BankAccountBalanceHistory bankAccountBalanceHistory = new BankAccountBalanceHistory()
                            {
                                BalanceDate = endTimeSection.AddMonths(-1),//比如存储的是 2015.05.01 那么代表的是5月份的
                                Balance = balance,
                                BankAccountID = bankAccountID,
                            };
                            bankAccountBalanceHistoryRepository.Add(bankAccountBalanceHistory);
                            unitOfWork.SaveChanges();

                            applicationPaymentsSection.ForEach(x =>
                            {
                                applicationPayments.Remove(x);
                            });
                        }

                    }
                });

            }


            Utility.WriteTrace("Process CalculateBankAccountBalanceService Work end at:" + DateTime.Now);
        }
    }
}
