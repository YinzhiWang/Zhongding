CREATE TABLE [dbo].[ClientAttachedInvoiceSettlementDetail] (
    [ID]                                INT      IDENTITY (1, 1) NOT NULL,
    [ClientAttachedInvoiceSettlementID] INT      NOT NULL,
    [ClientInvoiceDetailID]             INT      NOT NULL,
    [StockOutDetailID]                  INT      NOT NULL,
    [InvoiceQty]                        INT      NOT NULL,
    [SettlementQty]                     INT      NULL,
    [SalesAmount]                       MONEY    NULL,
    [SettlementAmount]                  MONEY    NULL,
    [IsDeleted]                         BIT      NOT NULL,
    [CreatedOn]                         DATETIME NOT NULL,
    [CreatedBy]                         INT      NULL,
    [LastModifiedOn]                    DATETIME NULL,
    [LastModifiedBy]                    INT      NULL,
    CONSTRAINT [PK_ClientAttachedInvoiceSettlementDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientAttachedInvoiceSettlementDetail_ClientAttachedInvoiceSettlement] FOREIGN KEY ([ClientAttachedInvoiceSettlementID]) REFERENCES [dbo].[ClientAttachedInvoiceSettlement] ([ID]),
    CONSTRAINT [FK_ClientAttachedInvoiceSettlementDetail_ClientInvoiceDetail] FOREIGN KEY ([ClientInvoiceDetailID]) REFERENCES [dbo].[ClientInvoiceDetail] ([ID]),
    CONSTRAINT [FK_ClientAttachedInvoiceSettlementDetail_StockOutDetail] FOREIGN KEY ([StockOutDetailID]) REFERENCES [dbo].[StockOutDetail] ([ID])
);



