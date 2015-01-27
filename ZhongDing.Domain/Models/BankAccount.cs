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
    public partial class BankAccount : IEntityExtendedProperty
    {
        public BankAccount()
        {
            this.ClientInfoBankAccount = new HashSet<ClientInfoBankAccount>();
            this.SupplierBankAccount = new HashSet<SupplierBankAccount>();
            this.ClientSaleAppBankAccount = new HashSet<ClientSaleAppBankAccount>();
            this.ApplicationPayment = new HashSet<ApplicationPayment>();
            this.ApplicationPayment1 = new HashSet<ApplicationPayment>();
        }
    
        public int ID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string AccountName { get; set; }
        public string BankBranchName { get; set; }
        public string Account { get; set; }
        public Nullable<int> AccountTypeID { get; set; }
        public Nullable<int> OwnerTypeID { get; set; }
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
    
    
        public virtual AccountType AccountType { get; set; }
        public virtual Company Company { get; set; }
        public virtual OwnerType OwnerType { get; set; }
        public virtual ICollection<ClientInfoBankAccount> ClientInfoBankAccount { get; set; }
        public virtual ICollection<SupplierBankAccount> SupplierBankAccount { get; set; }
        public virtual ICollection<ClientSaleAppBankAccount> ClientSaleAppBankAccount { get; set; }
        public virtual ICollection<ApplicationPayment> ApplicationPayment { get; set; }
        public virtual ICollection<ApplicationPayment> ApplicationPayment1 { get; set; }
    }
}
