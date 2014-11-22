CREATE TABLE [dbo].[ProductBasicPrice] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [ProductID]              INT            NOT NULL,
    [ProductSpecificationID] INT            NOT NULL,
    [ProcurePrice]           MONEY          NULL,
    [SalePrice]              MONEY          NULL,
    [Comment]                NVARCHAR (255) NULL,
    [IsDeleted]              BIT            CONSTRAINT [DF_ProductBasicPrice_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]              DATETIME       CONSTRAINT [DF_ProductBasicPrice_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT            NULL,
    [LastModifiedOn]         DATETIME       NULL,
    [LastModifiedBy]         INT            NULL,
    CONSTRAINT [PK_ProductBasicPrice] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProductBasicPrice_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_ProductBasicPrice_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID])
);







