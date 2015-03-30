using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientFlowData : UIBase
    {
        public string ClientUserName { get; set; }

        public string ClientCompanyName { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public System.DateTime SaleDate { get; set; }

        public int SaleQty { get; set; }

        public string FactoryName { get; set; }

        public string FlowTo { get; set; }

        public string HospitalType { get; set; }

        public string MarketName { get; set; }

    }
}
