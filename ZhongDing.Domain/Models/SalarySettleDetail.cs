//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZhongDing.Domain.Models
{
    using System;
    using System.Collections.Generic;
    
    [Serializable]
    public partial class SalarySettleDetail : IEntityExtendedProperty
    {
        public int ID { get; set; }
        public int SalarySettleID { get; set; }
        public int UserID { get; set; }
        public decimal BasicSalary { get; set; }
        public int WorkDay { get; set; }
        public Nullable<decimal> MealAllowance { get; set; }
        public Nullable<decimal> PositionSalary { get; set; }
        public Nullable<decimal> BonusPay { get; set; }
        public decimal WorkAgeSalary { get; set; }
        public Nullable<decimal> PhoneAllowance { get; set; }
        public Nullable<decimal> OfficeExpense { get; set; }
        public decimal OtherAllowance { get; set; }
        public decimal NeedPaySalary { get; set; }
        public decimal NeedDeduct { get; set; }
        public decimal HolidayDeductOfSalary { get; set; }
        public decimal HolidayDeductOfMealAllowance { get; set; }
        public decimal RealPaySalary { get; set; }
        public bool IsPayed { get; set; }
        public Nullable<int> ApplicationPaymentID { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
    
    	// Implements IEntityExtendedProperty
    	public string DefaultOrderColumnName { get { return "id"; } }
    	public bool HasColumnIsDeleted { get { return true; } }
    	public bool HasColumnDeletedOn { get { return false; } }
    	public bool HasColumnCreatedOn { get { return true; } }
    	public bool HasColumnCreatedBy { get { return true; } }
    	public bool HasColumnLastModifiedOn { get { return true; } }
    	public bool HasColumnLastModifiedBy { get { return true; } }
    
    
        public virtual ApplicationPayment ApplicationPayment { get; set; }
        public virtual SalarySettle SalarySettle { get; set; }
        public virtual Users Users { get; set; }
    }
}
