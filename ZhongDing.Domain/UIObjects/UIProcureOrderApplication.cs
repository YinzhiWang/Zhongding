using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIProcureOrderApplication : UIWorkflowBase
    {
        public string OrderCode { get; set; }

        public string SupplierName { get; set; }

        public bool IsStop { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? EstDeliveryDate { get; set; }

    }
}
