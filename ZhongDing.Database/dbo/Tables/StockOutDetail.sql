CREATE TABLE [dbo].[StockOutDetail] (
    [ID]                      INT            IDENTITY (1, 1) NOT NULL,
    [StockOutID]              INT            NOT NULL,
    [SalesOrderApplicationID] INT            NOT NULL,
    [SalesOrderAppDetailID]   INT            NOT NULL,
    [ProductID]               INT            NOT NULL,
    [ProductSpecificationID]  INT            NOT NULL,
    [SalesPrice]              MONEY          NOT NULL,
    [OutQty]                  INT            NOT NULL,
    [TotalSalesAmount]        MONEY          NOT NULL,
    [WarehouseID]             INT            NOT NULL,
    [BatchNumber]             NVARCHAR (255) NULL,
    [ExpirationDate]          DATETIME       NULL,
    [LicenseNumber]           NVARCHAR (255) NULL,
    [TaxQty]                  INT            NULL,
    [IsDeleted]               BIT            CONSTRAINT [DF_StockOutDetail_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]               DATETIME       CONSTRAINT [DF_StockOutDetail_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]               INT            NULL,
    [LastModifiedOn]          DATETIME       NULL,
    [LastModifiedBy]          INT            NULL,
    CONSTRAINT [PK_StockOutDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_StockOutDetail_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_StockOutDetail_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID]),
    CONSTRAINT [FK_StockOutDetail_SalesOrderAppDetail] FOREIGN KEY ([SalesOrderAppDetailID]) REFERENCES [dbo].[SalesOrderAppDetail] ([ID]),
    CONSTRAINT [FK_StockOutDetail_SalesOrderApplication] FOREIGN KEY ([SalesOrderApplicationID]) REFERENCES [dbo].[SalesOrderApplication] ([ID]),
    CONSTRAINT [FK_StockOutDetail_StockOut] FOREIGN KEY ([StockOutID]) REFERENCES [dbo].[StockOut] ([ID]),
    CONSTRAINT [FK_StockOutDetail_Warehouse] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([ID])
);





