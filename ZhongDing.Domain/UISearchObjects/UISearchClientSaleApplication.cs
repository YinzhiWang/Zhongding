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

        public bool OnlyFilterID { get; set; }

        public IEnumerable<int> IncludeIDs { get; set; }

        public IEnumerable<int> UnIncludeIDs { get; set; }

        public bool OnlyNotReceipted { get; set; }

        public DateTime? GuaranteeExpirationDateBeginDate { get; set; }

        public DateTime? GuaranteeExpirationDateEndDate { get; set; }

        public bool NeedGuaranteeAmount { get; set; }

        public bool? IsGuaranteed { get; set; }
    }
}
