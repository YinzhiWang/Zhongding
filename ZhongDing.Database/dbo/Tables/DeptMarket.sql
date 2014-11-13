CREATE TABLE [dbo].[DeptMarket] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [DeptDistrictID] INT           NOT NULL,
    [MarketName]     NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_DeptMarket] PRIMARY KEY CLUSTERED ([ID] ASC)
);

