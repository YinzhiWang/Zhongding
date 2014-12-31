CREATE TABLE [dbo].[DaBaoApplication] (
    [ID]                      INT            IDENTITY (1, 1) NOT NULL,
    [SalesOrderApplicationID] INT            NOT NULL,
    [DistributionCompanyID]   INT            NOT NULL,
    [DepartmentID]            INT            NOT NULL,
    [CompanyID]               INT            NOT NULL,
    [ReceiverName]            NVARCHAR (50)  NULL,
    [ReceiverPhone]           NVARCHAR (50)  NULL,
    [ReceiverAddress]         NVARCHAR (500) NULL,
    [WorkflowStatusID]        INT            NOT NULL,
    [IsDeleted]               BIT            CONSTRAINT [DF_DaBaoApplication_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]               DATETIME       CONSTRAINT [DF_DaBaoApplication_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]               INT            NULL,
    [LastModifiedOn]          DATETIME       NULL,
    [LastModifiedBy]          INT            NULL,
    CONSTRAINT [PK_DaBaoApplication] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DaBaoApplication_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_DaBaoApplication_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([ID]),
    CONSTRAINT [FK_DaBaoApplication_DistributionCompany] FOREIGN KEY ([DistributionCompanyID]) REFERENCES [dbo].[DistributionCompany] ([ID]),
    CONSTRAINT [FK_DaBaoApplication_SalesOrderApplication] FOREIGN KEY ([SalesOrderApplicationID]) REFERENCES [dbo].[SalesOrderApplication] ([ID]),
    CONSTRAINT [FK_DaBaoApplication_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);



