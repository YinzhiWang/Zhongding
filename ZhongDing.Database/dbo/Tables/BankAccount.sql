CREATE TABLE [dbo].[BankAccount] (
    [ID]             INT             IDENTITY (1, 1) NOT NULL,
    [CompanyID]      INT             NULL,
    [AccountName]    NVARCHAR (100)  NULL,
    [BankBranchName] NVARCHAR (255)  NULL,
    [Account]        NVARCHAR (50)   NULL,
    [AccountTypeID]  INT             NULL,
    [OwnerTypeID]    INT             NULL,
    [Comment]        NVARCHAR (1000) NULL,
    [IsDeleted]      BIT             CONSTRAINT [DF_BankAccount_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME        CONSTRAINT [DF_BankAccount_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT             NULL,
    [LastModifiedOn] DATETIME        NULL,
    [LastModifiedBy] INT             NULL,
    CONSTRAINT [PK_BankAccount] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BankAccount_AccountType] FOREIGN KEY ([AccountTypeID]) REFERENCES [dbo].[AccountType] ([ID]),
    CONSTRAINT [FK_BankAccount_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_BankAccount_OwnerType] FOREIGN KEY ([OwnerTypeID]) REFERENCES [dbo].[OwnerType] ([ID])
);





