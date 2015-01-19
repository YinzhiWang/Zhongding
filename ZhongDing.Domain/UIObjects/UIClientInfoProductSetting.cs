using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientInfoProductSetting : UIBase
    {
        public string ClientName { get; set; }
        public string ClientCompany { get; set; }
        public string ProductName { get; set; }
        public string ProductSpecification { get; set; }
        public string FactoryName { get; set; }
        public decimal? HighPrice { get; set; }
        public decimal? BasicPrice { get; set; }
        public bool UseFlowData { get; set; }

    }
}
