using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchSupplierInvoice : UISearchBase
    {
        public string InvoiceNumber { get; set; }

        public int? SupplierID { get; set; }

        public int CompanyID { get; set; }

        public bool IsGroupByProduct { get; set; }
    }
}
