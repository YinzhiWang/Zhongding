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
    public partial class SupplierRefundApplication : IEntityExtendedProperty
    {
        public SupplierRefundApplication()
        {
            this.SupplierDeduction = new HashSet<SupplierDeduction>();
        }
    
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int SupplierID { get; set; }
        public int ProductID { get; set; }
        public int ProductSpecificationID { get; set; }
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
    
    
        public virtual Company Company { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductSpecification ProductSpecification { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<SupplierDeduction> SupplierDeduction { get; set; }
    }
}
