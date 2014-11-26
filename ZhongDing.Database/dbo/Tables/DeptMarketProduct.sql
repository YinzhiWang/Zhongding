CREATE TABLE [dbo].[DeptMarketProduct] (
    [ID]               INT      IDENTITY (1, 1) NOT NULL,
    [MarketDivisionID] INT      NOT NULL,
    [ProductID]        INT      NOT NULL,
    [Q1Task]           INT      NULL,
    [Q2Task]           INT      NULL,
    [Q3Task]           INT      NULL,
    [Q4Task]           INT      NULL,
    [IsDeleted]        BIT      CONSTRAINT [DF_DeptMarketProduct_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]        DATETIME CONSTRAINT [DF_DeptMarketProduct_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT      NULL,
    [LastModifiedOn]   DATETIME NULL,
    [LastModifiedBy]   INT      NULL,
    CONSTRAINT [PK_DeptMarketProduct] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DeptMarketProduct_DeptMarketDivision] FOREIGN KEY ([MarketDivisionID]) REFERENCES [dbo].[DeptMarketDivision] ([ID]),
    CONSTRAINT [FK_DeptMarketProduct_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID])
);





