using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchClientCautionMoney : UISearchBase
    {
        public int WorkflowStatusID { get; set; }

        public string ClientName { get; set; }

        public string ProductName { get; set; }

        public int? DepartmentID { get; set; }
    }
}
