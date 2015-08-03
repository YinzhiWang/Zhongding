CREATE TABLE [dbo].[Reimbursement] (
    [ID]               INT      IDENTITY (1, 1) NOT NULL,
    [DepartmentID]     INT      NULL,
    [ApplyDate]        DATETIME NOT NULL,
    [WorkflowStatusID] INT      NOT NULL,
    [IsDeleted]        BIT      DEFAULT ((0)) NOT NULL,
    [CreatedOn]        DATETIME DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT      NULL,
    [LastModifiedOn]   DATETIME NULL,
    [LastModifiedBy]   INT      NULL,
    [PaidBy]           INT      NULL,
    [PaidDate]         DATETIME NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Reimbursement_DepartmentID] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([ID]),
    CONSTRAINT [FK_Reimbursement_WorkflowStatusID] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);



