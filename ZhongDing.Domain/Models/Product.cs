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
    public partial class Product : IEntityExtendedProperty
    {
        public Product()
        {
            this.ProductCertificate = new HashSet<ProductCertificate>();
            this.DeptProductEvaluation = new HashSet<DeptProductEvaluation>();
            this.ProductBasicPrice = new HashSet<ProductBasicPrice>();
            this.ProductDBPolicyPrice = new HashSet<ProductDBPolicyPrice>();
            this.DeptMarketProduct = new HashSet<DeptMarketProduct>();
            this.DepartmentProductRecord = new HashSet<DepartmentProductRecord>();
            this.DepartmentProductSalesPlan = new HashSet<DepartmentProductSalesPlan>();
            this.SupplierContract = new HashSet<SupplierContract>();
            this.ProcureOrderAppDetail = new HashSet<ProcureOrderAppDetail>();
            this.StockInDetail = new HashSet<StockInDetail>();
            this.DaBaoRequestAppDetail = new HashSet<DaBaoRequestAppDetail>();
            this.InventoryHistory = new HashSet<InventoryHistory>();
            this.ClientInfoProductSetting = new HashSet<ClientInfoProductSetting>();
            this.ProductSpecification = new HashSet<ProductSpecification>();
            this.ProductHighPrice = new HashSet<ProductHighPrice>();
            this.ClientRefundAppDetail = new HashSet<ClientRefundAppDetail>();
            this.FactoryManagerRefundApplication = new HashSet<FactoryManagerRefundApplication>();
            this.SupplierRefundApplication = new HashSet<SupplierRefundApplication>();
            this.ClientTaskRefundApplication = new HashSet<ClientTaskRefundApplication>();
            this.DCFlowData = new HashSet<DCFlowData>();
            this.ClientFlowData = new HashSet<ClientFlowData>();
            this.StockOutDetail = new HashSet<StockOutDetail>();
            this.DBClientBonus = new HashSet<DBClientBonus>();
            this.DBContract = new HashSet<DBContract>();
            this.SalesOrderAppDetail = new HashSet<SalesOrderAppDetail>();
            this.SupplierCautionMoney = new HashSet<SupplierCautionMoney>();
            this.DCInventoryData = new HashSet<DCInventoryData>();
        }
    
        public int ID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public Nullable<bool> IsManagedByBatchNumber { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public Nullable<int> SafetyStock { get; set; }
        public Nullable<int> ValidDays { get; set; }
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
    
    
        public virtual Company Company { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<ProductCertificate> ProductCertificate { get; set; }
        public virtual ICollection<DeptProductEvaluation> DeptProductEvaluation { get; set; }
        public virtual ICollection<ProductBasicPrice> ProductBasicPrice { get; set; }
        public virtual ICollection<ProductDBPolicyPrice> ProductDBPolicyPrice { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<DeptMarketProduct> DeptMarketProduct { get; set; }
        public virtual ICollection<DepartmentProductRecord> DepartmentProductRecord { get; set; }
        public virtual ICollection<DepartmentProductSalesPlan> DepartmentProductSalesPlan { get; set; }
        public virtual ICollection<SupplierContract> SupplierContract { get; set; }
        public virtual ICollection<ProcureOrderAppDetail> ProcureOrderAppDetail { get; set; }
        public virtual ICollection<StockInDetail> StockInDetail { get; set; }
        public virtual ICollection<DaBaoRequestAppDetail> DaBaoRequestAppDetail { get; set; }
        public virtual ICollection<InventoryHistory> InventoryHistory { get; set; }
        public virtual ICollection<ClientInfoProductSetting> ClientInfoProductSetting { get; set; }
        public virtual ICollection<ProductSpecification> ProductSpecification { get; set; }
        public virtual ICollection<ProductHighPrice> ProductHighPrice { get; set; }
        public virtual ICollection<ClientRefundAppDetail> ClientRefundAppDetail { get; set; }
        public virtual ICollection<FactoryManagerRefundApplication> FactoryManagerRefundApplication { get; set; }
        public virtual ICollection<SupplierRefundApplication> SupplierRefundApplication { get; set; }
        public virtual ICollection<ClientTaskRefundApplication> ClientTaskRefundApplication { get; set; }
        public virtual ICollection<DCFlowData> DCFlowData { get; set; }
        public virtual ICollection<ClientFlowData> ClientFlowData { get; set; }
        public virtual ICollection<StockOutDetail> StockOutDetail { get; set; }
        public virtual ICollection<DBClientBonus> DBClientBonus { get; set; }
        public virtual ICollection<DBContract> DBContract { get; set; }
        public virtual ICollection<SalesOrderAppDetail> SalesOrderAppDetail { get; set; }
        public virtual ICollection<SupplierCautionMoney> SupplierCautionMoney { get; set; }
        public virtual ICollection<DCInventoryData> DCInventoryData { get; set; }
    }
}
