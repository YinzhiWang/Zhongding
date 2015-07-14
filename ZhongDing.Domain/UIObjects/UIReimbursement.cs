using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIReimbursement :  UIWorkflowBase
    {
        public string DepartmentName { get; set; }

        public DateTime ApplyDate { get; set; }
    }
}
