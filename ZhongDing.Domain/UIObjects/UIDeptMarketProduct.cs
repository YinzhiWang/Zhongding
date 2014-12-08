using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDeptMarketProduct : UIBase
    {
        public int MarketDivisionID { get; set; }
        public string ProductName { get; set; }
        public int? Q1Task { get; set; }
        public int? Q2Task { get; set; }
        public int? Q3Task { get; set; }
        public int? Q4Task { get; set; }
        public int? SubtotalTask { get; set; }
    }
}
