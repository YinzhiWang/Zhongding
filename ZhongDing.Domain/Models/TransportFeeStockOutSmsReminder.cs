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
    public partial class TransportFeeStockOutSmsReminder : IEntityExtendedProperty
    {
        public TransportFeeStockOutSmsReminder()
        {
            this.TransportFeeStockOut = new HashSet<TransportFeeStockOut>();
        }
    
        public int ID { get; set; }
        public int TransportFeeStockOutID { get; set; }
        public int Status { get; set; }
        public string MobileNumber { get; set; }
        public string Content { get; set; }
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
    
    
        public virtual ICollection<TransportFeeStockOut> TransportFeeStockOut { get; set; }
        public virtual TransportFeeStockOut TransportFeeStockOut1 { get; set; }
    }
}
