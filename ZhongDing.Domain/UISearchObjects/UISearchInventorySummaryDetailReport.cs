using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchInventorySummaryDetailReport : UISearchBase
    {
        public int? WarehouseID { get; set; }

        public int? ProductID { get; set; }

        public int? ProductSpecificationID { get; set; }

        public string BatchNumber { get; set; }

        public string LicenseNumber { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
