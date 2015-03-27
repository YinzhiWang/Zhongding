using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIStockOut : UIWorkflowBase
    {
        public string Code { get; set; }

        public string DistributionCompany { get; set; }

        public string ClientUserName { get; set; }

        public string ClientCompany { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverPhone { get; set; }

        public string ReceiverAddress { get; set; }
 
    }
}
