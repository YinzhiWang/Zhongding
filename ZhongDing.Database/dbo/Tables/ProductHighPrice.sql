CREATE TABLE [dbo].[ProductHighPrice] (
    [ID]                     INT             IDENTITY (1, 1) NOT NULL,
    [ProductID]              INT             NOT NULL,
    [ProductSpecificationID] INT             NOT NULL,
    [HighPrice]              MONEY           NULL,
    [ActualProcurePrice]     MONEY           NULL,
    [ActualSalePrice]        MONEY           NULL,
    [SupplierTaxRatio]       DECIMAL (18, 8) NULL,
    [ClientTaxRatio]         DECIMAL (18, 8) NULL,
    [Comment]                NVARCHAR (255)  NULL,
    [IsDeleted]              BIT             CONSTRAINT [DF_ProductHighPrice_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]              DATETIME        CONSTRAINT [DF_ProductHighPrice_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT             NULL,
    [LastModifiedOn]         DATETIME        NULL,
    [LastModifiedBy]         INT             NULL,
    CONSTRAINT [PK_ProductHighPrice] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProductHighPrice_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_ProductHighPrice_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID])
);







