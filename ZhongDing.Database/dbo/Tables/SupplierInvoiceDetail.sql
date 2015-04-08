CREATE TABLE [dbo].[SupplierInvoiceDetail] (
    [ID]                      INT      IDENTITY (1, 1) NOT NULL,
    [SupplierInvoiceID]       INT      NOT NULL,
    [ProcureOrderAppDetailID] INT      NOT NULL,
    [Amount]                  MONEY    NOT NULL,
    [IsDeleted]               BIT      NOT NULL,
    [CreatedOn]               DATETIME NOT NULL,
    [CreatedBy]               INT      NULL,
    [LastModifiedOn]          DATETIME NULL,
    [LastModifiedBy]          INT      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierInvoiceDetail_ProcureOrderAppDetailID] FOREIGN KEY ([ProcureOrderAppDetailID]) REFERENCES [dbo].[ProcureOrderAppDetail] ([ID]),
    CONSTRAINT [FK_SupplierInvoiceDetail_SupplierInvoiceID] FOREIGN KEY ([SupplierInvoiceID]) REFERENCES [dbo].[SupplierInvoice] ([ID])
);

