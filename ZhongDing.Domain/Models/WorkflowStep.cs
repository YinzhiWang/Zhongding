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
    public partial class WorkflowStep : IEntityExtendedProperty
    {
        public WorkflowStep()
        {
            this.WorkflowStepUser = new HashSet<WorkflowStepUser>();
            this.WorkflowStepStatus = new HashSet<WorkflowStepStatus>();
            this.ApplicationNote = new HashSet<ApplicationNote>();
        }
    
        public int ID { get; set; }
        public int WorkflowID { get; set; }
        public string StepName { get; set; }
        public bool IsDeleted { get; set; }
    
    	// Implements IEntityExtendedProperty
    	public string DefaultOrderColumnName { get { return "id"; } }
    	public bool HasColumnIsDeleted { get { return true; } }
    	public bool HasColumnDeletedOn { get { return false; } }
    	public bool HasColumnCreatedOn { get { return false; } }
    	public bool HasColumnCreatedBy { get { return false; } }
    	public bool HasColumnLastModifiedOn { get { return false; } }
    	public bool HasColumnLastModifiedBy { get { return false; } }
    
    
        public virtual ICollection<WorkflowStepUser> WorkflowStepUser { get; set; }
        public virtual Workflow Workflow { get; set; }
        public virtual ICollection<WorkflowStepStatus> WorkflowStepStatus { get; set; }
        public virtual ICollection<ApplicationNote> ApplicationNote { get; set; }
    }
}
