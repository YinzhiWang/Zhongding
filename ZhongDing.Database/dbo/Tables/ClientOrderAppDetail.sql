CREATE TABLE [dbo].[ClientOrderAppDetail] (
    [ID]                       INT      IDENTITY (1, 1) NOT NULL,
    [ClientOrderApplicationID] INT      NULL,
    [WarehouseID]              INT      NULL,
    [ProductID]                INT      NULL,
    [ProductSpecificationID]   INT      NULL,
    [ProcureCount]             INT      NULL,
    [ProcurePrice]             MONEY    NULL,
    [TotalAmount]              MONEY    NULL,
    [TaxAmount]                MONEY    NULL,
    [IsDeleted]                BIT      NULL,
    [CreatedOn]                DATETIME NULL,
    [CreatedBy]                INT      NULL,
    [LastModifiedOn]           DATETIME NULL,
    [LastModifiedBy]           INT      NULL,
    CONSTRAINT [PK_ClientOrderAppDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientOrderAppDetail_ClientOrderApplication] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([ID]),
    CONSTRAINT [FK_ClientOrderAppDetail_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_ClientOrderAppDetail_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID])
);

