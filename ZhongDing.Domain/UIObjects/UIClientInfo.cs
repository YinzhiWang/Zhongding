using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientInfo : UIBase
    {
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public string ClientCompany { get; set; }
        public string ReceiverName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
