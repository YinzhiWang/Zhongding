CREATE TABLE [dbo].[ClientSaleAppBankAccount] (
    [ID]                      INT      IDENTITY (1, 1) NOT NULL,
    [ClientSaleApplicationID] INT      NOT NULL,
    [ReceiverBankAccountID]   INT      NOT NULL,
    [IsDeleted]               BIT      CONSTRAINT [DF_ClientSaleAppBankAccount_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]               DATETIME CONSTRAINT [DF_ClientSaleAppBankAccount_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]               INT      NULL,
    [LastModifiedOn]          DATETIME NULL,
    [LastModifiedBy]          INT      NULL,
    CONSTRAINT [PK_ClientSaleAppBankAccount] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientSaleAppBankAccount_BankAccount] FOREIGN KEY ([ReceiverBankAccountID]) REFERENCES [dbo].[BankAccount] ([ID]),
    CONSTRAINT [FK_ClientSaleAppBankAccount_ClientSaleApplication] FOREIGN KEY ([ClientSaleApplicationID]) REFERENCES [dbo].[ClientSaleApplication] ([ID])
);

