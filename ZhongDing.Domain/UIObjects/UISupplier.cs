using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UISupplier : UIBase
    {
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
    }
}
