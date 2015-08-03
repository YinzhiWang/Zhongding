using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{



    public enum EPermission : int
    {
        NULL = 0,
        /// <summary>
        /// 账套管理 1
        /// </summary>
        CompanyManagement = 1,
        /// <summary>
        /// 银行账号管理 2
        /// </summary>
        BankAccountManagement = 2,
        /// <summary>
        /// //3	供应商管理
        /// </summary>
        SupplierManagement = 3,
        /// <summary>
        /// //4	商业单位管理
        /// </summary>
        ClientCompanyManagement = 4,
        /// <summary>
        /// //5	客户管理
        /// </summary>
        ClientInfoManagement = 5,
        /// <summary>
        /// //6	仓库管理
        /// </summary>
        WarehouseManagement = 6,
        /// <summary>
        /// //7	配送公司管理
        /// </summary>
        DistributionCompanyManagement = 7,
        /// <summary>
        /// //8	大包客户协议管理
        /// </summary>
        DBContractManagement = 8,
        /// <summary>
        /// //9	商业客户流向管理
        /// </summary>
        ClientInfoProductFlowManagement = 9,
        /// <summary>
        /// //10	部门销售计划及提成策略
        /// </summary>
        DeptProductSalesPlanAndBonusManagement = 10,
        /// <summary>
        /// //11	物流公司管理
        /// </summary>
        TransportCompanyManagement = 11,
        /// <summary>
        /// //12	货品管理
        /// </summary>
        ProductManagement = 12,
        /// <summary>
        /// //13	货品定价管理
        /// </summary>
        ProductPriceManagement = 13,
        /// <summary>
        /// //14	库存货品信息
        /// </summary>
        InventoryHistoryManagement = 14,
        /// <summary>
        /// //15	部门管理
        /// </summary>
        DepartmentManagement = 15,
        /// <summary>
        /// //16	员工管理
        /// </summary>
        EmployeeManagement = 16,
        /// <summary>
        /// //17	权限管理
        /// </summary>
        PermissionManagement = 17,


        /// <summary>
        /// //18	报销类型管理
        /// </summary>
        ReimbursementTypeManagement = 18,
        /// <summary>
        /// //19	物流费用管理
        /// </summary>
        TransportFeeManagement = 19,



        //20	担保收款

        /// <summary>
        /// //21	数据导入
        /// </summary>
        DataImport = 21,


        //22	部门月提成

        /// <summary>
        /// //23	采购订单报表
        /// </summary>
        ProcureOrderReportManagement = 23,
        /// <summary>
        /// //24	采购付款明细表
        /// </summary>
        ProcureOrderApplicationPaymentReportManagement = 24,
        /// <summary>
        /// //25	销售订单报表
        /// </summary>
        ClientSaleAppReportManagement = 25,
        /// <summary>
        /// //26	出库明细表
        /// </summary>
        StockOutDetailReportManagement = 26,
        /// <summary>
        /// //27	入库明细表
        /// </summary>
        StockInDetailReportManagement = 27,
        /// <summary>
        /// //28	采购计划表
        /// </summary>
        ProcurePlanReportManagement = 28,
        /// <summary>
        /// //29	库存汇总表
        /// </summary>
        InventorySummaryReportManagement = 29,
        /// <summary>
        /// //30	配送公司流向结算表
        /// </summary>
        DCFlowSettlementReport = 30,
        /// <summary>
        /// //31	大包客户季度考核表
        /// </summary>
        DBClientQuarterlyAssessmentReport = 31,
        /// <summary>
        /// //32	配送公司库存核对表
        /// </summary>
        DCInventoryChecklistReport = 32,
        /// <summary>
        /// //33	发票管理
        /// </summary>
        InvoiceManagement = 33,
        /// <summary>
        /// //34 大包客户结算表
        /// </summary>
        DBClientSettlementReportManagement = 34,
        /// <summary>
        /// //35 供应商任务表
        /// </summary>
        SupplierTaskReportManagement = 35,
        /// <summary>
        /// // 36 现金管理
        /// </summary>
        MoneyManagement = 36,
        /// <summary>
        /// //37 借款管理
        /// </summary>
        BorrowMoneyManagement = 37,
        /// <summary>
        /// 38 固定资产管理
        /// </summary>
        FixedAssetsManagement = 38,
        /// <summary>
        /// 担保收款管理
        /// </summary>
        GuaranteeReceiptManagement = 39,
        /// <summary>
        /// 首页-库存提醒
        /// </summary>
        InventoryReminder = 40,
        /// <summary>
        /// 首页-保证金提醒
        /// </summary>
        CautionMoneyReminder = 41,
        /// <summary>
        /// 首页-货品资料过期提醒
        /// </summary>
        ProductInfoExpiredReminder = 42,
        /// <summary>
        /// 首页-客户资料过期提醒
        /// </summary>
        ClientInfoReminder = 43,
        /// <summary>
        /// 首页-供应商资料过期提醒
        /// </summary>
        SupplierInfoReminder = 44,
        /// <summary>
        /// 首页-货品过期提醒
        /// </summary>
        ProductExpiredReminder = 45,
        /// <summary>
        /// 首页-借款到期提醒
        /// </summary>
        BorrowMoneyExpiredReminder = 46,
        /// <summary>
        /// 首页-担保收款到期提醒
        /// </summary>
        GuaranteeReceiptExpiredReminder = 47,
        /// <summary>
        /// 每月现金流量情况表
        /// </summary>
        CashFlowReportManagement = 48
    }
}
