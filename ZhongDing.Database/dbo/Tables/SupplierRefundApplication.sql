CREATE TABLE [dbo].[SupplierRefundApplication] (
    [ID]                     INT      IDENTITY (1, 1) NOT NULL,
    [CompanyID]              INT      NOT NULL,
    [SupplierID]             INT      NOT NULL,
    [ProductID]              INT      NOT NULL,
    [ProductSpecificationID] INT      NOT NULL,
    [IsDeleted]              BIT      CONSTRAINT [DF_SupplierRefundApplication_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]              DATETIME CONSTRAINT [DF_SupplierRefundApplication_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT      NULL,
    [LastModifiedOn]         DATETIME NULL,
    [LastModifiedBy]         INT      NULL,
    CONSTRAINT [PK_SupplierRefundApplication] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierRefundApplication_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_SupplierRefundApplication_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_SupplierRefundApplication_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID]),
    CONSTRAINT [FK_SupplierRefundApplication_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID])
);

