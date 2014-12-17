CREATE TABLE [dbo].[ClientSaleApplication] (
    [ID]                      INT      NOT NULL,
    [SalesOrderApplicationID] INT      NOT NULL,
    [SalesModelID]            INT      NULL,
    [ClientUserID]            INT      NULL,
    [ClientCompanyID]         INT      NULL,
    [DeliveryModeID]          INT      NULL,
    [ReceivingBankAccountID]  INT      NULL,
    [ClientContactID]         INT      NULL,
    [GuaranteeAmount]         MONEY    NULL,
    [Guaranteeby]             INT      NULL,
    [IsDeleted]               BIT      NULL,
    [CreatedOn]               DATETIME NULL,
    [CreatedBy]               INT      NULL,
    [LastModifiedOn]          DATETIME NULL,
    [LastModifiedBy]          INT      NULL,
    CONSTRAINT [PK_ClientSaleApplication] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientSaleApplication_BankAccount] FOREIGN KEY ([ReceivingBankAccountID]) REFERENCES [dbo].[BankAccount] ([ID]),
    CONSTRAINT [FK_ClientSaleApplication_ClientCompany] FOREIGN KEY ([ClientCompanyID]) REFERENCES [dbo].[ClientCompany] ([ID]),
    CONSTRAINT [FK_ClientSaleApplication_ClientUser] FOREIGN KEY ([ClientUserID]) REFERENCES [dbo].[ClientUser] ([ID]),
    CONSTRAINT [FK_ClientSaleApplication_SalesOrderApplication] FOREIGN KEY ([SalesOrderApplicationID]) REFERENCES [dbo].[SalesOrderApplication] ([ID])
);

