using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDBClientInvoiceSettlement : UIBase
    {
        public System.DateTime ReceiveDate { get; set; }

        /// <summary>
        /// 开票单位
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 收票单位
        /// </summary>
        public string DistributionCompanyName { get; set; }

        public string InvoiceNumbers { get; set; }

        public IEnumerable<string> InvoiceNumberArray { get; set; }

        public decimal TotalReceiveAmount { get; set; }

        public DateTime? ConfirmDate { get; set; }

    }
}
