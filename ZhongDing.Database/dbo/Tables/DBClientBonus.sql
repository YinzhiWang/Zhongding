CREATE TABLE [dbo].[DBClientBonus] (
    [ID]                     INT      IDENTITY (1, 1) NOT NULL,
    [DBContractID]           INT      NOT NULL,
    [ClientUserID]           INT      NOT NULL,
    [SettlementDate]         DATETIME NOT NULL,
    [ProductID]              INT      NOT NULL,
    [ProductSpecificationID] INT      NOT NULL,
    [PromotionExpense]       MONEY    NULL,
    [SaleQty]                INT      NOT NULL,
    [BonusAmount]            MONEY    NULL,
    [PerformanceAmount]      MONEY    NULL,
    [IsSettled]              BIT      NULL,
    [SettledDate]            DATETIME NULL,
    [IsDeleted]              BIT      CONSTRAINT [DF_DBClientBonus_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]              DATETIME CONSTRAINT [DF_DBClientBonus_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT      NULL,
    [LastModifiedOn]         DATETIME NULL,
    [LastModifiedBy]         INT      NULL,
    CONSTRAINT [PK_DBClientBonus] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DBClientBonus_ClientUser] FOREIGN KEY ([ClientUserID]) REFERENCES [dbo].[ClientUser] ([ID]),
    CONSTRAINT [FK_DBClientBonus_DBContract] FOREIGN KEY ([DBContractID]) REFERENCES [dbo].[DBContract] ([ID]),
    CONSTRAINT [FK_DBClientBonus_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_DBClientBonus_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID])
);





