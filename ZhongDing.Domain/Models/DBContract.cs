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
    public partial class DBContract : IEntityExtendedProperty
    {
        public DBContract()
        {
            this.DBContractHospital = new HashSet<DBContractHospital>();
            this.DBContractTaskAssignment = new HashSet<DBContractTaskAssignment>();
            this.DCFlowDataDetail = new HashSet<DCFlowDataDetail>();
        }
    
        public int ID { get; set; }
        public string ContractCode { get; set; }
        public Nullable<int> ClientUserID { get; set; }
        public Nullable<bool> IsTempContract { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public Nullable<int> InChargeUserID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> ProductSpecificationID { get; set; }
        public Nullable<double> PromotionExpense { get; set; }
        public Nullable<System.DateTime> ContractExpDate { get; set; }
        public Nullable<bool> IsNew { get; set; }
        public Nullable<int> HospitalTypeID { get; set; }
        public string Comment { get; set; }
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
    
    
        public virtual ClientUser ClientUser { get; set; }
        public virtual Department Department { get; set; }
        public virtual Product Product { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<DBContractHospital> DBContractHospital { get; set; }
        public virtual ICollection<DBContractTaskAssignment> DBContractTaskAssignment { get; set; }
        public virtual ProductSpecification ProductSpecification { get; set; }
        public virtual ICollection<DCFlowDataDetail> DCFlowDataDetail { get; set; }
    }
}
