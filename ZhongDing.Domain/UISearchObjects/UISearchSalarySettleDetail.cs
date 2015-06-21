using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchSalarySettleDetail : UISearchBase
    {
        public int? DepartmentID { get; set; }

        public DateTime SettleDate { get; set; }

        public int? SalarySettleID { get; set; }
    }
}
