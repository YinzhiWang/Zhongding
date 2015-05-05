using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchDCInventoryData : UISearchBase
    {
        public int DistributionCompanyID { get; set; }

        public int ProductID { get; set; }

        public DateTime? SettlementDate { get; set; }

        public int ImportFileLogID { get; set; }

    }
}
