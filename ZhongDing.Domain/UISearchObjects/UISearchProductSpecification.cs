using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchProductSpecification : UISearchBase
    {
        public int ProductID { get; set; }

        public string Specification { get; set; }

        public int UnitOfMeasurementID { get; set; }
    }
}
