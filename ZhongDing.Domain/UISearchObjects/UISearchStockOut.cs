using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchStockOut : UISearchWorkflowBase
    {
        public int CompanyID { get; set; }

        public int ReceiverTypeID { get; set; }

        public string Code { get; set; }

        public int DistributionCompanyID { get; set; }

        public int ClientUserID { get; set; }

        public int ClientCompanyID { get; set; }

        public int ProductID { get; set; }

        public int ProductSpecificationID { get; set; }
    }
}
