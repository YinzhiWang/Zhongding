using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UISupplierCautionMoney
    {
        public int ID { get; set; }
        public string SupplierName { get; set; }
        public string ProductName { get; set; }
        public string ProductSpecification { get; set; }
        public string CautionMoneyTypeName { get; set; }
        public System.DateTime EndDate { get; set; }
        public decimal PaymentCautionMoney { get; set; }
        public decimal TakeBackCautionMoney { get; set; }
      
      
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public int WorkflowStatusID { get; set; }
        public bool IsStop { get; set; }
        public Nullable<System.DateTime> StoppedOn { get; set; }
        public Nullable<int> StoppedBy { get; set; }
        public System.DateTime ApplyDate { get; set; }
        public string Remark { get; set; }


        public string CreatedByUserName { get; set; }

        public string StatusName { get; set; }

        public int CreatedByUserID { get; set; }
    }
}
