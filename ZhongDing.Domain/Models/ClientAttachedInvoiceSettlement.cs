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
    public partial class ClientAttachedInvoiceSettlement : IEntityExtendedProperty
    {
        public ClientAttachedInvoiceSettlement()
        {
            this.ClientAttachedInvoiceSettlementDetail = new HashSet<ClientAttachedInvoiceSettlementDetail>();
        }
    
        public int ID { get; set; }
        public int ClientUserID { get; set; }
        public int CompanyID { get; set; }
        public int ClientCompanyID { get; set; }
        public int WorkflowStatusID { get; set; }
        public int ReceiveBankAccountID { get; set; }
        public string ReceiveAccount { get; set; }
        public decimal ReceiveAmount { get; set; }
        public Nullable<int> OtherCostTypeID { get; set; }
        public Nullable<decimal> OtherCostAmount { get; set; }
        public Nullable<System.DateTime> ConfirmDate { get; set; }
        public Nullable<System.DateTime> SettlementDate { get; set; }
        public Nullable<decimal> TotalSettlementAmount { get; set; }
        public Nullable<decimal> TotalRefundAmount { get; set; }
        public Nullable<System.DateTime> PaidDate { get; set; }
        public Nullable<int> PaidBy { get; set; }
        public Nullable<int> AppPaymentID { get; set; }
        public Nullable<int> CanceledAppPaymentID { get; set; }
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
        public virtual ClientCompany ClientCompany { get; set; }
        public virtual ClientUser ClientUser { get; set; }
        public virtual Company Company { get; set; }
        public virtual CostType CostType { get; set; }
        public virtual WorkflowStatus WorkflowStatus { get; set; }
        public virtual ICollection<ClientAttachedInvoiceSettlementDetail> ClientAttachedInvoiceSettlementDetail { get; set; }
        public virtual ApplicationPayment ApplicationPayment { get; set; }
        public virtual ApplicationPayment ApplicationPayment1 { get; set; }
    }
}
