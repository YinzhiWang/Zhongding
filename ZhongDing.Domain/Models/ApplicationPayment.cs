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
    public partial class ApplicationPayment : IEntityExtendedProperty
    {
        public int ID { get; set; }
        public int ApplicationID { get; set; }
        public int WorkflowID { get; set; }
        public Nullable<int> FromBankAccountID { get; set; }
        public string FromAccount { get; set; }
        public Nullable<int> ToBankAccountID { get; set; }
        public string ToAccount { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Fee { get; set; }
        public int PaymentTypeID { get; set; }
        public string Comment { get; set; }
        public int PaymentStatusID { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> PayDate { get; set; }
    
    	// Implements IEntityExtendedProperty
    	public string DefaultOrderColumnName { get { return "id"; } }
    	public bool HasColumnIsDeleted { get { return true; } }
    	public bool HasColumnDeletedOn { get { return false; } }
    	public bool HasColumnCreatedOn { get { return true; } }
    	public bool HasColumnCreatedBy { get { return true; } }
    	public bool HasColumnLastModifiedOn { get { return true; } }
    	public bool HasColumnLastModifiedBy { get { return true; } }
    
    
        public virtual BankAccount BankAccount { get; set; }
        public virtual PaymentStatus PaymentStatus { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual BankAccount BankAccount1 { get; set; }
        public virtual Workflow Workflow { get; set; }
    }
}
