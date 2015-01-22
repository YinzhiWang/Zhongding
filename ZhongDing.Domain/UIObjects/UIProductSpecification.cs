using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIProductSpecification : UIBase
    {
        public string Specification { get; set; }

        public string UnitOfMeasurement { get; set; }

        public int? NumberInSmallPackage { get; set; }

        public int? NumberInLargePackage { get; set; }

        public string LicenseNumber { get; set; }
    }
}
