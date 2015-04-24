using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchProcurePlanReport : UISearchBase
    {
        public int? WarehouseID { get; set; }

        public string ProductName { get; set; }
    }
}
