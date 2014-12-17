CREATE TABLE [dbo].[ProcureOrderAppDetail] (
    [ID]                        INT      IDENTITY (1, 1) NOT NULL,
    [ProcureOrderApplicationID] INT      NOT NULL,
    [WarehouseID]               INT      NOT NULL,
    [ProductID]                 INT      NOT NULL,
    [ProductSpecificationID]    INT      NOT NULL,
    [ProcureCount]              INT      NOT NULL,
    [ProcurePrice]              MONEY    NOT NULL,
    [TotalAmount]               MONEY    NOT NULL,
    [TaxAmount]                 MONEY    NULL,
    [IsDeleted]                 BIT      CONSTRAINT [DF_ProcureOrderAppDetail_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]                 DATETIME CONSTRAINT [DF_ProcureOrderAppDetail_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                 INT      NULL,
    [LastModifiedOn]            DATETIME NULL,
    [LastModifiedBy]            INT      NULL,
    CONSTRAINT [PK_ClientOrderAppDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProcureOrderAppDetail_ProcureOrderApplication] FOREIGN KEY ([ProcureOrderApplicationID]) REFERENCES [dbo].[ProcureOrderApplication] ([ID]),
    CONSTRAINT [FK_ProcureOrderAppDetail_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_ProcureOrderAppDetail_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID]),
    CONSTRAINT [FK_ProcureOrderAppDetail_Warehouse] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([ID])
);



