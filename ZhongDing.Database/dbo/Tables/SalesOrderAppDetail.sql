CREATE TABLE [dbo].[SalesOrderAppDetail] (
    [ID]                      INT      IDENTITY (1, 1) NOT NULL,
    [SalesOrderApplicationID] INT      NULL,
    [WarehouseID]             INT      NULL,
    [ProductID]               INT      NULL,
    [ProductSpecificationID]  INT      NULL,
    [Count]                   INT      NULL,
    [SalesPrice]              MONEY    NULL,
    [TotalSalesAmount]        MONEY    NULL,
    [GiftCount]               INT      NULL,
    [IsDeleted]               BIT      NULL,
    [CreatedOn]               DATETIME NULL,
    [CreatedBy]               INT      NULL,
    [LastModfiiedOn]          DATETIME NULL,
    [LastModifiedBy]          INT      NULL,
    CONSTRAINT [PK_SalesOrderAppDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SalesOrderAppDetail_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_SalesOrderAppDetail_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID]),
    CONSTRAINT [FK_SalesOrderAppDetail_SalesOrderApplication] FOREIGN KEY ([SalesOrderApplicationID]) REFERENCES [dbo].[SalesOrderApplication] ([ID]),
    CONSTRAINT [FK_SalesOrderAppDetail_Warehouse] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([ID])
);

