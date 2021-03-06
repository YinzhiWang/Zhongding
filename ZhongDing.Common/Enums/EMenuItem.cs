﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 枚举：菜单项
    /// </summary>
    public enum EMenuItem : int
    {
        /// <summary>
        /// 首页
        /// </summary>
        Home = 1,
        /// <summary>
        /// 基础信息管理
        /// </summary>
        BasicInfoManage,
        /// <summary>
        /// 账套管理
        /// </summary>
        CompanyManage,
        /// <summary>
        /// 银行账号管理
        /// </summary>
        BankAccountManage,
        /// <summary>
        /// 供应商管理
        /// </summary>
        SupplierManage,
        /// <summary>
        /// 商业单位管理
        /// </summary>
        ClientCompanyManage,
        /// <summary>
        /// 客户管理
        /// </summary>
        ClientInfoManage,
        /// <summary>
        /// 仓库管理
        /// </summary>
        WarehouseManage,
        /// <summary>
        /// 配送公司管理
        /// </summary>
        DistributionCompanyManage,

        /// <summary>
        /// 大包客户协议管理
        /// </summary>
        DBContractManage,

        /// <summary>
        /// 商业客户流向管理
        /// </summary>
        ClientInfoProductFlowManage,

        /// <summary>
        /// 部门货品销售计划和提成策略管理
        /// </summary>
        DeptProductSalesPlanAndBonusManage,
        /// <summary>
        /// 物流公司管理
        /// </summary>
        TransportCompanyManage,
        /// <summary>
        /// 现金管理
        /// </summary>
        MoneyManage,
        /// <summary>
        /// 报销类型管理
        /// </summary>
        ReimbursementTypeManage,
        /// <summary>
        /// 借款管理
        /// </summary>
        BorrowMoneyManage = 16,
        /// <summary>
        /// 固定资产管理 17
        /// </summary>
        FixedAssetsManage = 17,
        /// <summary>
        /// 报销类型管理
        /// </summary>
        ReimbursementManage = 18,
        /// <summary>
        /// 医院管理
        /// </summary>
        HospitalManage = 19,
        /// <summary>
        /// 货品信息管理
        /// </summary>
        ProductInfoManage = 20,

        /// <summary>
        /// 货品管理
        /// </summary>
        ProductManage,

        /// <summary>
        /// 货品定价管理
        /// </summary>
        ProductPriceManage,
        /// <summary>
        /// 库存货品信息
        /// </summary>
        InventoryHistoryManage,

        /// <summary>
        /// 人事管理
        /// </summary>
        HumanResourceManage = 30,

        /// <summary>
        /// 部门管理
        /// </summary>
        DepartmentManage = 31,

        /// <summary>
        /// 员工管理
        /// </summary>
        EmployeeManage,

        /// <summary>
        /// 工作流权限管理
        /// </summary>
        WorkflowPermissionManage,
        /// <summary>
        /// 用户组管理
        /// </summary>
        UserGroupManage,
        /// <summary>
        /// 工资结算管理
        /// </summary>
        SalarySettleManage = 35,

        /// <summary>
        /// 采购管理
        /// </summary>
        ProcureManage = 50,

        /// <summary>
        /// 采购订单管理
        /// </summary>
        ProcureOrderManage,

        /// <summary>
        /// 入库单管理
        /// </summary>
        StockInManage,
        /// <summary>
        /// 物流费用管理  入库
        /// </summary>
        TransportFeeManage_StockIn,

        /// <summary>
        /// 销售管理
        /// </summary>
        SalesManage = 60,

        /// <summary>
        /// 客户订单管理
        /// </summary>
        ClientOrderManage,

        /// <summary>
        /// 客户订单出库单管理
        /// </summary>
        ClientOrderStockOutManage,

        /// <summary>
        /// 大包订单申请单管理
        /// </summary>
        DBOrderRequestManage,

        /// <summary>
        /// 大包订单管理
        /// </summary>
        DBOrderManage,

        /// <summary>
        /// 大包订单出库单管理
        /// </summary>
        DBOrderStockOutManage,

        /// <summary>
        /// //担保收款
        /// </summary>
        GuaranteeReceiptManage = 66,
        /// <summary>
        /// 物流费用管理  出库
        /// </summary>
        TransportFeeManage_StockOut = 67,


        /// <summary>
        /// 返款管理
        /// </summary>
        RefundManage = 70,

        /// <summary>
        /// 供应商返款管理
        /// </summary>
        SupplierRefundsManage,

        /// <summary>
        /// 客户返款管理
        /// </summary>
        ClientRefundsManage,

        /// <summary>
        /// 厂家经理返款管理
        /// </summary>
        FactoryManagerRefundsManage,

        /// <summary>
        /// 供应商任务返款管理
        /// </summary>
        SupplierTaskRefundsManage,

        /// <summary>
        /// 客户任务奖励返款管理
        /// </summary>
        ClientTaskRefundsManage,

        /// <summary>
        /// 数据导入管理
        /// </summary>
        ImportDataManage = 80,

        /// <summary>
        /// 商业客户销售流向
        /// </summary>
        ClientSaleFlowData,

        /// <summary>
        /// 配送公司流向
        /// </summary>
        DCFlowData,

        /// <summary>
        /// 配送公司库存
        /// </summary>
        DCInventoryData,

        /// <summary>
        /// 采购订单导入
        /// </summary>
        ProcureOrderData = 84,
        /// <summary>
        /// 收货 入库单导入
        /// </summary>
        StockInData = 85,
        /// <summary>
        /// 客户订单导入
        /// </summary>
        ClientSaleApplicationData = 86,

        /// <summary>
        /// 提成与考核管理
        /// </summary>
        BonusAndAssessmentManage = 90,

        /// <summary>
        /// 大包客户提成结算管理
        /// </summary>
        DBClientSettleBonusManage,

        /// <summary>
        /// 报表管理
        /// </summary>
        ReportManage = 100,
        /// <summary>
        /// 采购订单报表
        /// </summary>
        ProcureOrderReportManage,
        /// <summary>
        /// 采购订单付款报表
        /// </summary>
        ProcureOrderPaymentReportManage,
        /// <summary>
        /// 销售订单报表
        /// </summary>
        ClientSaleAppReportManage,
        /// <summary>
        /// 出库明细报表
        /// </summary>
        StockOutDetailReportManage,
        /// <summary>
        /// 入库明细报表
        /// </summary>
        StockInDetailReportManage,
        /// <summary>
        /// 采购计划报表
        /// </summary>
        ProcurePlanReportManage,

        /// <summary>
        /// 现金流 报表
        /// </summary>
        CashFlowReportManage = 117,

        /// <summary>
        /// 库存汇总表
        /// </summary>
        InventorySummaryReportManage = 107,

        /// <summary>
        /// 配送公司流向结算表
        /// </summary>
        DCFlowSettlementReport = 110,

        /// <summary>
        /// 大包客户季度考核表
        /// </summary>
        DBClientQuarterlyAssessmentReport = 112,

        /// <summary>
        /// 配送公司库存核对表
        /// </summary>
        DCInventoryChecklistReport = 114,
        /// <summary>
        /// 大包客户结算表
        /// </summary>
        DBClientSettlementReportManage = 115,

        /// <summary>
        /// 供应商任务统计
        /// </summary>
        SupplierTaskReportManage = 116,


        /// <summary>
        /// 发票管理
        /// </summary>
        InvoiceManage = 150,
        /// <summary>
        /// 供应商发票管理
        /// </summary>
        SupplierInvoiceManage,
        /// <summary>
        /// 客户发票管理
        /// </summary>
        ClientInvoiceManage,
        /// <summary>
        /// 大包发票管理
        /// </summary>
        DBClientInvoiceManage,

        /// <summary>
        /// 供应商发票结算管理
        /// </summary>
        SupplierInvoiceSettlementManage,

        /// <summary>
        /// 客户发票结算管理
        /// </summary>
        ClientInvoiceSettlementManage,

        /// <summary>
        /// 大包收款管理（按发票收款）
        /// </summary>
        DBClientInvoiceSettlementManage,

        /// <summary>
        /// 挂靠发票结算管理
        /// </summary>
        ClientAttachedInvoiceSettlementManage,

        /// <summary>
        /// 保证金管理
        /// </summary>
        CautionMoneyManage = 200,
        /// <summary>
        /// 供应商保证金管理
        /// </summary>
        SupplierCautionMoneyManage = 201,
        /// <summary>
        /// 供应商保证金支出申请管理
        /// </summary>
        SupplierCautionMoneyApplyManage = 202,
        /// <summary>
        /// 客户商保证金管理
        /// </summary>
        ClientCautionMoneyManage = 203,
        /// <summary>
        /// 客户商保证金退回申请管理
        /// </summary>
        ClientCautionMoneyReturnApplyManage = 204,





    }
}
