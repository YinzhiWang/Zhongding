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
    public partial class Certificate : IEntityExtendedProperty
    {
        public Certificate()
        {
            this.ProductCertificate = new HashSet<ProductCertificate>();
            this.ClientCompanyCertificate = new HashSet<ClientCompanyCertificate>();
            this.SupplierCertificate = new HashSet<SupplierCertificate>();
        }
    
        public int ID { get; set; }
        public Nullable<int> CertificateTypeID { get; set; }
        public Nullable<int> OwnerTypeID { get; set; }
        public Nullable<bool> IsGotten { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public Nullable<bool> IsNeedAlert { get; set; }
        public Nullable<int> AlertBeforeDays { get; set; }
        public string Comment { get; set; }
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
    
    
        public virtual OwnerType OwnerType { get; set; }
        public virtual ICollection<ProductCertificate> ProductCertificate { get; set; }
        public virtual ICollection<ClientCompanyCertificate> ClientCompanyCertificate { get; set; }
        public virtual ICollection<SupplierCertificate> SupplierCertificate { get; set; }
        public virtual CertificateType CertificateType { get; set; }
    }
}
