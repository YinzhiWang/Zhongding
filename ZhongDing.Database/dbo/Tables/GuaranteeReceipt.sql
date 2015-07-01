CREATE TABLE [dbo].[GuaranteeReceipt] (
    [ID]                   INT      IDENTITY (1, 1) NOT NULL,
    [ReceiptDate]          DATETIME NOT NULL,
    [ReceiptAmount]        MONEY    DEFAULT ((0)) NOT NULL,
    [ApplicationPaymentID] INT      NOT NULL,
    [IsDeleted]            BIT      DEFAULT ((0)) NOT NULL,
    [CreatedOn]            DATETIME DEFAULT (getdate()) NOT NULL,
    [CreatedBy]            INT      NULL,
    [LastModifiedOn]       DATETIME NULL,
    [LastModifiedBy]       INT      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_GuaranteeReceipt_ApplicationPaymentID] FOREIGN KEY ([ApplicationPaymentID]) REFERENCES [dbo].[ApplicationPayment] ([ID])
);

