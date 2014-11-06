CREATE TABLE [dbo].[ProductBasicPrice] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [ProductID]              INT            NULL,
    [ProductSpecificationID] INT            NULL,
    [ProcurePrice]           FLOAT (53)     NULL,
    [SalePrice]              FLOAT (53)     NULL,
    [Comment]                NVARCHAR (255) NULL,
    [IsDeleted]              BIT            NULL,
    [CreatedOn]              DATETIME       NULL,
    [CreatedBy]              INT            NULL,
    [LastModifiedOn]         DATETIME       NULL,
    [LastModifiedBy]         INT            NULL,
    CONSTRAINT [PK_ProductBasicPrice] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProductBasicPrice_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_ProductBasicPrice_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID])
);

