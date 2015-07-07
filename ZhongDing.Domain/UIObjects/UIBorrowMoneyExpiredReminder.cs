using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIBorrowMoneyExpiredReminder : UIBase
    {
        public System.DateTime BorrowDate { get; set; }
        public string BorrowName { get; set; }
        public decimal BorrowAmount { get; set; }
        public System.DateTime ReturnDate { get; set; }
        public string Comment { get; set; }

        public decimal? ReturnAmount { get; set; }

        public int Status { get; set; }
    }
}
