using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIStockIn : UIBase
    {
        public string Code { get; set; }

        public DateTime? EntryDate { get; set; }

        public string SupplierName { get; set; }

        public int WorkflowStatusID { get; set; }

        public string WorkflowStatus { get; set; }
    }
}
