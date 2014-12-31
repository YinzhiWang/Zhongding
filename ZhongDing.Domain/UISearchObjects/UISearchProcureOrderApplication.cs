using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchProcureOrderApplication : UISearchWorkflowBase
    {
        public int CompanyID { get; set; }

        public string OrderCode { get; set; }

        public int SupplierID { get; set; }

    }
}
