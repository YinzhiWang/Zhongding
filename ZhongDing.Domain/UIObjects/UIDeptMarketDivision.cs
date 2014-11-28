using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDeptMarketDivision : UIBase
    {
        public string BusinessManager { get; set; }

        public string Markets { get; set; }

        public IEnumerable<int> ProductIDs { get; set; }

        public string Products { get; set; }

        public int DepartmentTypeID { get; set; }
    }
}
