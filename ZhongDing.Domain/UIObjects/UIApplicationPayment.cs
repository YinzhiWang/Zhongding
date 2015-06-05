using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIApplicationPayment : UIBase
    {
        public int? FromBankAccountID { get; set; }

        public string FromAccount { get; set; }

        public int? ToBankAccountID { get; set; }

        public string ToAccount { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Fee { get; set; }

        public DateTime? PayDate { get; set; }

        public int PaymentTypeID { get; set; }

        public string PaymentType { get; set; }

        public decimal? OutAmount { get; set; }

        public decimal? InAmount { get; set; }

        public decimal Balance { get; set; }
    }
}
