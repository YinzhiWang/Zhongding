CREATE TABLE [dbo].[DaBaoApplication] (
    [ID]                      INT            IDENTITY (1, 1) NOT NULL,
    [SalesOrderApplicationID] INT            NULL,
    [DistributionCompanyID]   INT            NULL,
    [DepartmentID]            INT            NULL,
    [ReceiverName]            NVARCHAR (50)  NULL,
    [ReceiverPhone]           NVARCHAR (50)  NULL,
    [ReceiverAddress]         NVARCHAR (500) NULL,
    [WorkflowStatusID]        INT            NULL,
    [IsDeleted]               BIT            NULL,
    [CreatedOn]               DATETIME       NULL,
    [CreatedBy]               INT            NULL,
    [LastModifiedOn]          DATETIME       NULL,
    [LastModifiedBy]          INT            NULL,
    CONSTRAINT [PK_DaBaoApplication] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DaBaoApplication_DistributionCompany] FOREIGN KEY ([DistributionCompanyID]) REFERENCES [dbo].[DistributionCompany] ([ID]),
    CONSTRAINT [FK_DaBaoApplication_SalesOrderApplication] FOREIGN KEY ([SalesOrderApplicationID]) REFERENCES [dbo].[SalesOrderApplication] ([ID]),
    CONSTRAINT [FK_DaBaoApplication_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);

