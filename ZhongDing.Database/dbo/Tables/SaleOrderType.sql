CREATE TABLE [dbo].[SaleOrderType] (
    [ID]       INT           IDENTITY (1, 1) NOT NULL,
    [TypeName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_SaleOrderType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

