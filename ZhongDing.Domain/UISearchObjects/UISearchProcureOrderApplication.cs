using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchProcureOrderApplication : UISearchBase
    {
        public int CompanyID { get; set; }

        public string OrderCode { get; set; }

        public DateTime? OrderBeginDate { get; set; }

        public DateTime? OrderEndDate { get; set; }

        public int SupplierID { get; set; }

        public int WorkflowStatusID { get; set; }

        public IEnumerable<int> IncludeWorkflowStatusIDs { get; set; }

    }
}
