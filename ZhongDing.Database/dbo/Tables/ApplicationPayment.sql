CREATE TABLE [dbo].[ApplicationPayment] (
    [ID]                INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationID]     INT            NOT NULL,
    [WorkflowID]        INT            NOT NULL,
    [FromBankAccountID] INT            NOT NULL,
    [FromAccount]       NVARCHAR (50)  NULL,
    [ToBankAccountID]   INT            NOT NULL,
    [ToAccount]         NVARCHAR (50)  NULL,
    [Amount]            MONEY          NULL,
    [fee]               MONEY          NULL,
    [PaymentTypeID]     INT            NOT NULL,
    [Comment]           NVARCHAR (500) NULL,
    [PaymentStatusID]   INT            NOT NULL,
    [IsDeleted]         BIT            CONSTRAINT [DF_ApplicationPayment_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]         DATETIME       CONSTRAINT [DF_ApplicationPayment_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]         INT            NULL,
    [LastModifiedOn]    DATETIME       NULL,
    [LastModifiedBy]    INT            NULL,
    CONSTRAINT [PK_ApplicationPayment] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ApplicationPayment_BankAccount] FOREIGN KEY ([ToBankAccountID]) REFERENCES [dbo].[BankAccount] ([ID]),
    CONSTRAINT [FK_ApplicationPayment_BankAccount1] FOREIGN KEY ([ToBankAccountID]) REFERENCES [dbo].[BankAccount] ([ID]),
    CONSTRAINT [FK_ApplicationPayment_PaymentStatus] FOREIGN KEY ([PaymentStatusID]) REFERENCES [dbo].[PaymentStatus] ([ID]),
    CONSTRAINT [FK_ApplicationPayment_PaymentType] FOREIGN KEY ([PaymentTypeID]) REFERENCES [dbo].[PaymentType] ([ID]),
    CONSTRAINT [FK_ApplicationPayment_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
);





