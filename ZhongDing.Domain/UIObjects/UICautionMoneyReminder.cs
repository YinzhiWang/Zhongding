using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UICautionMoneyReminder : UIBase
    {
        public string CautionMoneyTypeName { get; set; }

        public DateTime EndDate { get; set; }

        public decimal? PaymentCautionMoney { get; set; }

        public string ProductName { get; set; }

        public string ClientOrSupplierName { get; set; }

        public decimal? ReturnCautionMoney { get; set; }

        public string Type { get; set; }

        public decimal RefundedAmount { get; set; }

        public decimal DeductedAmount { get; set; }

        public int CautionMoneyTypeID { get; set; }
    }
}
