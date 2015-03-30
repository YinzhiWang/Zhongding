CREATE TABLE [dbo].[DCInventoryFlowData] (
    [ID]                     INT            NOT NULL,
    [DistributionCompanyID]  INT            NULL,
    [ImportFileLogID]        INT            NULL,
    [ProductID]              INT            NOT NULL,
    [ProductName]            NVARCHAR (255) NULL,
    [ProductCode]            NVARCHAR (50)  NULL,
    [ProductSpecificationID] INT            NOT NULL,
    [ProductSpecification]   NVARCHAR (50)  NULL,
    [SettlementDate]         DATETIME       NOT NULL,
    [BalanceQty]             INT            NOT NULL,
    [CreatedOn]              DATETIME       CONSTRAINT [DF_DCInventoryFlowData_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT            NULL,
    CONSTRAINT [PK_DCInventoryFlowData] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DCInventoryFlowData_ImportFileLog] FOREIGN KEY ([ImportFileLogID]) REFERENCES [dbo].[ImportFileLog] ([ID]),
    CONSTRAINT [FK_DCInventoryFlowData_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_DCInventoryFlowData_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID])
);

