using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIStockIn : UIWorkflowBase
    {
        public string Code { get; set; }

        public DateTime? EntryDate { get; set; }

        public string SupplierName { get; set; }

    }
}
