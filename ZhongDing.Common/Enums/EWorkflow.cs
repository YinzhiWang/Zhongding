using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 枚举：工作流
    /// </summary>
    public enum EWorkflow : int
    {
        /// <summary>
        /// 采购订单
        /// </summary>
        ProcureOrder = 1,
        /// <summary>
        /// 采购入库单
        /// </summary>
        ProcureReceipt,
        /// <summary>
        /// 大包申请单
        /// </summary>
        DBClientOrder,
        /// <summary>
        /// 大包配送订单
        /// </summary>
        DBDistribution,
        /// <summary>
        /// 大包出库单
        /// </summary>
        DBDelivery
    }
}
