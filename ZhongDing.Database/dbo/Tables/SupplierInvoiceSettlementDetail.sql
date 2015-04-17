CREATE TABLE [dbo].[SupplierInvoiceSettlementDetail] (
    [ID]                          INT            IDENTITY (1, 1) NOT NULL,
    [SupplierInvoiceSettlementID] INT            NOT NULL,
    [SupplierID]                  INT            NOT NULL,
    [SupplierInvoiceID]           INT            NOT NULL,
    [InvoiceDate]                 DATETIME       NOT NULL,
    [InvoiceNumber]               NVARCHAR (256) NOT NULL,
    [InvoiceAmount]               MONEY          NOT NULL,
    [AppPaymentID]                INT            NULL,
    [CanceledAppPaymentID]        INT            NULL,
    [PayAmount]                   MONEY          NOT NULL,
    [IsDeleted]                   BIT            NOT NULL,
    [CreatedOn]                   DATETIME       NOT NULL,
    [CreatedBy]                   INT            NULL,
    [LastModifiedOn]              DATETIME       NULL,
    [LastModifiedBy]              INT            NULL,
    CONSTRAINT [PK_SupplierInvoiceSettlementDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierInvoiceSettlementDetail_ApplicationPayment] FOREIGN KEY ([AppPaymentID]) REFERENCES [dbo].[ApplicationPayment] ([ID]),
    CONSTRAINT [FK_SupplierInvoiceSettlementDetail_CanceledApplicationPayment] FOREIGN KEY ([CanceledAppPaymentID]) REFERENCES [dbo].[ApplicationPayment] ([ID]),
    CONSTRAINT [FK_SupplierInvoiceSettlementDetail_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID]),
    CONSTRAINT [FK_SupplierInvoiceSettlementDetail_SupplierInvoice] FOREIGN KEY ([SupplierInvoiceID]) REFERENCES [dbo].[SupplierInvoice] ([ID]),
    CONSTRAINT [FK_SupplierInvoiceSettlementDetail_SupplierInvoiceSettlement] FOREIGN KEY ([SupplierInvoiceSettlementID]) REFERENCES [dbo].[SupplierInvoiceSettlement] ([ID])
);



