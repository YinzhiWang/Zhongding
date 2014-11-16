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
    public partial class Supplier : IEntityExtendedProperty
    {
        public Supplier()
        {
            this.Product = new HashSet<Product>();
            this.SupplierBankAccount = new HashSet<SupplierBankAccount>();
            this.SupplierContract = new HashSet<SupplierContract>();
            this.SupplierTaskAssignment = new HashSet<SupplierTaskAssignment>();
            this.SupplierCertificate = new HashSet<SupplierCertificate>();
        }
    
        public int ID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string District { get; set; }
        public string FactoryName { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactAddress { get; set; }
        public string Fax { get; set; }
        public string PostalCode { get; set; }
        public bool IsProducer { get; set; }
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
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<SupplierBankAccount> SupplierBankAccount { get; set; }
        public virtual ICollection<SupplierContract> SupplierContract { get; set; }
        public virtual ICollection<SupplierTaskAssignment> SupplierTaskAssignment { get; set; }
        public virtual ICollection<SupplierCertificate> SupplierCertificate { get; set; }
    }
}
