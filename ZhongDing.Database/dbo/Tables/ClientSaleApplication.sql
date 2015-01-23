CREATE TABLE [dbo].[ClientSaleApplication] (
    [ID]                      INT            IDENTITY (1, 1) NOT NULL,
    [SalesOrderApplicationID] INT            NOT NULL,
    [ClientUserID]            INT            NOT NULL,
    [ClientCompanyID]         INT            NOT NULL,
    [DeliveryModeID]          INT            NULL,
    [ClientContactID]         INT            NOT NULL,
    [Guaranteeby]             INT            NULL,
    [IsDeleted]               BIT            CONSTRAINT [DF_ClientSaleApplication_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]               DATETIME       CONSTRAINT [DF_ClientSaleApplication_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]               INT            NULL,
    [LastModifiedOn]          DATETIME       NULL,
    [LastModifiedBy]          INT            NULL,
    [WorkflowStatusID]        INT            NOT NULL,
    [CompanyID]               INT            NOT NULL,
    [IsGuaranteed]            BIT            CONSTRAINT [DF_ClientSaleApplication_IsGuaranteeTransaction] DEFAULT ((0)) NOT NULL,
    [GuaranteeExpirationDate] DATETIME       NULL,
    [ClientContactPhone]      NVARCHAR (50)  NULL,
    [ReceiverName]            NVARCHAR (50)  NULL,
    [ReceiverPhone]           NVARCHAR (50)  NULL,
    [ReceiverAddress]         NVARCHAR (500) NULL,
    [ReceiverFax]             NVARCHAR (50)  NULL,
    CONSTRAINT [PK_ClientSaleApplication] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientSaleApplication_ClientCompany] FOREIGN KEY ([ClientCompanyID]) REFERENCES [dbo].[ClientCompany] ([ID]),
    CONSTRAINT [FK_ClientSaleApplication_ClientInfoContact] FOREIGN KEY ([ClientContactID]) REFERENCES [dbo].[ClientInfoContact] ([ID]),
    CONSTRAINT [FK_ClientSaleApplication_ClientUser] FOREIGN KEY ([ClientUserID]) REFERENCES [dbo].[ClientUser] ([ID]),
    CONSTRAINT [FK_ClientSaleApplication_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_ClientSaleApplication_SalesOrderApplication] FOREIGN KEY ([SalesOrderApplicationID]) REFERENCES [dbo].[SalesOrderApplication] ([ID]),
    CONSTRAINT [FK_ClientSaleApplication_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);

















