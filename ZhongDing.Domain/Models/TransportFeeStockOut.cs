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
    public partial class TransportFeeStockOut : IEntityExtendedProperty
    {
        public TransportFeeStockOut()
        {
            this.TransportFeeStockOutSmsReminder1 = new HashSet<TransportFeeStockOutSmsReminder>();
        }
    
        public int ID { get; set; }
        public int TransportFeeID { get; set; }
        public int StockOutID { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<int> TransportFeeStockOutSmsReminderID { get; set; }
    
    	// Implements IEntityExtendedProperty
    	public string DefaultOrderColumnName { get { return "id"; } }
    	public bool HasColumnIsDeleted { get { return true; } }
    	public bool HasColumnDeletedOn { get { return false; } }
    	public bool HasColumnCreatedOn { get { return true; } }
    	public bool HasColumnCreatedBy { get { return true; } }
    	public bool HasColumnLastModifiedOn { get { return true; } }
    	public bool HasColumnLastModifiedBy { get { return true; } }
    
    
        public virtual StockOut StockOut { get; set; }
        public virtual TransportFee TransportFee { get; set; }
        public virtual TransportFeeStockOutSmsReminder TransportFeeStockOutSmsReminder { get; set; }
        public virtual ICollection<TransportFeeStockOutSmsReminder> TransportFeeStockOutSmsReminder1 { get; set; }
    }
}
