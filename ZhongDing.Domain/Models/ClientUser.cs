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
    public partial class ClientUser : IEntityExtendedProperty
    {
        public ClientUser()
        {
            this.ClientFlowData = new HashSet<ClientFlowData>();
            this.ClientImportFileLog = new HashSet<ClientImportFileLog>();
            this.ClientInfo = new HashSet<ClientInfo>();
            this.ClientRefundApplication = new HashSet<ClientRefundApplication>();
            this.ClientTaskRefundApplication = new HashSet<ClientTaskRefundApplication>();
            this.DBClientBonus = new HashSet<DBClientBonus>();
            this.DBContract = new HashSet<DBContract>();
            this.FactoryManagerRefundApplication = new HashSet<FactoryManagerRefundApplication>();
            this.StockOut = new HashSet<StockOut>();
            this.ClientAttachedInvoiceSettlement = new HashSet<ClientAttachedInvoiceSettlement>();
            this.ClientCautionMoney = new HashSet<ClientCautionMoney>();
            this.ClientSaleApplication = new HashSet<ClientSaleApplication>();
        }
    
        public int ID { get; set; }
        public string ClientName { get; set; }
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
        public virtual ICollection<ClientFlowData> ClientFlowData { get; set; }
        public virtual ICollection<ClientImportFileLog> ClientImportFileLog { get; set; }
        public virtual ICollection<ClientInfo> ClientInfo { get; set; }
        public virtual ICollection<ClientRefundApplication> ClientRefundApplication { get; set; }
        public virtual ICollection<ClientTaskRefundApplication> ClientTaskRefundApplication { get; set; }
        public virtual ICollection<DBClientBonus> DBClientBonus { get; set; }
        public virtual ICollection<DBContract> DBContract { get; set; }
        public virtual ICollection<FactoryManagerRefundApplication> FactoryManagerRefundApplication { get; set; }
        public virtual ICollection<StockOut> StockOut { get; set; }
        public virtual ICollection<ClientAttachedInvoiceSettlement> ClientAttachedInvoiceSettlement { get; set; }
        public virtual ICollection<ClientCautionMoney> ClientCautionMoney { get; set; }
        public virtual ICollection<ClientSaleApplication> ClientSaleApplication { get; set; }
    }
}
