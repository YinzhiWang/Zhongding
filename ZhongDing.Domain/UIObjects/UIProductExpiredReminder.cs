using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIProductExpiredReminder : UIBase
    {
        public int WarehouseID { get; set; }
        public int ProductID { get; set; }
        public int ProductSpecificationID { get; set; }
        public string LicenseNumber { get; set; }
        public string BatchNumber { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public decimal ProcurePrice { get; set; }
        public int InQty { get; set; }
        public int OutQty { get; set; }
        public int BalanceQty { get; set; }
        public DateTime StatDate { get; set; }

        public string WarehouseName { get; set; }

        public string ProductName { get; set; }

        public decimal? NumberOfPackages { get; set; }

        public string Specification { get; set; }

        public string UnitName { get; set; }

        public decimal Amount { get; set; }

        public int NumberInLargePackage { get; set; }

        public int NewInQty { get; set; }

        public int NewOutQty { get; set; }
    }
}
