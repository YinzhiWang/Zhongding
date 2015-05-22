using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDCFlowSettlementReport : UIBase
    {
        public string ClientUserName { get; set; }

        public string HospitalTypeName { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public string HospitalName { get; set; }

        public int SaleQty { get; set; }

        public DateTime SaleDate { get; set; }

        public string DistributionCompanyName { get; set; }

        public decimal? TotalPromotionExpense { get; set; }
    }
}
