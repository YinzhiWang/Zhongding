using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIStockInDetailReport
    {
        public int ID { get; set; }
        public DateTime? EntryDate { get; set; }
        public string StockInCode { get; set; }
        public string OrderCode { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string WarehouseName { get; set;  }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string CategoryName { get; set; }
        public string Specification { get; set; }
        public string UnitName { get; set; }
        public decimal ProcurePrice { get; set; }
        public int InQty { get; set; }
        public int NumberOfPackages { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
