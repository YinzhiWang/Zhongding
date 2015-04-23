CREATE TABLE [dbo].[DBClientInvoiceSettlementDetail] (
    [ID]                          INT            IDENTITY (1, 1) NOT NULL,
    [DBClientInvoiceSettlementID] INT            NOT NULL,
    [DBClientInvoiceID]           INT            NOT NULL,
    [InvoiceDate]                 DATETIME       NOT NULL,
    [InvoiceNumber]               NVARCHAR (256) NOT NULL,
    [InvoiceAmount]               MONEY          NOT NULL,
    [ReceiveAmount]               MONEY          NOT NULL,
    [AppPaymentID]                INT            NULL,
    [CanceledAppPaymentID]        INT            NULL,
    [IsDeleted]                   BIT            NOT NULL,
    [CreatedOn]                   DATETIME       NOT NULL,
    [CreatedBy]                   INT            NULL,
    [LastModifiedOn]              DATETIME       NULL,
    [LastModifiedBy]              INT            NULL,
    CONSTRAINT [PK_DBClientInvoiceSettlementDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DBClientInvoiceSettlementDetail_ApplicationPayment] FOREIGN KEY ([AppPaymentID]) REFERENCES [dbo].[ApplicationPayment] ([ID]),
    CONSTRAINT [FK_DBClientInvoiceSettlementDetail_CanceledApplicationPayment] FOREIGN KEY ([CanceledAppPaymentID]) REFERENCES [dbo].[ApplicationPayment] ([ID]),
    CONSTRAINT [FK_DBClientInvoiceSettlementDetail_DBClientInvoice] FOREIGN KEY ([DBClientInvoiceID]) REFERENCES [dbo].[DBClientInvoice] ([ID]),
    CONSTRAINT [FK_DBClientInvoiceSettlementDetail_DBClientInvoiceSettlement] FOREIGN KEY ([DBClientInvoiceSettlementID]) REFERENCES [dbo].[DBClientInvoiceSettlement] ([ID])
);

