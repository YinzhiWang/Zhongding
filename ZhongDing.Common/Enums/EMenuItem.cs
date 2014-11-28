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
        EmployeeManage

    }
}
