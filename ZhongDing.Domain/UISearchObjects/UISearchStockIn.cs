using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchStockIn : UISearchWorkflowBase
    {
        public int CompanyID { get; set; }

        public string Code { get; set; }

        public int SupplierID { get; set; }
    }
}
