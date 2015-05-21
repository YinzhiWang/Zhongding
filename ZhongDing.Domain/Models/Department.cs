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
    public partial class Department : IEntityExtendedProperty
    {
        public Department()
        {
            this.DeptProductEvaluation = new HashSet<DeptProductEvaluation>();
            this.Product = new HashSet<Product>();
            this.Users1 = new HashSet<Users>();
            this.DepartmentProductRecord = new HashSet<DepartmentProductRecord>();
            this.DepartmentProductSalesPlan = new HashSet<DepartmentProductSalesPlan>();
            this.DaBaoApplication = new HashSet<DaBaoApplication>();
            this.DaBaoRequestApplication = new HashSet<DaBaoRequestApplication>();
            this.ClientInfoProductSetting = new HashSet<ClientInfoProductSetting>();
            this.DBContract = new HashSet<DBContract>();
            this.ClientCautionMoney = new HashSet<ClientCautionMoney>();
        }
    
        public int ID { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> DirectorUserID { get; set; }
        public int DepartmentTypeID { get; set; }
        public Nullable<int> DeptDistrictID { get; set; }
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
    
    
        public virtual DeptDistrict DeptDistrict { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<DeptProductEvaluation> DeptProductEvaluation { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<Users> Users1 { get; set; }
        public virtual ICollection<DepartmentProductRecord> DepartmentProductRecord { get; set; }
        public virtual ICollection<DepartmentProductSalesPlan> DepartmentProductSalesPlan { get; set; }
        public virtual ICollection<DaBaoApplication> DaBaoApplication { get; set; }
        public virtual ICollection<DaBaoRequestApplication> DaBaoRequestApplication { get; set; }
        public virtual ICollection<ClientInfoProductSetting> ClientInfoProductSetting { get; set; }
        public virtual ICollection<DBContract> DBContract { get; set; }
        public virtual ICollection<ClientCautionMoney> ClientCautionMoney { get; set; }
    }
}
