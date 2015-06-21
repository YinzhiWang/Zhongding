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
        /// NULL
        /// </summary>
        NULL = -1,
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

        /// <summary>
        /// 客户返款
        /// </summary>
        ClientRefunds,

        /// <summary>
        /// 厂家经理返款
        /// </summary>
        FactoryManagerRefunds = 10,

        /// <summary>
        /// 供应商任务返款
        /// </summary>
        SupplierTaskRefunds,

        /// <summary>
        /// 客户任务奖励返款
        /// </summary>
        ClientTaskRefunds,

        /// <summary>
        /// 大包用户提成结算
        /// </summary>
        DBClientSettleBonus,

        /// <summary>
        /// 供应商发票结算
        /// </summary>
        SupplierInvoiceSettlement,

        /// <summary>
        /// 客户发票结算
        /// </summary>
        ClientInvoiceSettlement,

        /// <summary>
        /// 大包收款
        /// </summary>
        DBClientInvoiceSettlement,

        /// <summary>
        /// 挂靠发票结算
        /// </summary>
        ClientAttachedInvoiceSettlement,

        /// <summary>
        /// 供应商 保证金申请 
        /// </summary>
        SupplierCautionMoneyApply,
        /// <summary>
        /// 客户保证金
        /// </summary>
        ClientCautionMoney = 19,
        /// <summary>
        /// 客户保证金退回申请
        /// </summary>
        ClientCautionMoneyReturnApply = 20,

        /// <summary>
        /// 借款管理 21
        /// </summary>
        BorrowMoneyManagement = 21,

        /// <summary>
        /// 现金管理 22
        /// </summary>
        MoneyManagement = 22,
        /// <summary>
        /// 工资结算管理 23
        /// </summary>
        SalarySettleManagement = 23

    }
}
