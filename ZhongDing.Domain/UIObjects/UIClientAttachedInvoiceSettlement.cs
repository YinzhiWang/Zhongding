using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientAttachedInvoiceSettlement : UIWorkflowBase
    {
        public DateTime? SettlementDate { get; set; }

        public string ClientUserName { get; set; }

        public string ClientCompanyName { get; set; }

        public string InvoiceNumbers { get; set; }

        public IEnumerable<string> InvoiceNumberArray { get; set; }

        public decimal ReceiveAmount { get; set; }

        public decimal? OtherCostAmount { get; set; }

        public decimal? TotalRefundAmount { get; set; }

        public DateTime? ConfirmDate { get; set; }

        public int? PaidBy { get; set; }
    }
}
