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
    public partial class StockInDetailImportData : IEntityExtendedProperty
    {
        public int ID { get; set; }
        public int StockInImportDataID { get; set; }
        public int ProcureOrderAppID { get; set; }
        public int ProcureOrderAppDetailID { get; set; }
        public string ProductName { get; set; }
        public string ProductSpecification { get; set; }
        public string WarehouseName { get; set; }
        public decimal ProcurePrice { get; set; }
        public int InQty { get; set; }
        public string BatchNumber { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public string LicenseNumber { get; set; }
        public Nullable<bool> IsMortgagedProduct { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public string UnitOfMeasurement { get; set; }
    
    	// Implements IEntityExtendedProperty
    	public string DefaultOrderColumnName { get { return "id"; } }
    	public bool HasColumnIsDeleted { get { return false; } }
    	public bool HasColumnDeletedOn { get { return false; } }
    	public bool HasColumnCreatedOn { get { return true; } }
    	public bool HasColumnCreatedBy { get { return true; } }
    	public bool HasColumnLastModifiedOn { get { return true; } }
    	public bool HasColumnLastModifiedBy { get { return true; } }
    
    
        public virtual StockInImportData StockInImportData { get; set; }
    }
}
