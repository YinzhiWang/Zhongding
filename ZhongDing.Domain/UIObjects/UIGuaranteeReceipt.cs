using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIGuaranteeReceipt : UIBase
    {
        public IEnumerable<IDValueIntString> OrderCodesIDValueIntString { get; set; }

        public decimal ReceiptAmount { get; set; }

        public DateTime ReceiptDate { get; set; }

        public string OrderCodesHtml { get; set; }
    }
}
