using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    /// <summary>
    /// 类：仓库UI实体类
    /// </summary>
    public class UIWarehouse : UIBase
    {
        /// <summary>
        /// 仓库编号
        /// </summary>
        /// <value>The warehouse code.</value>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// 仓库售价类型
        /// </summary>
        /// <value>The name of the sale type.</value>
        public string SaleTypeName { get; set; }
    }
}
