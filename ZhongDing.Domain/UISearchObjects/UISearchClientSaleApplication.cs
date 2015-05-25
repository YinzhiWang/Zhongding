using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchClientSaleApplication : UISearchWorkflowBase
    {
        public int CompanyID { get; set; }

        public int ClientUserID { get; set; }

        public int ClientCompanyID { get; set; }


        public bool? IsImport { get; set; }
    }
}
