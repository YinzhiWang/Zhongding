using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchApplicationPayment : UISearchWorkflowBase
    {
        public int ApplicationID { get; set; }

        public int WorkflowID { get; set; }

        public int PaymentTypeID { get; set; }

        public DateTime? PayDate { get; set; }


        public int PaymentStatusID { get; set; }

        public int? BankAccountID { get; set; }

        public int CompanyID { get; set; }

        public List<int> IncludeBankAccountIDs { get; set; }
    }
}
