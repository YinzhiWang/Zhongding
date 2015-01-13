CREATE TABLE [dbo].[StockOut] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [CompanyID]             INT            NOT NULL,
    [ReceiverTypeID]        INT            NOT NULL,
    [Code]                  NVARCHAR (50)  NULL,
    [BillDate]              DATETIME       NOT NULL,
    [OutDate]               DATETIME       NULL,
    [DistributionCompanyID] INT            NULL,
    [ClientUserID]          INT            NULL,
    [ClientCompanyID]       INT            NULL,
    [InvoiceTypeID]         INT            NULL,
    [WorkflowStatusID]      INT            NOT NULL,
    [ReceiverName]          NVARCHAR (50)  NULL,
    [ReceiverPhone]         NVARCHAR (50)  NULL,
    [ReceiverAddress]       NVARCHAR (500) NULL,
    [IsDeleted]             BIT            CONSTRAINT [DF_StockOut_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]             DATETIME       CONSTRAINT [DF_StockOut_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]             INT            NULL,
    [LastModifiedOn]        DATETIME       NULL,
    [LastModifiedBy]        INT            NULL,
    CONSTRAINT [PK_StockOut] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_StockOut_ClientCompany] FOREIGN KEY ([ClientCompanyID]) REFERENCES [dbo].[ClientCompany] ([ID]),
    CONSTRAINT [FK_StockOut_ClientUser] FOREIGN KEY ([ClientUserID]) REFERENCES [dbo].[ClientUser] ([ID]),
    CONSTRAINT [FK_StockOut_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_StockOut_DistributionCompany] FOREIGN KEY ([DistributionCompanyID]) REFERENCES [dbo].[DistributionCompany] ([ID]),
    CONSTRAINT [FK_StockOut_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);





