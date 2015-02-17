using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchSupplierRefundApp : UISearchWorkflowBase
    {
        public int WorkflowID { get; set; }

        public int CompanyID { get; set; }

        public int SupplierID { get; set; }

        public int WarehouseID { get; set; }

        public int ProductID { get; set; }

        public int ProductSpecificationID { get; set; }

    }
}
