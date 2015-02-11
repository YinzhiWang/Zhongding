CREATE TABLE [dbo].[ClientRefundAppDetail] (
    [ID]                     INT             IDENTITY (1, 1) NOT NULL,
    [ClientRefundAppID]      INT             NOT NULL,
    [WarehouseID]            INT             NOT NULL,
    [ProductID]              INT             NOT NULL,
    [ProductSpecificationID] INT             NOT NULL,
    [Count]                  INT             NOT NULL,
    [HighPrice]              MONEY           NOT NULL,
    [ActualSalePrice]        MONEY           NOT NULL,
    [TotalSalesAmount]       MONEY           NOT NULL,
    [ClientTaxRatio]         DECIMAL (18, 8) NOT NULL,
    [RefundAmount]           MONEY           NOT NULL,
    [IsDeleted]              BIT             CONSTRAINT [DF_ClientRefundAppDetail_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]              DATETIME        CONSTRAINT [DF_ClientRefundAppDetail_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT             NULL,
    [LastModifiedOn]         DATETIME        NULL,
    [LastModifiedBy]         INT             NULL,
    CONSTRAINT [PK_ClientRefundAppDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientRefundAppDetail_ClientRefundApplication] FOREIGN KEY ([ClientRefundAppID]) REFERENCES [dbo].[ClientRefundApplication] ([ID]),
    CONSTRAINT [FK_ClientRefundAppDetail_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_ClientRefundAppDetail_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID]),
    CONSTRAINT [FK_ClientRefundAppDetail_Warehouse] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([ID])
);

