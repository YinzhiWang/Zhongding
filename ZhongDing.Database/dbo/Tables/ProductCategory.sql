CREATE TABLE [dbo].[ProductCategory] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [CategoryName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED ([ID] ASC)
);

