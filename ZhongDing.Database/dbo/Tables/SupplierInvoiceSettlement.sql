CREATE TABLE [dbo].[SupplierInvoiceSettlement] (
    [ID]                 INT             IDENTITY (1, 1) NOT NULL,
    [SettlementDate]     DATETIME        NOT NULL,
    [CompanyID]          INT             NOT NULL,
    [TaxRatio]           DECIMAL (18, 8) NOT NULL,
    [ToBankAccountID]    INT             NOT NULL,
    [ToAccount]          NVARCHAR (255)  NULL,
    [TotalInvoiceAmount] MONEY           NOT NULL,
    [TotalPayAmount]     MONEY           NOT NULL,
    [IsCanceled]         BIT             CONSTRAINT [DF_SupplierInvoiceSettlement_IsCanceled] DEFAULT ((0)) NOT NULL,
    [CanceledReason]     NVARCHAR (1000) NULL,
    [CanceledDate]       DATETIME        NULL,
    [CanceledBy]         INT             NULL,
    [IsDeleted]          BIT             NOT NULL,
    [CreatedOn]          DATETIME        NOT NULL,
    [CreatedBy]          INT             NULL,
    [LastModifiedOn]     DATETIME        NULL,
    [LastModifiedBy]     INT             NULL,
    CONSTRAINT [PK_SupplierInvoiceSettlement] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierInvoiceSettlement_BankAccount] FOREIGN KEY ([ToBankAccountID]) REFERENCES [dbo].[BankAccount] ([ID]),
    CONSTRAINT [FK_SupplierInvoiceSettlement_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID])
);



