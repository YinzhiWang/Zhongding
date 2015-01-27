using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 收款方式
    /// </summary>
    public enum EPaymentMethod : int
    {
        /// <summary>
        /// 银行转账
        /// </summary>
        BankTransfer = 1,
        /// <summary>
        /// 抵扣
        /// </summary>
        Deduction
    }
}
