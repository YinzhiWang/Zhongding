CREATE TABLE [dbo].[SalesOrderAppDetail] (
    [ID]                      INT      IDENTITY (1, 1) NOT NULL,
    [SalesOrderApplicationID] INT      NOT NULL,
    [ProductID]               INT      NOT NULL,
    [ProductSpecificationID]  INT      NOT NULL,
    [Count]                   INT      NOT NULL,
    [SalesPrice]              MONEY    NOT NULL,
    [InvoicePrice]            MONEY    NULL,
    [TotalSalesAmount]        MONEY    NOT NULL,
    [GiftCount]               INT      NULL,
    [IsDeleted]               BIT      CONSTRAINT [DF_SalesOrderAppDetail_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]               DATETIME CONSTRAINT [DF_SalesOrderAppDetail_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]               INT      NULL,
    [LastModifiedOn]          DATETIME NULL,
    [LastModifiedBy]          INT      NULL,
    [WarehouseID]             INT      NULL,
    CONSTRAINT [PK_SalesOrderAppDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SalesOrderAppDetail_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_SalesOrderAppDetail_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID]),
    CONSTRAINT [FK_SalesOrderAppDetail_SalesOrderApplication] FOREIGN KEY ([SalesOrderApplicationID]) REFERENCES [dbo].[SalesOrderApplication] ([ID]),
    CONSTRAINT [FK_SalesOrderAppDetail_Warehouse] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([ID])
);











