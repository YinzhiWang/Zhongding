using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UISalarySettleDetail : UIBase
    {
        public int SalarySettleID { get; set; }
        public int UserID { get; set; }
        public decimal? BasicSalary { get; set; }
        public int? WorkDay { get; set; }
        public decimal? MealAllowance { get; set; }
        public decimal? PositionSalary { get; set; }
        public decimal? BonusPay { get; set; }
        public decimal? WorkAgeSalary { get; set; }
        public decimal? PhoneAllowance { get; set; }
        public decimal? OfficeExpense { get; set; }
        public decimal? OtherAllowance { get; set; }
        public decimal? NeedPaySalary { get; set; }
        public decimal? NeedDeduct { get; set; }
        public decimal? HolidayDeductOfSalary { get; set; }
        public decimal? HolidayDeductOfMealAllowance { get; set; }
        public decimal? RealPaySalary { get; set; }
        public bool IsPayed { get; set; }
        public Nullable<int> ApplicationPaymentID { get; set; }

        public string FullName { get; set; }

        public DateTime? EnrollDate { get; set; }

        public int Order { get; set; }

        public decimal? DefaultBasicSalary { get; set; }

        public decimal? DefaultPositionSalary { get; set; }

        public decimal? DefaultPhoneAllowance { get; set; }

        public decimal? DefaultMealAllowance { get; set; }

        public decimal? DefaultOfficeExpense { get; set; }

        public decimal? DefaultBonusPay { get; set; }

        public int? FromBankAccountID { get; set; }

        public string FromAccount { get; set; }
    }
}
