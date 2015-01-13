using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 枚举：出库配送类型
    /// </summary>
    public enum EReceiverType : int
    {
        /// <summary>
        /// 配送公司（大包配送订单出库单）
        /// </summary>
        DistributionCompany = 1,
        /// <summary>
        /// 客户（客户订单出库单）
        /// </summary>
        ClientUser,
    }
}
