using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 枚举：支付状态
    /// </summary>
    public enum EPaymentStatus : int
    {
        /// <summary>
        /// 待支付
        /// </summary>
        ToBePaid = 1,
        /// <summary>
        /// 已支付
        /// </summary>
        Paid

    }
}
