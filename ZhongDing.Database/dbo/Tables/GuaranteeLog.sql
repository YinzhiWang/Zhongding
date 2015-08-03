CREATE TABLE [dbo].[GuaranteeLog] (
    [ID]                      INT      IDENTITY (1, 1) NOT NULL,
    [GuaranteeReceiptID]      INT      NOT NULL,
    [ClientSaleApplicationID] INT      NOT NULL,
    [GuaranteeAmount]         MONEY    NULL,
    [Guaranteeby]             INT      NULL,
    [GuaranteeExpirationDate] DATETIME NULL,
    [IsReceipted]             BIT      CONSTRAINT [DF_GuaranteeLog_IsReturnedGuaranteeAmount] DEFAULT ((0)) NOT NULL,
    [GuaranteeReceiptDate]    DATETIME NOT NULL,
    [IsDeleted]               BIT      CONSTRAINT [DF_GuaranteeLog_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]               DATETIME CONSTRAINT [DF_GuaranteeLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]               INT      NULL,
    [LastModifiedOn]          DATETIME NULL,
    [LastModifiedBy]          INT      NULL,
    CONSTRAINT [PK_GuaranteeLog] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_GuaranteeLog_ClientSaleApplication] FOREIGN KEY ([ClientSaleApplicationID]) REFERENCES [dbo].[ClientSaleApplication] ([ID]),
    CONSTRAINT [FK_GuaranteeLog_GuaranteeReceiptID] FOREIGN KEY ([GuaranteeReceiptID]) REFERENCES [dbo].[GuaranteeReceipt] ([ID])
);



