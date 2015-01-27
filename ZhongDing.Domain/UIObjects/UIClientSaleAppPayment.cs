using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientSaleAppPayment : UIBase
    {
        /// <summary>
        /// 收款方式ID
        /// </summary>
        public int PaymentMethodID { get; set; }
        /// <summary>
        /// 收款方式
        /// </summary>
        public string PaymentMethod { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Fee { get; set; }

        /// <summary>
        /// 到账日期
        /// </summary>
        public DateTime PayDate { get; set; }
    }
}
