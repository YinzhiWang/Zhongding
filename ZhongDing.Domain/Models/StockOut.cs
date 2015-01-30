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
    public partial class StockOut : IEntityExtendedProperty
    {
        public StockOut()
        {
            this.StockOutDetail = new HashSet<StockOutDetail>();
        }
    
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int ReceiverTypeID { get; set; }
        public string Code { get; set; }
        public System.DateTime BillDate { get; set; }
        public Nullable<System.DateTime> OutDate { get; set; }
        public Nullable<int> DistributionCompanyID { get; set; }
        public Nullable<int> ClientUserID { get; set; }
        public Nullable<int> ClientCompanyID { get; set; }
        public Nullable<int> InvoiceTypeID { get; set; }
        public int WorkflowStatusID { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverAddress { get; set; }
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
        public virtual ClientUser ClientUser { get; set; }
        public virtual Company Company { get; set; }
        public virtual DistributionCompany DistributionCompany { get; set; }
        public virtual WorkflowStatus WorkflowStatus { get; set; }
        public virtual ICollection<StockOutDetail> StockOutDetail { get; set; }
    }
}