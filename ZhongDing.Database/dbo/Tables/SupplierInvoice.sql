CREATE TABLE [dbo].[SupplierInvoice] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [CompanyID]      INT            NOT NULL,
    [SupplierID]     INT            NOT NULL,
    [InvoiceDate]    DATETIME       NOT NULL,
    [InvoiceNumber]  NVARCHAR (256) NOT NULL,
    [Amount]         MONEY          NOT NULL,
    [IsDeleted]      BIT            NOT NULL,
    [CreatedOn]      DATETIME       NOT NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    CONSTRAINT [PK__Supplier__3214EC273AC39319] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierInvoice_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_SupplierInvoice_SupplierID] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID])
);

