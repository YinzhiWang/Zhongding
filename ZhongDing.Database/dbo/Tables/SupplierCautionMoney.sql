CREATE TABLE [dbo].[SupplierCautionMoney] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [SupplierID]             INT            NOT NULL,
    [ProductID]              INT            NOT NULL,
    [ProductSpecificationID] INT            NOT NULL,
    [CautionMoneyTypeID]     INT            NOT NULL,
    [EndDate]                DATETIME       NOT NULL,
    [PaymentCautionMoney]    MONEY          NOT NULL,
    [TakeBackCautionMoney]   MONEY          CONSTRAINT [DF__SupplierC__TakeB__0643C069] DEFAULT ((0)) NOT NULL,
    [IsDeleted]              BIT            NOT NULL,
    [CreatedOn]              DATETIME       NOT NULL,
    [CreatedBy]              INT            NULL,
    [LastModifiedOn]         DATETIME       NULL,
    [LastModifiedBy]         INT            NULL,
    [WorkflowStatusID]       INT            NOT NULL,
    [IsStop]                 BIT            NOT NULL,
    [StoppedOn]              DATETIME       NULL,
    [StoppedBy]              INT            NULL,
    [ApplyDate]              DATETIME       NOT NULL,
    [Remark]                 NVARCHAR (256) NULL,
    [CompanyID]              INT            NOT NULL,
    [PaidDate]               DATETIME       NULL,
    [PaidBy]                 INT            NULL,
    CONSTRAINT [PK__Supplier__3214EC271D04A192] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierCautionMoney_CautionMoneyTypeID] FOREIGN KEY ([CautionMoneyTypeID]) REFERENCES [dbo].[CautionMoneyType] ([ID]),
    CONSTRAINT [FK_SupplierCautionMoney_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_SupplierCautionMoney_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_SupplierCautionMoney_ProductSpecificationID] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID]),
    CONSTRAINT [FK_SupplierCautionMoney_SupplierID] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID]),
    CONSTRAINT [FK_SupplierCautionMoney_WorkflowStatusID] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);





