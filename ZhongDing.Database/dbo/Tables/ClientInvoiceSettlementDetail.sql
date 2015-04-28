CREATE TABLE [dbo].[ClientInvoiceSettlementDetail] (
    [ID]                        INT             IDENTITY (1, 1) NOT NULL,
    [ClientInvoiceSettlementID] INT             NOT NULL,
    [ClientCompanyID]           INT             NOT NULL,
    [ClientInvoiceID]           INT             NOT NULL,
    [InvoiceDate]               DATETIME        NOT NULL,
    [InvoiceNumber]             NVARCHAR (256)  NOT NULL,
    [TotalInvoiceAmount]        MONEY           NOT NULL,
    [ClientTaxHighRatio]        DECIMAL (18, 8) NULL,
    [HighRatioAmount]           MONEY           NULL,
    [ClientTaxLowRatio]         DECIMAL (18, 8) NULL,
    [LowRatioAmount]            MONEY           NULL,
    [ClientTaxDeductionRatio]   DECIMAL (18, 8) NULL,
    [DeductionRatioAmount]      MONEY           NULL,
    [PayAmount]                 MONEY           NOT NULL,
    [IsDeleted]                 BIT             NOT NULL,
    [CreatedOn]                 DATETIME        NOT NULL,
    [CreatedBy]                 INT             NULL,
    [LastModifiedOn]            DATETIME        NULL,
    [LastModifiedBy]            INT             NULL,
    CONSTRAINT [PK_ClientInvoiceSettlementDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientInvoiceSettlementDetail_ClientCompany] FOREIGN KEY ([ClientCompanyID]) REFERENCES [dbo].[ClientCompany] ([ID]),
    CONSTRAINT [FK_ClientInvoiceSettlementDetail_ClientInvoice] FOREIGN KEY ([ClientInvoiceID]) REFERENCES [dbo].[ClientInvoice] ([ID]),
    CONSTRAINT [FK_ClientInvoiceSettlementDetail_ClientInvoiceSettlement] FOREIGN KEY ([ClientInvoiceSettlementID]) REFERENCES [dbo].[ClientInvoiceSettlement] ([ID])
);





