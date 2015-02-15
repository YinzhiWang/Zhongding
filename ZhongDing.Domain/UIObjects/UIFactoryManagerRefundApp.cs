using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIFactoryManagerRefundApp : UIWorkflowBase
    {
        public string CompanyName { get; set; }

        public string ClientUserName { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal RefundAmount { get; set; }

    }
}
