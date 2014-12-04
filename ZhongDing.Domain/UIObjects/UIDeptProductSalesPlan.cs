using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDeptProductSalesPlan : UIBase
    {
        public string DepartmentName { get; set; }
        public string ProductName { get; set; }
        public bool IsFixedOfInside { get; set; }
        public double? FixedRatioOfInside { get; set; }
        public bool IsFixedOfOutside { get; set; }
        public double? FixedRatioOfOutside { get; set; }
        public string InsideBonusDescription { get; set; }
        public string OutsideBonusDescription { get; set; }

    }
}
