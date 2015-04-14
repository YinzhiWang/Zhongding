using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDBClientSettlementAppPayment
    {
        /// <summary>
        /// 转账日期
        /// </summary>
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// 转出账号ID.
        /// </summary>
        public int? FromBankAccountID { get; set; }

        /// <summary>
        /// 转出账号
        /// </summary>
        public string FromAccount { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Fee { get; set; }

    }
}
