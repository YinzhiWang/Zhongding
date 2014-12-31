using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDaBaoApplication : UIWorkflowBase
    {
        public string OrderCode { get; set; }

        public DateTime OrderDate { get; set; }

        public string DepartmentName { get; set; }

        public string DistributionCompany { get; set; }

    }
}
