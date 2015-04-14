CREATE TABLE [dbo].[ClientUser] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [ClientName]      NVARCHAR (50) NOT NULL,
    [IsDeleted]       BIT           CONSTRAINT [DF_ClientUser_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]       DATETIME      CONSTRAINT [DF_ClientUser_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       INT           NULL,
    [LastModifiedOn]  DATETIME      NULL,
    [LastModifiedBy]  INT           NULL,
    [DBBankAccountID] INT           NULL,
    CONSTRAINT [PK_ClientUser] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientUser_BankAccount] FOREIGN KEY ([DBBankAccountID]) REFERENCES [dbo].[BankAccount] ([ID])
);









