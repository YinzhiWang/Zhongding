CREATE TABLE [dbo].[DBClientInvoiceSettlement] (
    [ID]                    INT             IDENTITY (1, 1) NOT NULL,
    [CompanyID]             INT             NOT NULL,
    [DistributionCompanyID] INT             NOT NULL,
    [ReceiveDate]           DATETIME        NOT NULL,
    [ReceiveBankAccountID]  INT             NOT NULL,
    [TotalInvoiceAmount]    MONEY           NOT NULL,
    [TotalReceiveAmount]    MONEY           NOT NULL,
    [ConfirmDate]           DATETIME        NULL,
    [IsCanceled]            BIT             CONSTRAINT [DF_DBClientInvoiceSettlement_IsCanceled] DEFAULT ((0)) NOT NULL,
    [CanceledReason]        NVARCHAR (1000) NULL,
    [CanceledDate]          DATETIME        NULL,
    [CanceledBy]            INT             NULL,
    [IsDeleted]             BIT             NOT NULL,
    [CreatedOn]             DATETIME        NOT NULL,
    [CreatedBy]             INT             NULL,
    [LastModifiedOn]        DATETIME        NULL,
    [LastModifiedBy]        INT             NULL,
    CONSTRAINT [PK_DBClientInvoiceSettlement] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DBClientInvoiceSettlement_BankAccount] FOREIGN KEY ([ReceiveBankAccountID]) REFERENCES [dbo].[BankAccount] ([ID]),
    CONSTRAINT [FK_DBClientInvoiceSettlement_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_DBClientInvoiceSettlement_DistributionCompany] FOREIGN KEY ([DistributionCompanyID]) REFERENCES [dbo].[DistributionCompany] ([ID])
);

