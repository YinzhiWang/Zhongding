using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDBClientSettlement : UIWorkflowBase
    {
        public DateTime SettlementDate { get; set; }

        public string HospitalType { get; set; }
    }
}
