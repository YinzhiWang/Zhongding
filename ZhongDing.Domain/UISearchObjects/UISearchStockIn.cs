using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchStockIn : UISearchBase
    {
        public int CompanyID { get; set; }

        public string Code { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int SupplierID { get; set; }

        public int WorkflowStatusID { get; set; }

        public IEnumerable<int> IncludeWorkflowStatusIDs { get; set; }
    }
}
