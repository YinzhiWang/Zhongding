using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIProductPrice : UIBase
    {
        public int ProductID { get; set; }

        public int ProductSpecificationID { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public string FactoryName { get; set; }

    }
}
