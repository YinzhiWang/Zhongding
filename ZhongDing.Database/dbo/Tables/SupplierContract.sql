CREATE TABLE [dbo].[SupplierContract] (
    [ID]                     INT             IDENTITY (1, 1) NOT NULL,
    [SupplierID]             INT             NULL,
    [ProductID]              INT             NULL,
    [ProductSpecificationID] INT             NULL,
    [UnitPrice]              DECIMAL (18, 8) NULL,
    [IsNeedTaskAssignment]   BIT             NULL,
    [ExpirationDate]         DATETIME        NULL,
    [IsDeleted]              BIT             CONSTRAINT [DF_SupplierContract_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedOn]              DATETIME        NULL,
    [CreatedOn]              DATETIME        CONSTRAINT [DF_SupplierContract_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT             NULL,
    [LastModifiedOn]         DATETIME        NULL,
    [LastModifiedBy]         INT             NULL,
    CONSTRAINT [PK_SupplierContract] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierContract_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_SupplierContract_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID]),
    CONSTRAINT [FK_SupplierContract_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID])
);

