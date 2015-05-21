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
    public partial class WorkflowStatus : IEntityExtendedProperty
    {
        public WorkflowStatus()
        {
            this.WorkflowStepStatus = new HashSet<WorkflowStepStatus>();
            this.ProcureOrderApplication = new HashSet<ProcureOrderApplication>();
            this.StockIn = new HashSet<StockIn>();
            this.DaBaoApplication = new HashSet<DaBaoApplication>();
            this.DaBaoRequestApplication = new HashSet<DaBaoRequestApplication>();
            this.StockOut = new HashSet<StockOut>();
            this.ClientSaleApplication = new HashSet<ClientSaleApplication>();
            this.ClientRefundApplication = new HashSet<ClientRefundApplication>();
            this.FactoryManagerRefundApplication = new HashSet<FactoryManagerRefundApplication>();
            this.ClientTaskRefundApplication = new HashSet<ClientTaskRefundApplication>();
            this.DBClientSettlement = new HashSet<DBClientSettlement>();
            this.ClientInvoiceSettlement = new HashSet<ClientInvoiceSettlement>();
            this.SupplierCautionMoney = new HashSet<SupplierCautionMoney>();
            this.ClientAttachedInvoiceSettlement = new HashSet<ClientAttachedInvoiceSettlement>();
            this.ClientCautionMoneyReturnApplication = new HashSet<ClientCautionMoneyReturnApplication>();
        }
    
        public int ID { get; set; }
        public string StatusName { get; set; }
        public string Comment { get; set; }
        public bool IsDeleted { get; set; }
    
    	// Implements IEntityExtendedProperty
    	public string DefaultOrderColumnName { get { return "id"; } }
    	public bool HasColumnIsDeleted { get { return true; } }
    	public bool HasColumnDeletedOn { get { return false; } }
    	public bool HasColumnCreatedOn { get { return false; } }
    	public bool HasColumnCreatedBy { get { return false; } }
    	public bool HasColumnLastModifiedOn { get { return false; } }
    	public bool HasColumnLastModifiedBy { get { return false; } }
    
    
        public virtual ICollection<WorkflowStepStatus> WorkflowStepStatus { get; set; }
        public virtual ICollection<ProcureOrderApplication> ProcureOrderApplication { get; set; }
        public virtual ICollection<StockIn> StockIn { get; set; }
        public virtual ICollection<DaBaoApplication> DaBaoApplication { get; set; }
        public virtual ICollection<DaBaoRequestApplication> DaBaoRequestApplication { get; set; }
        public virtual ICollection<StockOut> StockOut { get; set; }
        public virtual ICollection<ClientSaleApplication> ClientSaleApplication { get; set; }
        public virtual ICollection<ClientRefundApplication> ClientRefundApplication { get; set; }
        public virtual ICollection<FactoryManagerRefundApplication> FactoryManagerRefundApplication { get; set; }
        public virtual ICollection<ClientTaskRefundApplication> ClientTaskRefundApplication { get; set; }
        public virtual ICollection<DBClientSettlement> DBClientSettlement { get; set; }
        public virtual ICollection<ClientInvoiceSettlement> ClientInvoiceSettlement { get; set; }
        public virtual ICollection<SupplierCautionMoney> SupplierCautionMoney { get; set; }
        public virtual ICollection<ClientAttachedInvoiceSettlement> ClientAttachedInvoiceSettlement { get; set; }
        public virtual ICollection<ClientCautionMoneyReturnApplication> ClientCautionMoneyReturnApplication { get; set; }
    }
}
