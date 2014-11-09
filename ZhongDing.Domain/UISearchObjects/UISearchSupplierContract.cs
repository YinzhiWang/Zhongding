using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchSupplierContract : UISearchBase
    {
        public int SupplierID { get; set; }
        public int ProductID { get; set; }
        public string ContractCode { get; set; }
    }
}
