using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchDBClientInvoice : UISearchBase
    {
        public string InvoiceNumber { get; set; }

        public int CompanyID { get; set; }

        public int? ClientCompanyID { get; set; }

        public int? DistributionCompanyID { get; set; }
    }
}
