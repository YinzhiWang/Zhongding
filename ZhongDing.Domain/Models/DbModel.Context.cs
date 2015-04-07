﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DbModelContainer : DbContext
    {
        public DbModelContainer()
            : base("name=DbModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AccountType> AccountType { get; set; }
        public DbSet<aspnet_Membership> aspnet_Membership { get; set; }
        public DbSet<aspnet_Roles> aspnet_Roles { get; set; }
        public DbSet<aspnet_Users> aspnet_Users { get; set; }
        public DbSet<BankAccount> BankAccount { get; set; }
        public DbSet<Certificate> Certificate { get; set; }
        public DbSet<ClientInfoBankAccount> ClientInfoBankAccount { get; set; }
        public DbSet<ClientInfoContact> ClientInfoContact { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<OwnerType> OwnerType { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductCertificate> ProductCertificate { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<SaleType> SaleType { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<SupplierBankAccount> SupplierBankAccount { get; set; }
        public DbSet<SupplierTaskAssignment> SupplierTaskAssignment { get; set; }
        public DbSet<UnitOfMeasurement> UnitOfMeasurement { get; set; }
        public DbSet<UserBonus> UserBonus { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<DeptDistrict> DeptDistrict { get; set; }
        public DbSet<DistributionCompany> DistributionCompany { get; set; }
        public DbSet<ClientCompanyCertificate> ClientCompanyCertificate { get; set; }
        public DbSet<DeptProductEvaluation> DeptProductEvaluation { get; set; }
        public DbSet<SupplierCertificate> SupplierCertificate { get; set; }
        public DbSet<ClientCompany> ClientCompany { get; set; }
        public DbSet<ClientUser> ClientUser { get; set; }
        public DbSet<ClientInfo> ClientInfo { get; set; }
        public DbSet<ProductBasicPrice> ProductBasicPrice { get; set; }
        public DbSet<ProductDBPolicyPrice> ProductDBPolicyPrice { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<DeptMarketDivision> DeptMarketDivision { get; set; }
        public DbSet<DeptMarketProduct> DeptMarketProduct { get; set; }
        public DbSet<DeptMarket> DeptMarket { get; set; }
        public DbSet<DBContractTaskAssignment> DBContractTaskAssignment { get; set; }
        public DbSet<DepartmentProductRecord> DepartmentProductRecord { get; set; }
        public DbSet<DepartmentProductSalesBonus> DepartmentProductSalesBonus { get; set; }
        public DbSet<DepartmentProductSalesPlan> DepartmentProductSalesPlan { get; set; }
        public DbSet<SupplierContract> SupplierContract { get; set; }
        public DbSet<SupplierContractFile> SupplierContractFile { get; set; }
        public DbSet<DBContract> DBContract { get; set; }
        public DbSet<DBContractHospital> DBContractHospital { get; set; }
        public DbSet<Hospital> Hospital { get; set; }
        public DbSet<WorkflowStepUser> WorkflowStepUser { get; set; }
        public DbSet<WorkflowStep> WorkflowStep { get; set; }
        public DbSet<Workflow> Workflow { get; set; }
        public DbSet<WorkflowStatus> WorkflowStatus { get; set; }
        public DbSet<WorkflowStepStatus> WorkflowStepStatus { get; set; }
        public DbSet<ProcureOrderAppDetail> ProcureOrderAppDetail { get; set; }
        public DbSet<ProcureOrderApplication> ProcureOrderApplication { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }
        public DbSet<PaymentType> PaymentType { get; set; }
        public DbSet<StockIn> StockIn { get; set; }
        public DbSet<StockInDetail> StockInDetail { get; set; }
        public DbSet<DaBaoApplication> DaBaoApplication { get; set; }
        public DbSet<DaBaoRequestApplication> DaBaoRequestApplication { get; set; }
        public DbSet<DaBaoRequestAppDetail> DaBaoRequestAppDetail { get; set; }
        public DbSet<StockOut> StockOut { get; set; }
        public DbSet<InventoryHistory> InventoryHistory { get; set; }
        public DbSet<ApplicationNote> ApplicationNote { get; set; }
        public DbSet<NoteType> NoteType { get; set; }
        public DbSet<ClientInfoProductSetting> ClientInfoProductSetting { get; set; }
        public DbSet<ProductSpecification> ProductSpecification { get; set; }
        public DbSet<ClientSaleAppBankAccount> ClientSaleAppBankAccount { get; set; }
        public DbSet<GuaranteeLog> GuaranteeLog { get; set; }
        public DbSet<SaleOrderType> SaleOrderType { get; set; }
        public DbSet<SalesOrderApplication> SalesOrderApplication { get; set; }
        public DbSet<ClientSaleApplication> ClientSaleApplication { get; set; }
        public DbSet<SalesOrderAppDetail> SalesOrderAppDetail { get; set; }
        public DbSet<ProductHighPrice> ProductHighPrice { get; set; }
        public DbSet<ApplicationPayment> ApplicationPayment { get; set; }
        public DbSet<SupplierDeduction> SupplierDeduction { get; set; }
        public DbSet<ClientRefundApplication> ClientRefundApplication { get; set; }
        public DbSet<ClientRefundAppDetail> ClientRefundAppDetail { get; set; }
        public DbSet<FactoryManagerRefundApplication> FactoryManagerRefundApplication { get; set; }
        public DbSet<SupplierRefundApplication> SupplierRefundApplication { get; set; }
        public DbSet<ClientTaskRefundApplication> ClientTaskRefundApplication { get; set; }
        public DbSet<TransportCompany> TransportCompany { get; set; }
        public DbSet<ImportDataType> ImportDataType { get; set; }
        public DbSet<ImportStatus> ImportStatus { get; set; }
        public DbSet<ImportErrorLog> ImportErrorLog { get; set; }
        public DbSet<TransportFee> TransportFee { get; set; }
        public DbSet<TransportFeeStockIn> TransportFeeStockIn { get; set; }
        public DbSet<TransportFeeStockOut> TransportFeeStockOut { get; set; }
        public DbSet<DCFlowData> DCFlowData { get; set; }
        public DbSet<DCFlowDataDetail> DCFlowDataDetail { get; set; }
        public DbSet<ImportFileLog> ImportFileLog { get; set; }
        public DbSet<CertificateType> CertificateType { get; set; }
        public DbSet<ClientFlowData> ClientFlowData { get; set; }
        public DbSet<HospitalType> HospitalType { get; set; }
        public DbSet<TransportFeeStockOutSmsReminder> TransportFeeStockOutSmsReminder { get; set; }
        public DbSet<ClientImportFileLog> ClientImportFileLog { get; set; }
        public DbSet<DCInventoryData> DCInventoryData { get; set; }
        public DbSet<DCImportFileLog> DCImportFileLog { get; set; }
        public DbSet<StockOutDetail> StockOutDetail { get; set; }
    }
}
