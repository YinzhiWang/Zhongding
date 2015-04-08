using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UISupplierInvoice
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int SupplierID { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Amount { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedOn { get; set; }


        public string SupplierName { get; set; }

        public string CompanyName { get; set; }

        public string ProcureOrderApplicationOrderCode { get; set; }

        public DateTime ProcureOrderApplicationOrderDate { get; set; }

        public string ProductName { get; set; }

        public int ProcureCount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal? TaxAmount { get; set; }

        public int ProductID { get; set; }

        public int SupplierInvoiceDetailID { get; set; }
    }
}
