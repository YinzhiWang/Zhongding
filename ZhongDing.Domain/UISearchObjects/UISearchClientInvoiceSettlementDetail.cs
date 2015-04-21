using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchClientInvoiceSettlementDetail : UISearchBase
    {
        public int ClientInvoiceSettlementID { get; set; }

        public int ClientCompanyID { get; set; }

        public int CompanyID { get; set; }

        public bool OnlyIncludeChecked { get; set; }

    }
}
