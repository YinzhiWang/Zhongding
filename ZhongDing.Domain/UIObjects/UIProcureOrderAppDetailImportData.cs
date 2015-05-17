using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIProcureOrderAppDetailImportData : UIBase
    {
        public string OrderCode { get; set; }
        public string SupplierName { get; set; }
        public System.DateTime OrderDate { get; set; }
        public System.DateTime EstDeliveryDate { get; set; }

        public int ProcureOrderApplicationImportDataID { get; set; }
        public int ProcureOrderAppDetailID { get; set; }
        public string WarehouseName { get; set; }
        public string ProductName { get; set; }
        public string Specification { get; set; }
        public Nullable<int> ProcureCount { get; set; }
        public Nullable<decimal> ProcurePrice { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
    }
}
