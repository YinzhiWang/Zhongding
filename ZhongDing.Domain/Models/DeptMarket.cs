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
    public partial class DeptMarket : IEntityExtendedProperty
    {
        public DeptMarket()
        {
            this.ClientInfoProductSetting = new HashSet<ClientInfoProductSetting>();
            this.ClientFlowData = new HashSet<ClientFlowData>();
        }
    
        public int ID { get; set; }
        public int DeptDistrictID { get; set; }
        public string MarketName { get; set; }
    
    	// Implements IEntityExtendedProperty
    	public string DefaultOrderColumnName { get { return "id"; } }
    	public bool HasColumnIsDeleted { get { return false; } }
    	public bool HasColumnDeletedOn { get { return false; } }
    	public bool HasColumnCreatedOn { get { return false; } }
    	public bool HasColumnCreatedBy { get { return false; } }
    	public bool HasColumnLastModifiedOn { get { return false; } }
    	public bool HasColumnLastModifiedBy { get { return false; } }
    
    
        public virtual DeptDistrict DeptDistrict { get; set; }
        public virtual ICollection<ClientInfoProductSetting> ClientInfoProductSetting { get; set; }
        public virtual ICollection<ClientFlowData> ClientFlowData { get; set; }
    }
}
