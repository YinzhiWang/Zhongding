using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;

namespace ZhongDing.Business.IRepositories
{
    public interface IBankAccountBalanceHistoryRepository : IBaseRepository<BankAccountBalanceHistory>
    {
        int GetCount();

        BankAccountBalanceHistory GetLastByBankAccountID(int bankAccountID);

        BankAccountBalanceHistory GetLastByBankAccountID(int bankAccountID, DateTime payDate);
    }
}
