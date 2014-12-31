CREATE TABLE [dbo].[DaBaoRequestAppDetail] (
    [ID]                        INT      IDENTITY (1, 1) NOT NULL,
    [DaBaoRequestApplicationID] INT      NOT NULL,
    [ProductID]                 INT      NOT NULL,
    [ProductSpecificationID]    INT      NOT NULL,
    [Count]                     INT      NOT NULL,
    [SalesPrice]                MONEY    NOT NULL,
    [TotalSalesAmount]          MONEY    NOT NULL,
    [GiftCount]                 INT      NULL,
    [IsDeleted]                 BIT      CONSTRAINT [DF_DaBaoRequestAppDetail_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]                 DATETIME CONSTRAINT [DF_DaBaoRequestAppDetail_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                 INT      NULL,
    [LastModifiedOn]            DATETIME NULL,
    [LastModifiedBy]            INT      NULL,
    CONSTRAINT [PK_DaBaoRequestAppDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DaBaoRequestAppDetail_DaBaoRequestApplication] FOREIGN KEY ([DaBaoRequestApplicationID]) REFERENCES [dbo].[DaBaoRequestApplication] ([ID]),
    CONSTRAINT [FK_DaBaoRequestAppDetail_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_DaBaoRequestAppDetail_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID])
);





