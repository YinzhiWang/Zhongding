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
    public partial class DBClientInvoiceSettlement : IEntityExtendedProperty
    {
        public DBClientInvoiceSettlement()
        {
            this.DBClientInvoiceSettlementDetail = new HashSet<DBClientInvoiceSettlementDetail>();
        }
    
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int DistributionCompanyID { get; set; }
        public System.DateTime ReceiveDate { get; set; }
        public int ReceiveBankAccountID { get; set; }
        public decimal TotalInvoiceAmount { get; set; }
        public decimal TotalReceiveAmount { get; set; }
        public Nullable<System.DateTime> ConfirmDate { get; set; }
        public bool IsCanceled { get; set; }
        public string CanceledReason { get; set; }
        public Nullable<System.DateTime> CanceledDate { get; set; }
        public Nullable<int> CanceledBy { get; set; }
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
    
    
        public virtual BankAccount BankAccount { get; set; }
        public virtual Company Company { get; set; }
        public virtual DistributionCompany DistributionCompany { get; set; }
        public virtual ICollection<DBClientInvoiceSettlementDetail> DBClientInvoiceSettlementDetail { get; set; }
    }
}
