using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDaBaoAppDetail : UIWorkflowBase
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public string UnitOfMeasurement { get; set; }

        public decimal SalesPrice { get; set; }

        public int Count { get; set; }

        public decimal TotalSalesAmount { get; set; }
    }
}
