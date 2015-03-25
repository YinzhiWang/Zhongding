using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchTransportFee : UISearchBase
    {

        public int TransportFeeType { get; set; }

        public string TransportCompanyName { get; set; }

        public string TransportCompanyNumber { get; set; }
    }
}
