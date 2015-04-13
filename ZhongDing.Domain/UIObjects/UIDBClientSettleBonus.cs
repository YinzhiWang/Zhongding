using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDBClientSettleBonus : UIBase
    {
        public string ClientUserName { get; set; }

        public string Hospitals { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public DateTime SettlementDate { get; set; }

        public decimal? BonusAmount { get; set; }

        public decimal? PerformanceAmount { get; set; }

        public bool? IsNeedSettlement { get; set; }

        public decimal? TotalPayAmount { get; set; }

        public IEnumerable<int> HospitalIDs { get; set; }

        public bool? IsSettled { get; set; }

        public bool? IsManualSettled { get; set; }

    }
}
