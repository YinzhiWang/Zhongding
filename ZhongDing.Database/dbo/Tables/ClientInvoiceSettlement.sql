CREATE TABLE [dbo].[ClientInvoiceSettlement] (
    [ID]                 INT             IDENTITY (1, 1) NOT NULL,
    [SettlementDate]     DATETIME        NOT NULL,
    [CompanyID]          INT             NOT NULL,
    [ClientCompanyID]    INT             NOT NULL,
    [WorkflowStatusID]   INT             NOT NULL,
    [TotalInvoiceAmount] MONEY           NOT NULL,
    [TotalPayAmount]     MONEY           NOT NULL,
    [IsCanceled]         BIT             CONSTRAINT [DF_ClientInvoiceSettlement_IsCanceled] DEFAULT ((0)) NOT NULL,
    [CanceledReason]     NVARCHAR (1000) NULL,
    [CanceledDate]       DATETIME        NULL,
    [CanceledBy]         INT             NULL,
    [IsDeleted]          BIT             NOT NULL,
    [CreatedOn]          DATETIME        NOT NULL,
    [CreatedBy]          INT             NULL,
    [LastModifiedOn]     DATETIME        NULL,
    [LastModifiedBy]     INT             NULL,
    CONSTRAINT [PK_ClientInvoiceSettlement] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientInvoiceSettlement_ClientCompany] FOREIGN KEY ([ClientCompanyID]) REFERENCES [dbo].[ClientCompany] ([ID]),
    CONSTRAINT [FK_ClientInvoiceSettlement_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_ClientInvoiceSettlement_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);



