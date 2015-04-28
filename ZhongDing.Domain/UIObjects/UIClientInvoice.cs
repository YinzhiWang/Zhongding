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
        
        public DateTime InvoiceDate { get; set; }

        public string SaleOrderType { get; set; }

        public string CompanyName { get; set; }

        public string ClientCompanyName { get; set; }

        public string ClientUserName { get; set; }

        public string InvoiceNumber { get; set; }

        public string StockOutCode { get; set; }

        public string ProductName { get; set; }

        public int InvoiceQty { get; set; }

        public decimal TotalSalesAmount { get; set; }

        public decimal InvoiceAmount { get; set; }

        public string TransportNumber { get; set; }
    }
}
