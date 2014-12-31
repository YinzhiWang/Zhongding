using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 枚举：工作流步骤
    /// </summary>
    public enum EWorkflowStep : int
    {
        /// <summary>
        /// 新增采购订单
        /// </summary>
        NewProcureOrder = 1,
        /// <summary>
        /// 审核采购订单
        /// </summary>
        AuditProcureOrder,
        /// <summary>
        /// 审核支付信息
        /// </summary>
        AuditPaymentInfo,
        /// <summary>
        /// 采购订单出纳
        /// </summary>
        ProcureOrderCashier,

        /// <summary>
        /// 修改采购订单
        /// </summary>
        EditProcureOrder,

        /// <summary>
        /// 新增入库单
        /// </summary>
        NewStockIn,

        /// <summary>
        /// 入库操作
        /// </summary>
        EntryStockRoom,

        /// <summary>
        /// 修改入库单
        /// </summary>
        EditStockIn,

        /// <summary>
        /// 新增大包配送申请单
        /// </summary>
        NewDBOrderRequest,

        /// <summary>
        /// 审核大包配送申请单
        /// </summary>
        AuditDBOrderRequest,

        /// <summary>
        /// 修改大包配送申请单
        /// </summary>
        EditDBOrderRequest,

        /// <summary>
        /// 修改大包配送订单
        /// </summary>
        EditDBOrder,

    }
}
