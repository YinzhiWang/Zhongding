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
    public partial class DistributionCompany : IEntityExtendedProperty
    {
        public DistributionCompany()
        {
            this.DaBaoApplication = new HashSet<DaBaoApplication>();
            this.DaBaoRequestApplication = new HashSet<DaBaoRequestApplication>();
            this.StockOut = new HashSet<StockOut>();
            this.DCFlowData = new HashSet<DCFlowData>();
            this.DCInventoryData = new HashSet<DCInventoryData>();
            this.DCImportFileLog = new HashSet<DCImportFileLog>();
            this.DBClientInvoice = new HashSet<DBClientInvoice>();
        }
    
        public int ID { get; set; }
        public string SerialNo { get; set; }
        public string Name { get; set; }
        public string ReceiverName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
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
    
    
        public virtual ICollection<DaBaoApplication> DaBaoApplication { get; set; }
        public virtual ICollection<DaBaoRequestApplication> DaBaoRequestApplication { get; set; }
        public virtual ICollection<StockOut> StockOut { get; set; }
        public virtual ICollection<DCFlowData> DCFlowData { get; set; }
        public virtual ICollection<DCInventoryData> DCInventoryData { get; set; }
        public virtual ICollection<DCImportFileLog> DCImportFileLog { get; set; }
        public virtual ICollection<DBClientInvoice> DBClientInvoice { get; set; }
    }
}
