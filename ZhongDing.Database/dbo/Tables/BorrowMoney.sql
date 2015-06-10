CREATE TABLE [dbo].[BorrowMoney] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [BorrowDate]     DATETIME       NOT NULL,
    [BorrowName]     NVARCHAR (100) NOT NULL,
    [BorrowAmount]   MONEY          NOT NULL,
    [ReturnDate]     DATETIME       NOT NULL,
    [Comment]        NVARCHAR (100) NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF__BorrowMon__IsDel__0169FCB3] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME       CONSTRAINT [DF__BorrowMon__Creat__025E20EC] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    [Status]         INT            CONSTRAINT [DF_BorrowMoney_Status] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__BorrowMo__3214EC27B14C6623] PRIMARY KEY CLUSTERED ([ID] ASC)
);

