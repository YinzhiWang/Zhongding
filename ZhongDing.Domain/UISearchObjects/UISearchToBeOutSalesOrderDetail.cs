using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchToBeOutSalesOrderDetail : UISearchBase
    {
        public int SalesOrderApplicationID { get; set; }

        public IEnumerable<int> SaleOrderTypeIDs { get; set; }

        public int DistributionCompanyID { get; set; }

        /// <summary>
        /// 需排除的IDs
        /// </summary>
        public IEnumerable<int> ExcludeIDs { get; set; }

    }
}
