using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientSaleAppReport
    {
        public int ID { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderCode { get; set; }
        public string ClientName { get; set; }
        public string ClientCompanyName { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string CategoryName { get; set; }
        public string Specification { get; set; }
        public string UnitName { get; set; }
        public int Count { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal TotalSalesAmount { get; set; }
    }
}
