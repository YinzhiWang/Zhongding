using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchDepartment: UISearchBase
    {
        public string DepartmentName { get; set; }

        public int DepartmentTypeID { get; set; }
    }
}
