CREATE TABLE [dbo].[DBClientInvoice] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [CompanyID]             INT            NOT NULL,
    [DistributionCompanyID] INT            NOT NULL,
    [InvoiceDate]           DATETIME       NOT NULL,
    [InvoiceNumber]         NVARCHAR (256) NOT NULL,
    [Amount]                MONEY          NOT NULL,
    [TransportNumber]       NVARCHAR (256) NOT NULL,
    [TransportCompany]      NVARCHAR (256) NOT NULL,
    [SaleOrderTypeID]       INT            NOT NULL,
    [IsSettled]             BIT            NULL,
    [SettledDate]           DATETIME       NULL,
    [IsDeleted]             BIT            NOT NULL,
    [CreatedOn]             DATETIME       NOT NULL,
    [CreatedBy]             INT            NULL,
    [LastModifiedOn]        DATETIME       NULL,
    [LastModifiedBy]        INT            NULL,
    CONSTRAINT [PK__DBClientInvoice__ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DBClientInvoice_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_DBClientInvoice_DistributionCompanyID] FOREIGN KEY ([DistributionCompanyID]) REFERENCES [dbo].[DistributionCompany] ([ID]),
    CONSTRAINT [FK_DBClientInvoice_SaleOrderTypeID] FOREIGN KEY ([SaleOrderTypeID]) REFERENCES [dbo].[SaleOrderType] ([ID])
);





