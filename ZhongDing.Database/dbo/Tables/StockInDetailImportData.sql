CREATE TABLE [dbo].[StockInDetailImportData] (
    [ID]                      INT            IDENTITY (1, 1) NOT NULL,
    [StockInImportDataID]     INT            NOT NULL,
    [ProcureOrderAppID]       INT            NOT NULL,
    [ProcureOrderAppDetailID] INT            NOT NULL,
    [ProductName]             NVARCHAR (255) NULL,
    [ProductSpecification]    NVARCHAR (255) NULL,
    [WarehouseName]           NVARCHAR (255) NULL,
    [ProcurePrice]            MONEY          NOT NULL,
    [InQty]                   INT            NOT NULL,
    [BatchNumber]             NVARCHAR (255) NULL,
    [ExpirationDate]          DATETIME       NULL,
    [LicenseNumber]           NVARCHAR (255) NULL,
    [IsMortgagedProduct]      BIT            NULL,
    [CreatedOn]               DATETIME       NOT NULL,
    [CreatedBy]               INT            NULL,
    [LastModifiedOn]          DATETIME       NULL,
    [LastModifiedBy]          INT            NULL,
    [UnitOfMeasurement]       NVARCHAR (255) NULL,
    CONSTRAINT [PK_StockInDetailImportData] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_StockInDetailImportData_StockInImportDataID] FOREIGN KEY ([StockInImportDataID]) REFERENCES [dbo].[StockInImportData] ([ID])
);

