using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchReimbursement : UISearchWorkflowBase
    {
        public int? DepartmentID { get; set; }

        public string ApplyUser { get; set; }
    }
}
