using System;
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

    }
}
