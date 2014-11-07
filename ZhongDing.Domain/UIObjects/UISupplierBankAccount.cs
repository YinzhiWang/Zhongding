using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UISupplierBankAccount : UIBankAccount
    {
        public int? SupplierID { get; set; }

        public int? BankAccountID { get; set; }

        public string SupplierName { get; set; }

    }
}
