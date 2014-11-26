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
    public partial class Product : IEntityExtendedProperty
    {
        public Product()
        {
            this.ProductCertificate = new HashSet<ProductCertificate>();
            this.ProductSpecification = new HashSet<ProductSpecification>();
            this.SupplierContract = new HashSet<SupplierContract>();
            this.ClientInfoProductSetting = new HashSet<ClientInfoProductSetting>();
            this.DeptProductEvaluation = new HashSet<DeptProductEvaluation>();
            this.ProductHighPrice = new HashSet<ProductHighPrice>();
            this.ProductBasicPrice = new HashSet<ProductBasicPrice>();
            this.ProductDBPolicyPrice = new HashSet<ProductDBPolicyPrice>();
            this.DeptMarketProduct = new HashSet<DeptMarketProduct>();
        }
    
        public int ID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public Nullable<bool> IsManagedByBatchNumber { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public Nullable<int> SafetyStock { get; set; }
        public Nullable<int> ValidDays { get; set; }
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
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<ProductCertificate> ProductCertificate { get; set; }
        public virtual ICollection<ProductSpecification> ProductSpecification { get; set; }
        public virtual ICollection<SupplierContract> SupplierContract { get; set; }
        public virtual ICollection<ClientInfoProductSetting> ClientInfoProductSetting { get; set; }
        public virtual ICollection<DeptProductEvaluation> DeptProductEvaluation { get; set; }
        public virtual ICollection<ProductHighPrice> ProductHighPrice { get; set; }
        public virtual ICollection<ProductBasicPrice> ProductBasicPrice { get; set; }
        public virtual ICollection<ProductDBPolicyPrice> ProductDBPolicyPrice { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<DeptMarketProduct> DeptMarketProduct { get; set; }
    }
}
