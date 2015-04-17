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
    public partial class Company : IEntityExtendedProperty
    {
        public Company()
        {
            this.BankAccount = new HashSet<BankAccount>();
            this.Product = new HashSet<Product>();
            this.Supplier = new HashSet<Supplier>();
            this.Warehouse = new HashSet<Warehouse>();
            this.DaBaoApplication = new HashSet<DaBaoApplication>();
            this.DaBaoRequestApplication = new HashSet<DaBaoRequestApplication>();
            this.StockOut = new HashSet<StockOut>();
            this.ClientSaleApplication = new HashSet<ClientSaleApplication>();
            this.ClientRefundApplication = new HashSet<ClientRefundApplication>();
            this.FactoryManagerRefundApplication = new HashSet<FactoryManagerRefundApplication>();
            this.SupplierRefundApplication = new HashSet<SupplierRefundApplication>();
            this.ClientTaskRefundApplication = new HashSet<ClientTaskRefundApplication>();
            this.ClientInvoice = new HashSet<ClientInvoice>();
            this.DBClientInvoice = new HashSet<DBClientInvoice>();
            this.SupplierInvoice = new HashSet<SupplierInvoice>();
            this.ClientInvoiceSettlement = new HashSet<ClientInvoiceSettlement>();
            this.SupplierInvoiceSettlement = new HashSet<SupplierInvoiceSettlement>();
        }
    
        public int ID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public Nullable<decimal> ProviderTexRatio { get; set; }
        public Nullable<decimal> ClientTaxHighRatio { get; set; }
        public Nullable<decimal> ClientTaxLowRatio { get; set; }
        public Nullable<bool> EnableTaxDeduction { get; set; }
        public Nullable<decimal> ClientTaxDeductionRatio { get; set; }
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
    
    
        public virtual ICollection<BankAccount> BankAccount { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<Supplier> Supplier { get; set; }
        public virtual ICollection<Warehouse> Warehouse { get; set; }
        public virtual ICollection<DaBaoApplication> DaBaoApplication { get; set; }
        public virtual ICollection<DaBaoRequestApplication> DaBaoRequestApplication { get; set; }
        public virtual ICollection<StockOut> StockOut { get; set; }
        public virtual ICollection<ClientSaleApplication> ClientSaleApplication { get; set; }
        public virtual ICollection<ClientRefundApplication> ClientRefundApplication { get; set; }
        public virtual ICollection<FactoryManagerRefundApplication> FactoryManagerRefundApplication { get; set; }
        public virtual ICollection<SupplierRefundApplication> SupplierRefundApplication { get; set; }
        public virtual ICollection<ClientTaskRefundApplication> ClientTaskRefundApplication { get; set; }
        public virtual ICollection<ClientInvoice> ClientInvoice { get; set; }
        public virtual ICollection<DBClientInvoice> DBClientInvoice { get; set; }
        public virtual ICollection<SupplierInvoice> SupplierInvoice { get; set; }
        public virtual ICollection<ClientInvoiceSettlement> ClientInvoiceSettlement { get; set; }
        public virtual ICollection<SupplierInvoiceSettlement> SupplierInvoiceSettlement { get; set; }
    }
}
