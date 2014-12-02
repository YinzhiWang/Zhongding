using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchDBContract : UISearchBase
    {
        public string ContractCode { get; set; }
        public int ClientUserID { get; set; }
        public string ProductName { get; set; }
        public int DepartmentID { get; set; }
    }
}
