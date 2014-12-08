using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchExtension
    {
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

    }
}
