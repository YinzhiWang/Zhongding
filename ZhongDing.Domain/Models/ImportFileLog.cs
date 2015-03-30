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
    public partial class ImportFileLog : IEntityExtendedProperty
    {
        public ImportFileLog()
        {
            this.DCFlowData = new HashSet<DCFlowData>();
            this.ImportErrorLog = new HashSet<ImportErrorLog>();
            this.ClientFlowData = new HashSet<ClientFlowData>();
            this.DCInventoryFlowData = new HashSet<DCInventoryFlowData>();
        }
    
        public int ID { get; set; }
        public int ImportDataTypeID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Nullable<System.DateTime> ImportBeginDate { get; set; }
        public Nullable<System.DateTime> ImportEndDate { get; set; }
        public int ImportStatusID { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public Nullable<int> SucceedCount { get; set; }
        public Nullable<int> FailedCount { get; set; }
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
    
    
        public virtual ICollection<DCFlowData> DCFlowData { get; set; }
        public virtual DCImportFileLog DCImportFileLog { get; set; }
        public virtual ImportDataType ImportDataType { get; set; }
        public virtual ICollection<ImportErrorLog> ImportErrorLog { get; set; }
        public virtual ImportStatus ImportStatus { get; set; }
        public virtual ICollection<ClientFlowData> ClientFlowData { get; set; }
        public virtual ICollection<DCInventoryFlowData> DCInventoryFlowData { get; set; }
        public virtual ClientImportFileLog ClientImportFileLog { get; set; }
    }
}
