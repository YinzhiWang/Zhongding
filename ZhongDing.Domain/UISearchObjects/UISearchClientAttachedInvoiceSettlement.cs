using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchClientAttachedInvoiceSettlement : UISearchWorkflowBase
    {
        public int CompanyID { get; set; }

        public bool ExcludeCanceled { get; set; }

        public int ClientUserID { get; set; }

        public int ClientCompanyID { get; set; }

    }
}
