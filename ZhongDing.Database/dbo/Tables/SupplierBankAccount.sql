CREATE TABLE [dbo].[SupplierBankAccount] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [SupplierID]     INT      NULL,
    [BankAccountID]  INT      NULL,
    [IsDeleted]      BIT      CONSTRAINT [DF_SupplierBankAccount_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedOn]      DATETIME NULL,
    [CreatedOn]      DATETIME CONSTRAINT [DF_SupplierBankAccount_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    CONSTRAINT [PK_SupplierBankAccount] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierBankAccount_BankAccount] FOREIGN KEY ([BankAccountID]) REFERENCES [dbo].[BankAccount] ([ID]),
    CONSTRAINT [FK_SupplierBankAccount_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID])
);

