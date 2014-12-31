CREATE TABLE [dbo].[DaBaoRequestApplication] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [DaBaoApplicationID]    INT            NULL,
    [DistributionCompanyID] INT            NOT NULL,
    [DepartmentID]          INT            NOT NULL,
    [CompanyID]             INT            NOT NULL,
    [ReceiverName]          NVARCHAR (50)  NULL,
    [ReceiverPhone]         NVARCHAR (50)  NULL,
    [ReceiverAddress]       NVARCHAR (500) NULL,
    [WorkflowStatusID]      INT            NOT NULL,
    [IsDeleted]             BIT            CONSTRAINT [DF_DaBaoRequestApplication_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]             DATETIME       CONSTRAINT [DF_DaBaoRequestApplication_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]             INT            NULL,
    [LastModifiedOn]        DATETIME       NULL,
    [LastModifiedBy]        INT            NULL,
    CONSTRAINT [PK_DaBaoRequestApplication] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DaBaoRequestApplication_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_DaBaoRequestApplication_DaBaoApplication] FOREIGN KEY ([DaBaoApplicationID]) REFERENCES [dbo].[DaBaoApplication] ([ID]),
    CONSTRAINT [FK_DaBaoRequestApplication_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([ID]),
    CONSTRAINT [FK_DaBaoRequestApplication_DistributionCompany] FOREIGN KEY ([DistributionCompanyID]) REFERENCES [dbo].[DistributionCompany] ([ID]),
    CONSTRAINT [FK_DaBaoRequestApplication_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);

