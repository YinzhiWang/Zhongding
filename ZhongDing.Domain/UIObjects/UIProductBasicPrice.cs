using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIProductBasicPrice : UIProductPrice
    {
        public decimal? ProcurePrice { get; set; }

        public decimal? SalePrice { get; set; }
    }
}
