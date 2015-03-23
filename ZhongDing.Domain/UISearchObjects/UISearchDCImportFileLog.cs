using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchDCImportFileLog : UISearchImportFileLog
    {
        public int DistributionCompanyID { get; set; }

        public DateTime? SettlementDate { get; set; }
    }
}
