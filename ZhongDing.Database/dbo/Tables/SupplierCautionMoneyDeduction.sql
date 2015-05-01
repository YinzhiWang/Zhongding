CREATE TABLE [dbo].[SupplierCautionMoneyDeduction] (
    [ID]                     INT             IDENTITY (1, 1) NOT NULL,
    [SupplierCautionMoneyID] INT             NOT NULL,
    [SupplierID]             INT             NOT NULL,
    [Amount]                 MONEY           NOT NULL,
    [DeductedDate]           DATETIME        NULL,
    [Comment]                NVARCHAR (1000) NULL,
    [IsDeleted]              BIT             NOT NULL,
    [CreatedOn]              DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT             NULL,
    [LastModifiedOn]         DATETIME        NULL,
    [LastModifiedBy]         INT             NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierCautionMoneyDeduction_SupplierCautionMoneyID] FOREIGN KEY ([SupplierCautionMoneyID]) REFERENCES [dbo].[SupplierCautionMoney] ([ID]),
    CONSTRAINT [FK_SupplierCautionMoneyDeduction_SupplierID] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID])
);

