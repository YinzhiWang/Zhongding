using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    public enum EBorrowMoneyStatus : int
    {
        /// <summary>
        /// 未还款
        /// </summary>
        NotReturn = 1,
        /// <summary>
        /// 已还款
        /// </summary>
        Returned = 2
    }
}
