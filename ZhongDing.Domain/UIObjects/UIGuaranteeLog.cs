using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIGuaranteeLog : UIBase
    {
        public int ClientSaleApplicationID { get; set; }
        public Nullable<decimal> GuaranteeAmount { get; set; }
        public Nullable<int> Guaranteeby { get; set; }
        public Nullable<System.DateTime> GuaranteeExpirationDate { get; set; }
        public int GuaranteeReceiptID { get; set; }
        public System.DateTime GuaranteeReceiptDate { get; set; }

        public string OrderCode { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
