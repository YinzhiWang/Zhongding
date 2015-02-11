using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientRefundAppDetail : UIBase
    {
        public string ProductName { get; set; }

        public string Specification { get; set; }

        public string UnitOfMeasurement { get; set; }

        public int Count { get; set; }

        public decimal HighPrice { get; set; }

        public decimal ActualSalePrice { get; set; }

        public decimal TotalSalesAmount { get; set; }

        public decimal? ClientTaxRatio { get; set; }

        public decimal? RefundAmount { get; set; }
    }
}
