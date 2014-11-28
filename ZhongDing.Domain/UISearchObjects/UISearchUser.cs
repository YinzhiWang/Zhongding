using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchUser : UISearchBase
    {
        public Guid? AspnetUserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int DepartmentID { get; set; }
    }
}
