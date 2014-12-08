using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchDeptMarketProduct : UISearchBase
    {
        public int MarketDivisionID { get; set; }

        public int ProductID { get; set; }
    }
}
