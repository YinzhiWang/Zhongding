CREATE TABLE [dbo].[ClientInvoiceDetail] (
    [ID]               INT      IDENTITY (1, 1) NOT NULL,
    [ClientInvoiceID]  INT      NOT NULL,
    [StockOutDetailID] INT      NOT NULL,
    [InvoiceTypeID]    INT      NOT NULL,
    [Amount]           MONEY    NOT NULL,
    [IsDeleted]        BIT      NOT NULL,
    [CreatedOn]        DATETIME NOT NULL,
    [CreatedBy]        INT      NULL,
    [LastModifiedOn]   DATETIME NULL,
    [LastModifiedBy]   INT      NULL,
    CONSTRAINT [PK__ClientIn__3214EC2701CC17B0] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientInvoiceDetail_ClientInvoiceID] FOREIGN KEY ([ClientInvoiceID]) REFERENCES [dbo].[ClientInvoice] ([ID]),
    CONSTRAINT [FK_ClientInvoiceDetail_InvoiceTypeID] FOREIGN KEY ([InvoiceTypeID]) REFERENCES [dbo].[InvoiceType] ([ID]),
    CONSTRAINT [FK_ClientInvoiceDetail_StockOutDetailID] FOREIGN KEY ([StockOutDetailID]) REFERENCES [dbo].[StockOutDetail] ([ID])
);



