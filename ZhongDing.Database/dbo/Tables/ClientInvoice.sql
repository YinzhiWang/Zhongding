CREATE TABLE [dbo].[ClientInvoice] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [CompanyID]        INT            NOT NULL,
    [ClientCompanyID]  INT            NOT NULL,
    [InvoiceDate]      DATETIME       NOT NULL,
    [InvoiceNumber]    NVARCHAR (256) NOT NULL,
    [Amount]           MONEY          NOT NULL,
    [TransportNumber]  NVARCHAR (256) NOT NULL,
    [TransportCompany] NVARCHAR (256) NOT NULL,
    [SaleOrderTypeID]  INT            NOT NULL,
    [IsDeleted]        BIT            NOT NULL,
    [CreatedOn]        DATETIME       NOT NULL,
    [CreatedBy]        INT            NULL,
    [LastModifiedOn]   DATETIME       NULL,
    [LastModifiedBy]   INT            NULL,
    CONSTRAINT [PK__ClientIn__3214EC2758440941] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientInvoice_ClientCompanyID] FOREIGN KEY ([ClientCompanyID]) REFERENCES [dbo].[ClientCompany] ([ID]),
    CONSTRAINT [FK_ClientInvoice_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_ClientInvoice_SaleOrderTypeID] FOREIGN KEY ([SaleOrderTypeID]) REFERENCES [dbo].[SaleOrderType] ([ID])
);



