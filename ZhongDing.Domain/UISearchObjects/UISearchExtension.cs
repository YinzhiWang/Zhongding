using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    /// <summary>
    /// 类：查询扩展参数
    /// </summary>
    public class UISearchExtension
    {
        /// <summary>
        /// 账套ID
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DepartmentID { get; set; }

        /// <summary>
        /// 部门类型ID
        /// </summary>
        public int DepartmentTypeID { get; set; }

        /// <summary>
        /// 货品ID
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// 货品类别ID
        /// </summary>
        public int ProductCategoryID { get; set; }

        /// <summary>
        /// 货品类型ID列表
        /// </summary>
        public IList<int> ProductCategoryIDs { get; set; }

        /// <summary>
        /// 工作流ID
        /// </summary>
        public int WorkflowID { get; set; }

        /// <summary>
        /// 销售价格类型ID：高价、低价
        /// </summary>
        public int SaleTypeID { get; set; }

        /// <summary>
        /// 所有者类型ID
        /// </summary>
        public int OwnerTypeID { get; set; }

        /// <summary>
        /// 银行账号类型ID
        /// </summary>
        public int AccountTypeID { get; set; }

        /// <summary>
        /// 客户名ID
        /// </summary>
        public int ClientUserID { get; set; }

    }
}
