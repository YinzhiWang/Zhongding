using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UISupplierDeduction : UIBase
    {
        public int SupplierID { get; set; }

        public int SupplierRefundAppID { get; set; }

        public DateTime? DeductedDate { get; set; }

        public string SupplierName { get; set; }

        public decimal Amount { get; set; }

    }
}
