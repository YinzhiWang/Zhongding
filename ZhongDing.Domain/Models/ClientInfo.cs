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
    public partial class ClientInfo : IEntityExtendedProperty
    {
        public ClientInfo()
        {
            this.ClientInfoBankAccount = new HashSet<ClientInfoBankAccount>();
            this.ClientInfoContact = new HashSet<ClientInfoContact>();
            this.ClientInfoProductSetting = new HashSet<ClientInfoProductSetting>();
        }
    
        public int ID { get; set; }
        public int ClientUserID { get; set; }
        public int ClientCompanyID { get; set; }
        public string ClientCode { get; set; }
        public string ReceiverName { get; set; }
        public string PhoneNumber { get; set; }
        public string Fax { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiptAddress { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<int> DBBankAccountID { get; set; }
    
    	// Implements IEntityExtendedProperty
    	public string DefaultOrderColumnName { get { return "id"; } }
    	public bool HasColumnIsDeleted { get { return true; } }
    	public bool HasColumnDeletedOn { get { return false; } }
    	public bool HasColumnCreatedOn { get { return true; } }
    	public bool HasColumnCreatedBy { get { return true; } }
    	public bool HasColumnLastModifiedOn { get { return true; } }
    	public bool HasColumnLastModifiedBy { get { return true; } }
    
    
        public virtual BankAccount BankAccount { get; set; }
        public virtual ClientCompany ClientCompany { get; set; }
        public virtual ClientUser ClientUser { get; set; }
        public virtual ICollection<ClientInfoBankAccount> ClientInfoBankAccount { get; set; }
        public virtual ICollection<ClientInfoContact> ClientInfoContact { get; set; }
        public virtual ICollection<ClientInfoProductSetting> ClientInfoProductSetting { get; set; }
    }
}
