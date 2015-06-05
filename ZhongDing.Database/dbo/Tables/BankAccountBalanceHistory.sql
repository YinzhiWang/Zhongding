CREATE TABLE [dbo].[BankAccountBalanceHistory] (
    [ID]            INT      IDENTITY (1, 1) NOT NULL,
    [BankAccountID] INT      NOT NULL,
    [BalanceDate]   DATETIME NOT NULL,
    [Balance]       MONEY    NOT NULL,
    [IsDeleted]     BIT      NOT NULL,
    [CreatedOn]     DATETIME DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     INT      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BankAccountBalanceHistory_BankAccountID] FOREIGN KEY ([BankAccountID]) REFERENCES [dbo].[BankAccount] ([ID])
);

