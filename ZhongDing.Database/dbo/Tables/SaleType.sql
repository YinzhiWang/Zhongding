CREATE TABLE [dbo].[SaleType] (
    [ID]       INT           IDENTITY (1, 1) NOT NULL,
    [SaleType] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_SaleType] PRIMARY KEY CLUSTERED ([ID] ASC)
);



