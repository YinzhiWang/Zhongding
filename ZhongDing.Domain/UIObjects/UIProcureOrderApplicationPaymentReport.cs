using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIProcureOrderApplicationPaymentReport
    {
        public int ID { get; set; }
        public DateTime? PayDate { get; set; }
        public int ProcureOrderApplicationID { get; set; }
        public string OrderCode { get; set; }
        public decimal Amount { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
    }
}
