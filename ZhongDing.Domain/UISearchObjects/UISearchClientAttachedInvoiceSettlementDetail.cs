using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchClientAttachedInvoiceSettlementDetail : UISearchBase
    {
        public int CompanyID { get; set; }

        public int ClientAttachedInvoiceSettlementID { get; set; }

        public int ClientUserID { get; set; }

        public int ClientCompanyID { get; set; }

        public bool OnlyIncludeChecked { get; set; }
    }
}
