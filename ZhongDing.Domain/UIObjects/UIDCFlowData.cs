using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDCFlowData : UIBase
    {
        public DateTime SaleDate { get; set; }

        public string DistributionCompanyName { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public string UnitName { get; set; }

        public int SaleQty { get; set; }

        public string FactoryName { get; set; }

    }
}
