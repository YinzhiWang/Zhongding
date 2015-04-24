using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDBClientInvoiceSettlementDetail : UIBase
    {
        public int DBClientInvoiceSettlementID { get; set; }
        public int DBClientInvoiceID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string CompanyName { get; set; }
        public string DistributionCompanyName { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal ToBeReceiveAmount { get; set; }
        public decimal CurrentReceiveAmount { get; set; }

    }
}
