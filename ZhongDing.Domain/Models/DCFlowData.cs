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
    public partial class DCFlowData : IEntityExtendedProperty
    {
        public DCFlowData()
        {
            this.DCFlowDataDetail = new HashSet<DCFlowDataDetail>();
        }
    
        public int ID { get; set; }
        public Nullable<int> DistributionCompanyID { get; set; }
        public int ImportFileLogID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int ProductSpecificationID { get; set; }
        public string ProductSpecification { get; set; }
        public System.DateTime SaleDate { get; set; }
        public int SaleQty { get; set; }
        public System.DateTime SettlementDate { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
    
    	// Implements IEntityExtendedProperty
    	public string DefaultOrderColumnName { get { return "id"; } }
    	public bool HasColumnIsDeleted { get { return false; } }
    	public bool HasColumnDeletedOn { get { return false; } }
    	public bool HasColumnCreatedOn { get { return true; } }
    	public bool HasColumnCreatedBy { get { return true; } }
    	public bool HasColumnLastModifiedOn { get { return false; } }
    	public bool HasColumnLastModifiedBy { get { return false; } }
    
    
        public virtual DistributionCompany DistributionCompany { get; set; }
        public virtual ImportFileLog ImportFileLog { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductSpecification ProductSpecification1 { get; set; }
        public virtual ICollection<DCFlowDataDetail> DCFlowDataDetail { get; set; }
    }
}
