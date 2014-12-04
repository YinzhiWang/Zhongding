using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchDeptProductRecord : UISearchBase
    {
        public int DepartmentID { get; set; }

        public int ProductID { get; set; }

        public int Year { get; set; }
    }
}
