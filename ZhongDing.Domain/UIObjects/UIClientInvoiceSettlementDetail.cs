using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientInvoiceSettlementDetail : UIBase
    {
        public int ClientInvoiceSettlementID { get; set; }
        public int ClientCompanyID { get; set; }
        public int ClientInvoiceID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalInvoiceAmount { get; set; }
        public decimal? ClientTaxHighRatio { get; set; }
        public decimal? ClientTaxLowRatio { get; set; }
        public decimal? ClientTaxDeductionRatio { get; set; }
        public decimal? HighRatioAmount { get; set; }
        public decimal? LowRatioAmount { get; set; }
        public decimal? DeductionRatioAmount { get; set; }
        public decimal PayAmount { get; set; }
        public string ClientCompanyName { get; set; }
        /// <summary>
        /// 是否被选中
        /// </summary>
        /// <value><c>true</c> if this instance is checked; otherwise, <c>false</c>.</value>
        public bool IsChecked { get; set; }
        /// <summary>
        /// 是否包含平进平出发票
        /// </summary>
        /// <value><c>true</c> if this instance is contain deduction invoice; otherwise, <c>false</c>.</value>
        public bool IsContainDeductionInvoice { get; set; }

    }
}
