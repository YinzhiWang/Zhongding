using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchStockInDetail : UISearchBase
    {
        public int StockInID { get; set; }

        public int WarehouseID { get; set; }

        public int ProductID { get; set; }

        public int ProductSpecificationID { get; set; }

        public int SupplierID { get; set; }

        /// <summary>
        /// 是否排除过期的货品
        /// </summary>
        public bool ExcludeExpired { get; set; }
    }
}
