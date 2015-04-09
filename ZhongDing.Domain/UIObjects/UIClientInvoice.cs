using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientInvoice
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        
        public System.DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Amount { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedOn { get; set; }

 
        public string CompanyName { get; set; }

        public string StockOutCode { get; set; }

        public DateTime? StockOutOutDate { get; set; }

        public string ProductName { get; set; }

        public int OutQty { get; set; }

        public decimal TotalSalesAmount { get; set; }

        public decimal? TaxAmount { get; set; }

        public int ProductID { get; set; }

        public int ClientInvoiceDetailID { get; set; }

        public int? TaxQty { get; set; }
    }
}
