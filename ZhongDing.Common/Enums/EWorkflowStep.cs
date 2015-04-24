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
        AuditDBOrderRequest = 10,

        /// <summary>
        /// 修改大包配送申请单
        /// </summary>
        EditDBOrderRequest,

        /// <summary>
        /// 修改大包配送订单
        /// </summary>
        EditDBOrder,

        /// <summary>
        /// 新增大包出库单
        /// </summary>
        NewDBStockOut,

        /// <summary>
        /// 大包出库单出库
        /// </summary>
        OutDBStockRoom,

        /// <summary>
        /// 修改大包出库单
        /// </summary>
        EditDBStockOut,

        /// <summary>
        /// 新增客户订单
        /// </summary>
        NewClientOrder,

        /// <summary>
        /// 审核客户订单
        /// </summary>
        AuditClientOrder,

        /// <summary>
        /// 中止客户订单
        /// </summary>
        StopClientOrder,

        /// <summary>
        /// 修改客户订单
        /// </summary>
        EditClientOrder,

        /// <summary>
        /// 新增客户订单出库单
        /// </summary>
        NewClientStockOut = 20,

        /// <summary>
        /// 客户订单出库操作
        /// </summary>
        OutClientStockRoom,

        /// <summary>
        /// 修改客户订单出库单
        /// </summary>
        EditClientStockOut,

        /// <summary>
        /// 修改供应商返款
        /// </summary>
        EditSupplierRefund,

        /// <summary>
        /// 新增客户返款
        /// </summary>
        NewClientRefund,

        /// <summary>
        /// 财务主管审核（客户返款）
        /// </summary>
        AuditClientRefundByTreasurers,

        /// <summary>
        /// 部门经理审核（客户返款）
        /// </summary>
        AuditClientRefundByDeptManagers,

        /// <summary>
        /// 出纳支付客户返款
        /// </summary>
        PayClientRefund,

        /// <summary>
        /// 修改客户返款
        /// </summary>
        EditClientRefund,

        /// <summary>
        /// 新增厂家经理返款
        /// </summary>
        NewFMRefund,

        /// <summary>
        /// 财务主管审核（厂家经理返款）
        /// </summary>
        AuditFMRefundByTreasurers = 30,

        /// <summary>
        /// 部门经理审核（厂家经理返款）
        /// </summary>
        AuditFMRefundByDeptManagers,

        /// <summary>
        /// 出纳支付厂家经理返款
        /// </summary>
        PayFMRefund,

        /// <summary>
        /// 修改厂家经理返款
        /// </summary>
        EditFMRefund,

        /// <summary>
        /// 修改供应商任务返款
        /// </summary>
        EditSupplierTaskRefund,

        /// <summary>
        /// 新增客户任务奖励返款
        /// </summary>
        NewClientTaskRefund,

        /// <summary>
        /// 大区经理审核（客户任务奖励返款）
        /// </summary>
        AuditCTRefundByDistrictManagers,

        /// <summary>
        /// 市场总管审核（客户任务奖励返款）
        /// </summary>
        AuditCTRefundByMarketManagers,

        /// <summary>
        /// 财务主管审核（客户任务奖励返款）
        /// </summary>
        AuditCTRefundByTreasurers,

        /// <summary>
        /// 部门领导审核（客户任务奖励返款）
        /// </summary>
        AuditCTRefundByDeptManagers,

        /// <summary>
        /// 出纳支付（客户任务奖励返款）
        /// </summary>
        PayCTRefund = 40,

        /// <summary>
        /// 修改客户任务奖励返款
        /// </summary>
        EditClientTaskRefund,

        /// <summary>
        /// 申请结算大包客户提成
        /// </summary>
        ApplyDBClientSettleBonus,

        /// <summary>
        /// 部门领导审核（大包客户提成）
        /// </summary>
        AuditDBClientSettleBonusByDeptManagers,

        /// <summary>
        /// 出纳支付（大包客户提成）
        /// </summary>
        PayDBClientSettleBonus,

        /// <summary>
        /// 修改大包客户提成
        /// </summary>
        EditDBClientSettleBonus,

        /// <summary>
        /// 新增供应商发票结算
        /// </summary>
        NewSupplierInvoiceSettlement,

        /// <summary>
        /// 修改供应商发票结算
        /// </summary>
        EditSupplierInvoiceSettlement,

        /// <summary>
        /// 新增客户发票结算
        /// </summary>
        NewCISettlement,

        /// <summary>
        /// 财务主管审核（客户发票结算）
        /// </summary>
        AuditCISettlementByTreasurers,

        /// <summary>
        /// 部门领导审核（客户发票结算）
        /// </summary>
        AuditCISettlementByDeptManagers = 50,

        /// <summary>
        /// 出纳支付（客户发票结算）
        /// </summary>
        PayCISettlement,

        /// <summary>
        /// 修改客户发票结算
        /// </summary>
        EditCISettlement,

        /// <summary>
        /// 中止采购订单
        /// </summary>
        StopProcureOrder,

        /// <summary>
        /// 新增大包收款
        /// </summary>
        NewDBClientInvoiceSettlement,

        /// <summary>
        /// 修改大包收款
        /// </summary>
        EditDBClientInvoiceSettlement,
    }
}
