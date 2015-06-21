using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UISalarySettle : UIWorkflowBase
    {
        public string CreatedByUserName { get; set; }

        public string DepartmentName { get; set; }

        public DateTime SettleDate { get; set; }
    }
}
