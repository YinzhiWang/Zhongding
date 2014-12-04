using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDeptProductSalesBonus : UIBase
    {
        public int SalesPlanID { get; set; }

        public int SalesPlanTypeID { get; set; }

        public int CompareOperatorID { get; set; }

        public string CompareOperator { get; set; }

        public decimal? SalesPrice { get; set; }

        public double? BonusRatio { get; set; }

        public bool IsDeleted { get; set; }
    }
}
