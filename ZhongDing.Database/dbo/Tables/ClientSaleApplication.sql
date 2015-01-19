CREATE TABLE [dbo].[ClientSaleApplication] (
    [ID]                      INT      NOT NULL,
    [SalesOrderApplicationID] INT      NOT NULL,
    [SalesModelID]            INT      NOT NULL,
    [ClientUserID]            INT      NOT NULL,
    [ClientCompanyID]         INT      NOT NULL,
    [DeliveryModeID]          INT      NULL,
    [ReceivingBankAccountID]  INT      NULL,
    [ClientContactID]         INT      NOT NULL,
    [GuaranteeAmount]         MONEY    NULL,
    [Guaranteeby]             INT      NULL,
    [IsDeleted]               BIT      CONSTRAINT [DF_ClientSaleApplication_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]               DATETIME CONSTRAINT [DF_ClientSaleApplication_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]               INT      NULL,
    [LastModifiedOn]          DATETIME NULL,
    [LastModifiedBy]          INT      NULL,
    CONSTRAINT [PK_ClientSaleApplication] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientSaleApplication_BankAccount] FOREIGN KEY ([ReceivingBankAccountID]) REFERENCES [dbo].[BankAccount] ([ID]),
    CONSTRAINT [FK_ClientSaleApplication_ClientCompany] FOREIGN KEY ([ClientCompanyID]) REFERENCES [dbo].[ClientCompany] ([ID]),
    CONSTRAINT [FK_ClientSaleApplication_ClientInfoContact] FOREIGN KEY ([ClientContactID]) REFERENCES [dbo].[ClientInfoContact] ([ID]),
    CONSTRAINT [FK_ClientSaleApplication_ClientUser] FOREIGN KEY ([ClientUserID]) REFERENCES [dbo].[ClientUser] ([ID]),
    CONSTRAINT [FK_ClientSaleApplication_SalesModel] FOREIGN KEY ([SalesModelID]) REFERENCES [dbo].[SalesModel] ([ID]),
    CONSTRAINT [FK_ClientSaleApplication_SalesOrderApplication] FOREIGN KEY ([SalesOrderApplicationID]) REFERENCES [dbo].[SalesOrderApplication] ([ID])
);



