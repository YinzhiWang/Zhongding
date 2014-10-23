CREATE TABLE [dbo].[Product] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [CategoryID]             INT            NULL,
    [ProductCode]            NVARCHAR (50)  NULL,
    [ProductName]            NVARCHAR (255) NULL,
    [IsManagedByBatchNumber] BIT            NULL,
    [SupplierID]             INT            NULL,
    [CompanyID]              INT            NULL,
    [DepartmentID]           INT            NULL,
    [SafetyStock]            INT            NULL,
    [IsDeleted]              BIT            CONSTRAINT [DF_Product_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedOn]              DATETIME       NULL,
    [CreatedOn]              DATETIME       CONSTRAINT [DF_Product_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT            NULL,
    [LastModifiedOn]         DATETIME       NULL,
    [LastModifiedBy]         INT            NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Product_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_Product_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([ID]),
    CONSTRAINT [FK_Product_ProductCategory] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[ProductCategory] ([ID]),
    CONSTRAINT [FK_Product_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID])
);

