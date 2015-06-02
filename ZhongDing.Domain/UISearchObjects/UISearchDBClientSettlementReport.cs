using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchDBClientSettlementReport : UISearchWorkflowBase
    {
        public int DBClientSettlementID { get; set; }

        public int? WorkflowStatusID { get; set; }

        public DateTime? SettlementDate { get; set; }

        public int? ClientUserID { get; set; }
    }
}
