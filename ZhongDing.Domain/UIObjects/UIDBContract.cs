using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDBContract : UIBase
    {
        public string ContractCode { get; set; }
        public string ClientUserName { get; set; }
        public string DepartmentName { get; set; }
        public string ProductName { get; set; }
        public string ProductSpecification { get; set; }
        public double? PromotionExpense { get; set; }
        public string InChargeUser { get; set; }
        public bool? IsTempContract { get; set; }
        public DateTime? ContractExpDate { get; set; }
        public IEnumerable<int> HospitalIDs { get; set; }
        public string HospitalNames { get; set; }
    }
}
