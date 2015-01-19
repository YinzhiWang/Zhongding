using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchClientInfoProductSetting : UISearchBase
    {
        public int ClientInfoID { get; set; }
        public int ClientUserID { get; set; }
        public int ClientCompanyID { get; set; }
    }
}
