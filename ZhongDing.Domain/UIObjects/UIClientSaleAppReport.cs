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

        public int AlreadyOutQty { get; set; }
        public decimal AlreadyOutQtySalesPricePrice { get; set; }
        public int NumberInLargePackage { get; set; }
        public decimal AlreadyOutNumberOfPackages { get; set; }

        public int StopOutQty { get; set; }
        public decimal StopOutQtySalesPricePrice { get; set; }
        public decimal StopOutNumberOfPackages { get; set; }

        public int NotOutQty { get; set; }
        public decimal NotOutQtySalesPricePrice { get; set; }
        public decimal NotOutNumberOfPackages { get; set; }

        public bool SalesOrderApplicationIsStop { get; set; }
    }
}
