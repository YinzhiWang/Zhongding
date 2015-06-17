using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIFixedAssets : UIBase
    {
        public string Code { get; set; }
        public int FixedAssetsTypeID { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public string Specification { get; set; }
        public string ProducingArea { get; set; }
        public string Manufacturer { get; set; }
        public int UseStatus { get; set; }
        public int DepartmentID { get; set; }
        public string UsePeople { get; set; }
        public int StorageLocationID { get; set; }
        public int DepreciationType { get; set; }
        public decimal OriginalValue { get; set; }
        public System.DateTime StartUsedDate { get; set; }
        public decimal EstimateNetSalvageValue { get; set; }
        public int EstimateUsedYear { get; set; }

        public string DepartmentName { get; set; }

        public string UseStatusText { get; set; }

        public string StorageLocationName { get; set; }

        public string FixedAssetsTypeName { get; set; }
    }
}
