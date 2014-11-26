CREATE TABLE [dbo].[DeptMarketDivision] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [UserID]         INT           NOT NULL,
    [MarketID]       VARCHAR (500) NULL,
    [IsDeleted]      BIT           CONSTRAINT [DF_DeptMarketDivision_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME      CONSTRAINT [DF_DeptMarketDivision_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT           NULL,
    [LastModifiedOn] DATETIME      NULL,
    [LastModifiedBy] INT           NULL,
    CONSTRAINT [PK_DeptMarketDivision] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DeptMarketDivision_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);



