using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIProduct : UIBase
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public string SupplierName { get; set; }
        public string DepartmentName { get; set; }
    }
}
