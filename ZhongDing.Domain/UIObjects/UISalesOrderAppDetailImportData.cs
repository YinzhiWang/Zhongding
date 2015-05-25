using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UISalesOrderAppDetailImportData : UIBase
    {
        public int SalesOrderApplicationImportDataID { get; set; }
        public int SalesOrderAppDetailID { get; set; }
        public string ProductName { get; set; }
        public string ProductSpecification { get; set; }
        public string WarehouseName { get; set; }
        public string UnitOfMeasurement { get; set; }
        public int Count { get; set; }
        public decimal SalesPrice { get; set; }
        public Nullable<decimal> InvoicePrice { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public Nullable<int> GiftCount { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }

        public string OrderCode { get; set; }

        public DateTime? OrderDate { get; set; }

        public string SaleOrderType { get; set; }

        public string ClientUserName { get; set; }

        public string ClientCompanyName { get; set; }
    }
}
