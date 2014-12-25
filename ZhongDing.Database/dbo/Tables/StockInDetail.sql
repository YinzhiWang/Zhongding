CREATE TABLE [dbo].[StockInDetail] (
    [ID]                      INT            IDENTITY (1, 1) NOT NULL,
    [StockInID]               INT            NOT NULL,
    [ProcureOrderAppID]       INT            NOT NULL,
    [ProcureOrderAppDetailID] INT            NOT NULL,
    [ProductID]               INT            NOT NULL,
    [ProductSpecificationID]  INT            NOT NULL,
    [WarehouseID]             INT            NOT NULL,
    [ProcurePrice]            MONEY          NOT NULL,
    [InQty]                   INT            NOT NULL,
    [BatchNumber]             NVARCHAR (255) NULL,
    [ExpirationDate]          DATETIME       NULL,
    [LicenseNumber]           NVARCHAR (255) NULL,
    [IsMortgagedProduct]      BIT            NULL,
    [IsDeleted]               BIT            CONSTRAINT [DF_StockInDetail_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]               DATETIME       CONSTRAINT [DF_StockInDetail_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]               INT            NULL,
    [LastModifiedOn]          DATETIME       NULL,
    [LastModifiedBy]          INT            NULL,
    CONSTRAINT [PK_StockInDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_StockInDetail_ProcureOrderAppDetail] FOREIGN KEY ([ProcureOrderAppDetailID]) REFERENCES [dbo].[ProcureOrderAppDetail] ([ID]),
    CONSTRAINT [FK_StockInDetail_ProcureOrderApplication] FOREIGN KEY ([ProcureOrderAppID]) REFERENCES [dbo].[ProcureOrderApplication] ([ID]),
    CONSTRAINT [FK_StockInDetail_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_StockInDetail_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID]),
    CONSTRAINT [FK_StockInDetail_StockIn] FOREIGN KEY ([StockInID]) REFERENCES [dbo].[StockIn] ([ID]),
    CONSTRAINT [FK_StockInDetail_Warehouse] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([ID])
);



