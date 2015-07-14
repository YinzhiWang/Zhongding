using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UITransportFee
    {
        public int ID { get; set; }
        public string TransportFeeTypeText { get; set; }
        public int TransportCompanyID { get; set; }
        public string TransportCompanyNumber { get; set; }
        public string Driver { get; set; }
        public string DriverTelephone { get; set; }
        public string StartPlace { get; set; }
        public string StartPlaceTelephone { get; set; }
        public string EndPlace { get; set; }
        public string EndPlaceTelephone { get; set; }
        public decimal Fee { get; set; }
        public System.DateTime SendDate { get; set; }
        public string Remark { get; set; }

        public string TransportCompanyName { get; set; }

        public int TransportFeeType { get; set; }

        public string CreatedByUserName { get; set; }

        public int Source { get; set; }

        public int? ReimbursementDetailTransportFeeID { get; set; }

        public string Comment { get; set; }

        public int ReimbursementDetailID { get; set; }
    }
}
