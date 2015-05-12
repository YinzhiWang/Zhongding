using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIInventorySummaryDetailReport : UIBase
    {
        public DateTime? EntryOrOutDate { get; set; }

        public string Type { get; set; }

        public string OrderCode { get; set; }

        public string StockInOrOutCode { get; set; }

        public int WarehouseID { get; set; }

        public int ProductID { get; set; }

        public int ProductSpecificationID { get; set; }

        public string BatchNumber { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string LicenseNumber { get; set; }

        public decimal Price { get; set; }

        public int? InQty { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public string UnitName { get; set; }

        public int NumberInLargePackage { get; set; }

        public int? OutQty { get; set; }

        public decimal? InNumberOfPackages { get; set; }

        public decimal? OutNumberOfPackages { get; set; }
    }
}
