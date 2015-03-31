using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIProcureOrderReport
    {
        public int ID { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderCode { get; set; }
        public string SupplierName { get; set; }
        public string WarehouseName { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string CategoryName { get; set; }
        public string Specification { get; set; }
        public string UnitName { get; set; }
        public int ProcureCount { get; set; }
        public decimal ProcurePrice { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
