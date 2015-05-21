using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchClientCautionMoneyReturnApplication : UISearchWorkflowBase
    {
        public int[] WorkflowStatusIDs { get; set; }

        public int WorkflowStatusID { get; set; }

        public int? ClientCautionMoneyID { get; set; }

        public string ClientName { get; set; }

        public string ProductName { get; set; }

        public int? DepartmentID { get; set; }

        public IEnumerable<int> UnIncludeWorkflowStatusIDs { get; set; }
    }
}
