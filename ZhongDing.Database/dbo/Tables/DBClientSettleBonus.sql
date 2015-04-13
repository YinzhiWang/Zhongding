CREATE TABLE [dbo].[DBClientSettleBonus] (
    [ID]                   INT            IDENTITY (1, 1) NOT NULL,
    [DBClientSettlementID] INT            NOT NULL,
    [DBClientBonusID]      INT            NOT NULL,
    [TotalPayAmount]       MONEY          NULL,
    [IsNeedSettlement]     BIT            NULL,
    [IsManualSettled]      BIT            NULL,
    [ManualSettledBy]      INT            NULL,
    [Comment]              NVARCHAR (MAX) NULL,
    [IsDeleted]            BIT            CONSTRAINT [DF_DBClientSettleBonus_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]            DATETIME       CONSTRAINT [DF_DBClientSettleBonus_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]            INT            NULL,
    [LastModifiedOn]       DATETIME       NULL,
    [LastModifiedBy]       INT            NULL,
    CONSTRAINT [PK_DBClientSettleBonus] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DBClientSettleBonus_DBClientBonus] FOREIGN KEY ([DBClientBonusID]) REFERENCES [dbo].[DBClientBonus] ([ID]),
    CONSTRAINT [FK_DBClientSettleBonus_DBClientSettlement] FOREIGN KEY ([DBClientSettlementID]) REFERENCES [dbo].[DBClientSettlement] ([ID])
);



