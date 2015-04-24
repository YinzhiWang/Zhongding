using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchDBClientInvoiceSettlementDetail : UISearchBase
    {
        public int DBClientInvoiceSettlementID { get; set; }

        public int CompanyID { get; set; }

        public int DistributionCompanyID { get; set; }

        public string InvoiceNumber { get; set; }

        public bool OnlyIncludeChecked { get; set; }
    }
}
