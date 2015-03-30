using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchClientFlowData : UISearchBase
    {
        public int ClientUserID { get; set; }

        public int ClientCompanyID { get; set; }

        public int ProductID { get; set; }

        public int ImportFileLogID { get; set; }

    }
}
