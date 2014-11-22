using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchProduct : UISearchBase
    {
        public int CompanyID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }
}
