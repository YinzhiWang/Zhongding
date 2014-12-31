﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 枚举：工作流状态
    /// </summary>
    public enum EWorkflowStatus : int
    {
        /// <summary>
        /// 暂存
        /// </summary>
        TemporarySave = 1,
        /// <summary>
        /// 提交
        /// </summary>
        Submit,
        /// <summary>
        /// 基础信息审核通过
        /// </summary>
        ApprovedBasicInfo,
        /// <summary>
        /// 基础信息退回
        /// </summary>
        ReturnBasicInfo,
        /// <summary>
        /// 支付信息待审核
        /// </summary>
        AuditingOfPaymentInfo,
        /// <summary>
        /// 待支付
        /// </summary>
        ToBePaid,

        /// <summary>
        /// 支付信息退回
        /// </summary>
        ReturnPaymentInfo,

        /// <summary>
        /// 已支付
        /// </summary>
        Paid,

        /// <summary>
        /// 待入库
        /// </summary>
        ToBeInWarehouse,

        /// <summary>
        /// 已入库
        /// </summary>
        InWarehouse,

        /// <summary>
        /// 已生成配送订单
        /// </summary>
        ExportToDBOrder,
    }
}
