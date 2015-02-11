using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientRefundApplication : UIWorkflowBase
    {
        public string OrderCode { get; set; }

        public string CompanyName { get; set; }

        public string ClientUserName { get; set; }

        public string ClientCompanyName { get; set; }

        public decimal? RefundAmount { get; set; }

    }
}
