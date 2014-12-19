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
        /// 修改订单
        /// </summary>
        EditOrder,
    }
}
