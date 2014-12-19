using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIProcureOrderAppDetail : UIBase
    {
        public string Warehouse { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public string UnitOfMeasurement { get; set; }

        public int ProcureCount { get; set; }

        public decimal NumberOfPackages { get; set; }

        public decimal ProcurePrice { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal? TaxAmount { get; set; }

    }
}
