using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchStockOutDetail : UISearchBase
    {
        public int StockOutID { get; set; }

        public int WarehouseID { get; set; }

        public int ProductID { get; set; }

        public int DistributionCompanyID { get; set; }

        public int? ClientCompanyID { get; set; }
    }
}
