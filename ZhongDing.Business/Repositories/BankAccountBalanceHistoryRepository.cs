using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;

namespace ZhongDing.Business.Repositories
{
    public class BankAccountBalanceHistoryRepository : BaseRepository<BankAccountBalanceHistory>, IBankAccountBalanceHistoryRepository
    {
        public int GetCount()
        {
            int count = base.BaseList().Count();
            return count;
        }


        public BankAccountBalanceHistory GetLastByBankAccountID(int bankAccountID)
        {
            var entity = GetList(x => x.BankAccountID == bankAccountID).OrderByDescending(x => x.BalanceDate).FirstOrDefault();
            return entity;
        }

        /// <summary>
        /// 获取 指定账号 和 付款时间  所对应的 最近一条 余额记录
        /// </summary>
        /// <param name="bankAccountID"></param>
        /// <param name="payDate"></param>
        /// <returns></returns>
        public BankAccountBalanceHistory GetLastByBankAccountID(int bankAccountID, DateTime payDate)
        {
            DateTime balanceDate = new DateTime(payDate.Year, payDate.Month, 1).AddMonths(-1);
            var entity = GetList(x => x.BankAccountID == bankAccountID && x.BalanceDate <= balanceDate).OrderByDescending(x => x.BalanceDate).FirstOrDefault();
            return entity;
        }
    }
}
