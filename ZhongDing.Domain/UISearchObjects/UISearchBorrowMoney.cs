using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchBorrowMoney : UISearchBase
    {
        public int? Status { get; set; }

        public string BorrowName { get; set; }
    }
}
