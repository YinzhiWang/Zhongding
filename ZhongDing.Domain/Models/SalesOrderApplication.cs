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
    public partial class SalesOrderApplication : IEntityExtendedProperty
    {
        public SalesOrderApplication()
        {
            this.DaBaoApplication = new HashSet<DaBaoApplication>();
            this.StockOutDetail = new HashSet<StockOutDetail>();
            this.ClientSaleApplication = new HashSet<ClientSaleApplication>();
            this.SalesOrderAppDetail = new HashSet<SalesOrderAppDetail>();
        }
    
        public int ID { get; set; }
        public int SaleOrderTypeID { get; set; }
        public string OrderCode { get; set; }
        public System.DateTime OrderDate { get; set; }
        public bool IsStop { get; set; }
        public Nullable<System.DateTime> StoppedOn { get; set; }
        public Nullable<int> StoppedBy { get; set; }
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
        public virtual SaleOrderType SaleOrderType { get; set; }
        public virtual ICollection<StockOutDetail> StockOutDetail { get; set; }
        public virtual ICollection<ClientSaleApplication> ClientSaleApplication { get; set; }
        public virtual ICollection<SalesOrderAppDetail> SalesOrderAppDetail { get; set; }
    }
}