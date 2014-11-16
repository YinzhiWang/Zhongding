using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientInfoBankAccount : UIBankAccount
    {
        public int? ClientInfoID { get; set; }

        public int? BankAccountID { get; set; }
    }
}
