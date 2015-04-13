using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchDBClientSettlement : UISearchWorkflowBase
    {
        public DateTime? SettlementDate { get; set; }

        public int HospitalTypeID { get; set; }
    }
}
