CREATE TABLE [dbo].[SalarySettle] (
    [ID]               INT      IDENTITY (1, 1) NOT NULL,
    [SettleDate]       DATETIME NOT NULL,
    [DepartmentID]     INT      NOT NULL,
    [WorkflowStatusID] INT      NOT NULL,
    [IsDeleted]        BIT      DEFAULT ((0)) NOT NULL,
    [CreatedOn]        DATETIME DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT      NULL,
    [LastModifiedOn]   DATETIME NULL,
    [LastModifiedBy]   INT      NULL,
    [PaidBy]           INT      NULL,
    [PaidDate]         DATETIME NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SalarySettle_DepartmentID] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([ID]),
    CONSTRAINT [FK_SalarySettle_WorkflowStatusID] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);

