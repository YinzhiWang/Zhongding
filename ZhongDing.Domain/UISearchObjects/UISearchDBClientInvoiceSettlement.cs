using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchDBClientInvoiceSettlement : UISearchBase
    {
        public int CompanyID { get; set; }

        public int DistributionCompanyID { get; set; }

        public string InvoiceNumber { get; set; }

        public bool ExcludeCanceled { get; set; }
    }
}
