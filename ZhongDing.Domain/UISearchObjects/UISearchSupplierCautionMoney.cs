using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchSupplierCautionMoney : UISearchBase
    {
        public int WorkflowStatusID { get; set; }

        public int[] WorkflowStatusIDs { get; set; }

        public bool NeedStatistics { get; set; }

        public string SupplierName { get; set; }

        public string ProductName { get; set; }
    }
}
