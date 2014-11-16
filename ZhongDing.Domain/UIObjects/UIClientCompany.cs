using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientCompany : UIBase
    {
        public string Name { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
    }
}
