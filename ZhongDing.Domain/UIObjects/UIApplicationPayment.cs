using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIApplicationPayment : UIBase
    {
        public int FromBankAccountID { get; set; }

        public string FromAccount { get; set; }

        public int ToBankAccountID { get; set; }

        public string ToAccount { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Fee { get; set; }
    }
}
