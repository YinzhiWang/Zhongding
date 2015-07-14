using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIReimbursementDetail : UIBase
    {
        public string ReimbursementType { get; set; }

        public string ReimbursementTypeChild { get; set; }

        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public decimal Amount { get; set; }
        public Nullable<int> Quantity { get; set; }
    }
}
