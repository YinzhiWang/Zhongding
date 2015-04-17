using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UISupplierInvoiceSettlementDetail : UIBase
    {
        public int SupplierInvoiceSettlementID { get; set; }
        public int SupplierID { get; set; }
        public int SupplierInvoiceID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PayAmount { get; set; }
        public string SupplierName { get; set; }
    }
}
