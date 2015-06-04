using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchSupplierTaskReport : UISearchWorkflowBase
    {
        public DateTime? Date { get; set; }

        public int? SupplierID { get; set; }

        public int? CompanyID { get; set; }
    }
}
