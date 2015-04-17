using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientInvoiceSettlement : UIWorkflowBase
    {
        public DateTime SettlementDate { get; set; }

        public string ClientCompanyName { get; set; }

        public string InvoiceNumbers { get; set; }

        public IEnumerable<string> InvoiceNumberArray { get; set; }

        public decimal TotalInvoiceAmount { get; set; }

        public decimal TotalPayAmount { get; set; }

    }
}
