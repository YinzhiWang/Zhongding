using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIProductHighPrice : UIProductPrice
    {
        public decimal? HighPrice { get; set; }

        public decimal? ActualProcurePrice { get; set; }

        public decimal? ActualSalePrice { get; set; }

        public double? SupplierTaxRatio { get; set; }

        public double? ClientTaxRatio { get; set; }
    }
}
