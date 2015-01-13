using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchSalesOrderAppDetail : UISearchBase
    {
        public int SalesOrderApplicationID { get; set; }

        public IEnumerable<int> SaleOrderTypeIDs { get; set; }

        public int DistributionCompanyID { get; set; }

        public int WarehouseID { get; set; }

        /// <summary>
        /// 需包含的IDs（SalesOrderAppDetail）
        /// </summary>
        public IEnumerable<int> IncludeIDs { get; set; }

        /// <summary>
        /// 需排除的IDs（SalesOrderAppDetail）
        /// </summary>
        public IEnumerable<int> ExcludeIDs { get; set; }
    }
}
