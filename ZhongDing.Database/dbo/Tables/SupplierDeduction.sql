CREATE TABLE [dbo].[SupplierDeduction] (
    [ID]                  INT             IDENTITY (1, 1) NOT NULL,
    [SupplierRefundAppID] INT             NOT NULL,
    [SupplierID]          INT             NOT NULL,
    [Amount]              MONEY           NOT NULL,
    [DeductedDate]        DATETIME        NULL,
    [Comment]             NVARCHAR (1000) NULL,
    [IsDeleted]           BIT             CONSTRAINT [DF_SupplierDeduction_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]           DATETIME        CONSTRAINT [DF_SupplierDeduction_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]           INT             NULL,
    [LastModifiedOn]      DATETIME        NULL,
    [LastModifiedBy]      INT             NULL,
    CONSTRAINT [PK_SupplierDeduction] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierDeduction_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID]),
    CONSTRAINT [FK_SupplierDeduction_SupplierRefundApplication] FOREIGN KEY ([SupplierRefundAppID]) REFERENCES [dbo].[SupplierRefundApplication] ([ID])
);





