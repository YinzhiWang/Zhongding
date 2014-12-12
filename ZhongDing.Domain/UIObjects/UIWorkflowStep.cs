using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIWorkflowStep : UIBase
    {
        public int WorkflowID { get; set; }

        public string WorkfolwName { get; set; }

        public string StepName { get; set; }

        public string StepUserNames { get; set; }

        public IEnumerable<int> StepUserIDs { get; set; }
    }
}
