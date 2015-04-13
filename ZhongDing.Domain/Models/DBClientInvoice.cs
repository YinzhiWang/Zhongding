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
    public partial class DBClientInvoice : IEntityExtendedProperty
    {
        public DBClientInvoice()
        {
            this.DBClientInvoiceDetail = new HashSet<DBClientInvoiceDetail>();
        }
    
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int ClientCompanyID { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Amount { get; set; }
        public string TransportNumber { get; set; }
        public string TransportCompany { get; set; }
        public int SaleOrderTypeID { get; set; }
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
        public virtual Company Company { get; set; }
        public virtual SaleOrderType SaleOrderType { get; set; }
        public virtual ICollection<DBClientInvoiceDetail> DBClientInvoiceDetail { get; set; }
    }
}
