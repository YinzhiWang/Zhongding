using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchWarehouse : UISearchBase
    {
        /// <summary>
        /// 账套ID
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID { get; set; }

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
        /// 仓库售价类型ID
        /// </summary>
        /// <value>The name of the sale type.</value>
        public int SaleTypeID { get; set; }
    }
}
