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
        StockIn,
        /// <summary>
        /// 大包配送申请单
        /// </summary>
        DBOrderRequest,
        /// <summary>
        /// 大包配送订单
        /// </summary>
        DBOrder,
        /// <summary>
        /// 大包出库单
        /// </summary>
        DBStockOut,

        /// <summary>
        /// 客户订单
        /// </summary>
        ClientOrder,

        /// <summary>
        /// 客户订单出库单
        /// </summary>
        ClientOrderStockOut,

        /// <summary>
        /// 供应商返款
        /// </summary>
        SupplierRefunds,
    }
}
