using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UISupplierContract : UIBase
    {
        public string ContractCode { get; set; }
        public string SupplierName { get; set; }
        public string ProductName { get; set; }
        public string ProductSpecification { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<DateTime> ExpirationDate { get; set; }
    }
}
