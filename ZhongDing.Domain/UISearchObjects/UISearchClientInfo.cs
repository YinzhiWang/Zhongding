using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchClientInfo : UISearchBase
    {
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public int ClientUserID { get; set; }
        public int ClientCompanyID { get; set; }
    }
}
