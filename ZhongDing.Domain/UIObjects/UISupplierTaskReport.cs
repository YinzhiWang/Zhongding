using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UISupplierTaskReport : UIBase
    {
        public int? TaskQuantity { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public int ProductID { get; set; }

        public int ProductSpecificationID { get; set; }

        public int AlreadyProcureCount { get; set; }

        public string SupplierName { get; set; }

        public DateTime? Date { get; set; }

        public int? SupplierID { get; set; }
    }
}
