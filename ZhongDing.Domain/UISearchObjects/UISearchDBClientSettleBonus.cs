using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchDBClientSettleBonus : UISearchBase
    {
        public int DBClientSettlementID { get; set; }

        public string ClientUserName { get; set; }

        public bool OnlyIncludeNeedSettlement { get; set; }
    }
}
