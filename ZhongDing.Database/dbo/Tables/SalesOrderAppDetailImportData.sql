CREATE TABLE [dbo].[SalesOrderAppDetailImportData] (
    [ID]                                INT            IDENTITY (1, 1) NOT NULL,
    [SalesOrderApplicationImportDataID] INT            NOT NULL,
    [SalesOrderAppDetailID]             INT            NOT NULL,
    [ProductName]                       NVARCHAR (255) NULL,
    [ProductSpecification]              NVARCHAR (255) NULL,
    [WarehouseName]                     NVARCHAR (255) NULL,
    [UnitOfMeasurement]                 NVARCHAR (255) NULL,
    [Count]                             INT            NOT NULL,
    [SalesPrice]                        MONEY          NOT NULL,
    [InvoicePrice]                      MONEY          NULL,
    [TotalSalesAmount]                  MONEY          NOT NULL,
    [GiftCount]                         INT            NULL,
    [IsDeleted]                         BIT            NOT NULL,
    [CreatedOn]                         DATETIME       NOT NULL,
    [CreatedBy]                         INT            NULL,
    [LastModifiedOn]                    DATETIME       NULL,
    [LastModifiedBy]                    INT            NULL,
    CONSTRAINT [PK_SalesOrderAppDetailImportData] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SalesOrderAppDetailImportData_SalesOrderAppDetailID] FOREIGN KEY ([SalesOrderAppDetailID]) REFERENCES [dbo].[SalesOrderAppDetail] ([ID]),
    CONSTRAINT [FK_SalesOrderAppDetailImportData_SalesOrderApplicationImportDataID] FOREIGN KEY ([SalesOrderApplicationImportDataID]) REFERENCES [dbo].[SalesOrderApplicationImportData] ([ID])
);

