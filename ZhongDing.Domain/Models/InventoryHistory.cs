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
    public partial class InventoryHistory : IEntityExtendedProperty
    {
        public int ID { get; set; }
        public int WarehouseID { get; set; }
        public int ProductID { get; set; }
        public int ProductSpecificationID { get; set; }
        public string LicenseNumber { get; set; }
        public string BatchNumber { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public decimal ProcurePrice { get; set; }
        public int InQty { get; set; }
        public int OutQty { get; set; }
        public int BalanceQty { get; set; }
        public System.DateTime StatDate { get; set; }
        public System.DateTime CreatedOn { get; set; }
    
    	// Implements IEntityExtendedProperty
    	public string DefaultOrderColumnName { get { return "id"; } }
    	public bool HasColumnIsDeleted { get { return false; } }
    	public bool HasColumnDeletedOn { get { return false; } }
    	public bool HasColumnCreatedOn { get { return true; } }
    	public bool HasColumnCreatedBy { get { return false; } }
    	public bool HasColumnLastModifiedOn { get { return false; } }
    	public bool HasColumnLastModifiedBy { get { return false; } }
    
    
        public virtual Product Product { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual ProductSpecification ProductSpecification { get; set; }
    }
}
