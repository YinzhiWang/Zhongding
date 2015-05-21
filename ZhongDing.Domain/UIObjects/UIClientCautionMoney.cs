using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientCautionMoney : UIWorkflowBase
    {

        public string DepartmentName { get; set; }
        public string ClientName { get; set; }
        public string ProductName { get; set; }
        public string ProductSpecification { get; set; }
        public string CautionMoneyTypeName { get; set; }
        public System.DateTime EndDate { get; set; }
        public decimal? PaymentCautionMoney { get; set; }
        public decimal? ReturnCautionMoney { get; set; }

        public bool IsStop { get; set; }
        public Nullable<System.DateTime> StoppedOn { get; set; }
        public Nullable<int> StoppedBy { get; set; }
      
        public string Remark { get; set; }


        public string CreatedByUserName { get; set; }

        public int CreatedByUserID { get; set; }

        public decimal? NotReturnCautionMoney { get; set; }
    }
}
