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
    public partial class ProductSpecification : IEntityExtendedProperty
    {
        public ProductSpecification()
        {
            this.ClientInfoProductSetting = new HashSet<ClientInfoProductSetting>();
            this.ProductHighPrice = new HashSet<ProductHighPrice>();
            this.ProductBasicPrice = new HashSet<ProductBasicPrice>();
            this.ProductDBPolicyPrice = new HashSet<ProductDBPolicyPrice>();
            this.SupplierContract = new HashSet<SupplierContract>();
            this.DBContract = new HashSet<DBContract>();
        }
    
        public int ID { get; set; }
        public string Specification { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> UnitOfMeasurementID { get; set; }
        public Nullable<int> NumberInSmallPackage { get; set; }
        public Nullable<int> NumberInLargePackage { get; set; }
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
    
    
        public virtual Product Product { get; set; }
        public virtual UnitOfMeasurement UnitOfMeasurement { get; set; }
        public virtual ICollection<ClientInfoProductSetting> ClientInfoProductSetting { get; set; }
        public virtual ICollection<ProductHighPrice> ProductHighPrice { get; set; }
        public virtual ICollection<ProductBasicPrice> ProductBasicPrice { get; set; }
        public virtual ICollection<ProductDBPolicyPrice> ProductDBPolicyPrice { get; set; }
        public virtual ICollection<SupplierContract> SupplierContract { get; set; }
        public virtual ICollection<DBContract> DBContract { get; set; }
    }
}
