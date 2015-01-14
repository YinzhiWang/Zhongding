CREATE TABLE [dbo].[InventoryHistory] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [WarehouseID]            INT            NOT NULL,
    [ProductID]              INT            NOT NULL,
    [ProductSpecificationID] INT            NOT NULL,
    [LicenseNumber]          NVARCHAR (255) NULL,
    [BatchNumber]            NVARCHAR (255) NULL,
    [ExpirationDate]         DATETIME       NULL,
    [ProcurePrice]           MONEY          NOT NULL,
    [InQty]                  INT            NOT NULL,
    [OutQty]                 INT            NOT NULL,
    [BalanceQty]             INT            NOT NULL,
    [StatDate]               DATETIME       NOT NULL,
    [CreatedOn]              DATETIME       CONSTRAINT [DF_InventoryHistory_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_InventoryHistory] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_InventoryHistory_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_InventoryHistory_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID]),
    CONSTRAINT [FK_InventoryHistory_Warehouse] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([ID])
);



