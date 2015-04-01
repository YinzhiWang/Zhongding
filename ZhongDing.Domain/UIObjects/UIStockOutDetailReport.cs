using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIStockOutDetailReport
    {
        public int ID { get; set; }
        public DateTime? OutDate { get; set;  }
        public string StockOutCode { get; set; }
        public string SalesOrderApplicationOrderCode { get; set; }
        public string ClientName { get; set; }
        public string ClientCompanyName { get; set; }
        public string WarehouseName { get; set; }
        public string CategoryName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Specification { get; set; }
        public string UnitName { get; set; }
        public int OutQty { get; set; }
        public int NumberOfPackages { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal TotalSalesAmount { get; set; }
    }
}
