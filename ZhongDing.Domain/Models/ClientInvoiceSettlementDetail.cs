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
    public partial class ClientInvoiceSettlementDetail : IEntityExtendedProperty
    {
        public int ID { get; set; }
        public int ClientInvoiceSettlementID { get; set; }
        public int ClientCompanyID { get; set; }
        public int ClientInvoiceID { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalInvoiceAmount { get; set; }
        public Nullable<decimal> ClientTaxHighRatio { get; set; }
        public Nullable<decimal> HighRatioAmount { get; set; }
        public Nullable<decimal> ClientTaxLowRatio { get; set; }
        public Nullable<decimal> LowRatioAmount { get; set; }
        public Nullable<decimal> ClientTaxDeductionRatio { get; set; }
        public Nullable<decimal> DeductionRatioAmount { get; set; }
        public decimal PayAmount { get; set; }
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
    
    
        public virtual ClientCompany ClientCompany { get; set; }
        public virtual ClientInvoice ClientInvoice { get; set; }
        public virtual ClientInvoiceSettlement ClientInvoiceSettlement { get; set; }
    }
}
