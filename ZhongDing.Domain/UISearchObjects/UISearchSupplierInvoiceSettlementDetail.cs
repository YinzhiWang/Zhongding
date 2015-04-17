using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchSupplierInvoiceSettlementDetail : UISearchBase
    {
        public int SupplierInvoiceSettlementID { get; set; }

        public int CompanyID { get; set; }

        public bool IsNew { get; set; }
    }
}
