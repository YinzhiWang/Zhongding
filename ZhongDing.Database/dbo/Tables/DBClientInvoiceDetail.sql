CREATE TABLE [dbo].[DBClientInvoiceDetail] (
    [ID]                INT      IDENTITY (1, 1) NOT NULL,
    [DBClientInvoiceID] INT      NOT NULL,
    [StockOutDetailID]  INT      NOT NULL,
    [Amount]            MONEY    NOT NULL,
    [IsDeleted]         BIT      NOT NULL,
    [CreatedOn]         DATETIME NOT NULL,
    [CreatedBy]         INT      NULL,
    [LastModifiedOn]    DATETIME NULL,
    [LastModifiedBy]    INT      NULL,
    CONSTRAINT [PK__DBClientInvoiceDetail__ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DBClientInvoiceDetail_DBClientInvoiceID] FOREIGN KEY ([DBClientInvoiceID]) REFERENCES [dbo].[DBClientInvoice] ([ID]),
    CONSTRAINT [FK_DBClientInvoiceDetail_StockOutDetailID] FOREIGN KEY ([StockOutDetailID]) REFERENCES [dbo].[StockOutDetail] ([ID])
);

