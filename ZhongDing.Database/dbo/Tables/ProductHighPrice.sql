CREATE TABLE [dbo].[ProductHighPrice] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [ProductID]              INT            NOT NULL,
    [ProductSpecificationID] INT            NOT NULL,
    [HighPrice]              FLOAT (53)     NULL,
    [ActualProcurePrice]     FLOAT (53)     NULL,
    [ActualSalePrice]        FLOAT (53)     NULL,
    [SupplierTaxRatio]       FLOAT (53)     NULL,
    [ClientTaxRatio]         FLOAT (53)     NULL,
    [Comment]                NVARCHAR (255) NULL,
    [IsDeleted]              BIT            NULL,
    [CreatedOn]              DATETIME       NULL,
    [CreatedBy]              INT            NULL,
    [LastModifiedOn]         DATETIME       NULL,
    [LastModifiedBy]         INT            NULL,
    CONSTRAINT [PK_ProductHighPrice] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProductHighPrice_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_ProductHighPrice_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID])
);

