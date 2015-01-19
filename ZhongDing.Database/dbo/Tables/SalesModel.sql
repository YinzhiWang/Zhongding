CREATE TABLE [dbo].[SalesModel] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [SalesModelName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_SalesModel] PRIMARY KEY CLUSTERED ([ID] ASC)
);

