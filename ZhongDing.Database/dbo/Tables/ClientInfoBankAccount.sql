CREATE TABLE [dbo].[ClientInfoBankAccount] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [ClientInfoID]   INT      NULL,
    [BankAccountID]  INT      NULL,
    [IsDeleted]      BIT      CONSTRAINT [DF_ClientInfoBankAccount_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME CONSTRAINT [DF_ClientInfoBankAccount_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    CONSTRAINT [PK_ClientInfoBankAccount] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientInfoBankAccount_BankAccount] FOREIGN KEY ([BankAccountID]) REFERENCES [dbo].[BankAccount] ([ID]),
    CONSTRAINT [FK_ClientInfoBankAccount_ClientInfo] FOREIGN KEY ([ClientInfoID]) REFERENCES [dbo].[ClientInfo] ([ID])
);



