CREATE TABLE [dbo].[UserBonus] (
    [ID]                  INT             IDENTITY (1, 1) NOT NULL,
    [UserID]              INT             NULL,
    [ProductID]           INT             NULL,
    [UnitOfMeasurementID] INT             NULL,
    [UnitPrice]           MONEY           NULL,
    [Quantity]            INT             NULL,
    [SalesAmount]         MONEY           NULL,
    [SalesRatio]          DECIMAL (18, 8) NULL,
    [BonusPay]            MONEY           NULL,
    [IsDeleted]           BIT             CONSTRAINT [DF_UserBonus_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedOn]           DATETIME        NULL,
    [CreatedOn]           DATETIME        CONSTRAINT [DF_UserBonus_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]           INT             NULL,
    [LastModifiedOn]      DATETIME        NULL,
    [LastModifiedBy]      INT             NULL,
    CONSTRAINT [PK_UserBonus] PRIMARY KEY CLUSTERED ([ID] ASC)
);

