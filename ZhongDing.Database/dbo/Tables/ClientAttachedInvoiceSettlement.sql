CREATE TABLE [dbo].[ClientAttachedInvoiceSettlement] (
    [ID]                    INT             IDENTITY (1, 1) NOT NULL,
    [ClientUserID]          INT             NOT NULL,
    [CompanyID]             INT             NOT NULL,
    [ClientCompanyID]       INT             NOT NULL,
    [WorkflowStatusID]      INT             NOT NULL,
    [ReceiveBankAccountID]  INT             NOT NULL,
    [ReceiveAccount]        NVARCHAR (255)  NULL,
    [ReceiveAmount]         MONEY           NOT NULL,
    [OtherCostTypeID]       INT             NULL,
    [OtherCostAmount]       MONEY           NULL,
    [ConfirmDate]           DATETIME        NULL,
    [SettlementDate]        DATETIME        NULL,
    [TotalSettlementAmount] MONEY           NULL,
    [TotalRefundAmount]     MONEY           NULL,
    [PaidDate]              DATETIME        NULL,
    [PaidBy]                INT             NULL,
    [AppPaymentID]          INT             NULL,
    [CanceledAppPaymentID]  INT             NULL,
    [IsCanceled]            BIT             CONSTRAINT [DF_ClientAttachedInvoiceSettlement_IsCanceled] DEFAULT ((0)) NOT NULL,
    [CanceledReason]        NVARCHAR (1000) NULL,
    [CanceledDate]          DATETIME        NULL,
    [CanceledBy]            INT             NULL,
    [IsDeleted]             BIT             NOT NULL,
    [CreatedOn]             DATETIME        NOT NULL,
    [CreatedBy]             INT             NULL,
    [LastModifiedOn]        DATETIME        NULL,
    [LastModifiedBy]        INT             NULL,
    CONSTRAINT [PK_ClientAttachedInvoiceSettlement] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientAttachedInvoiceSettlement_ApplicationPayment] FOREIGN KEY ([AppPaymentID]) REFERENCES [dbo].[ApplicationPayment] ([ID]),
    CONSTRAINT [FK_ClientAttachedInvoiceSettlement_BankAccount] FOREIGN KEY ([ReceiveBankAccountID]) REFERENCES [dbo].[BankAccount] ([ID]),
    CONSTRAINT [FK_ClientAttachedInvoiceSettlement_CanceledApplicationPayment] FOREIGN KEY ([CanceledAppPaymentID]) REFERENCES [dbo].[ApplicationPayment] ([ID]),
    CONSTRAINT [FK_ClientAttachedInvoiceSettlement_ClientCompany] FOREIGN KEY ([ClientCompanyID]) REFERENCES [dbo].[ClientCompany] ([ID]),
    CONSTRAINT [FK_ClientAttachedInvoiceSettlement_ClientUser] FOREIGN KEY ([ClientUserID]) REFERENCES [dbo].[ClientUser] ([ID]),
    CONSTRAINT [FK_ClientAttachedInvoiceSettlement_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_ClientAttachedInvoiceSettlement_CostType] FOREIGN KEY ([OtherCostTypeID]) REFERENCES [dbo].[CostType] ([ID]),
    CONSTRAINT [FK_ClientAttachedInvoiceSettlement_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);





