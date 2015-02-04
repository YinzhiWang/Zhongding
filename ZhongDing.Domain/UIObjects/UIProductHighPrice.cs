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

        public decimal? SupplierTaxRatio { get; set; }

        public decimal? ClientTaxRatio { get; set; }
    }
}
