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
    public partial class Users : IEntityExtendedProperty
    {
        public Users()
        {
            this.Department = new HashSet<Department>();
        }
    
        public int UserID { get; set; }
        public Nullable<System.Guid> AspnetUserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string Position { get; set; }
        public string MobilePhone { get; set; }
        public Nullable<System.DateTime> EnrollDate { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public Nullable<decimal> PositionSalary { get; set; }
        public Nullable<decimal> PhoneAllowance { get; set; }
        public Nullable<decimal> MealAllowance { get; set; }
        public Nullable<decimal> OfficeExpense { get; set; }
        public Nullable<decimal> BonusPay { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
    
    	// Implements IEntityExtendedProperty
    	public string DefaultOrderColumnName { get { return "userid"; } }
    	public bool HasColumnIsDeleted { get { return true; } }
    	public bool HasColumnDeletedOn { get { return true; } }
    	public bool HasColumnCreatedOn { get { return true; } }
    	public bool HasColumnCreatedBy { get { return true; } }
    	public bool HasColumnLastModifiedOn { get { return true; } }
    	public bool HasColumnLastModifiedBy { get { return true; } }
    
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual ICollection<Department> Department { get; set; }
    }
}
