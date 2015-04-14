using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDBClientSettleBonusPayment : UIBase
    {
        public string ClientUserName { get; set; }

        public string ClientDBBankAccount { get; set; }

        public DateTime SettlementDate { get; set; }

        public decimal? TotalPayAmount { get; set; }

        public decimal? Fee { get; set; }

        public bool? IsNeedSettlement { get; set; }

        public bool? IsSettled { get; set; }

    }
}
