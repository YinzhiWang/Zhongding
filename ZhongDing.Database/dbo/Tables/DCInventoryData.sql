CREATE TABLE [dbo].[DCInventoryData] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [DistributionCompanyID]  INT            NULL,
    [ImportFileLogID]        INT            NOT NULL,
    [ProductID]              INT            NOT NULL,
    [ProductName]            NVARCHAR (255) NULL,
    [ProductCode]            NVARCHAR (50)  NULL,
    [ProductSpecificationID] INT            NOT NULL,
    [ProductSpecification]   NVARCHAR (50)  NULL,
    [SettlementDate]         DATETIME       NOT NULL,
    [BalanceQty]             INT            NULL,
    [CreatedOn]              DATETIME       CONSTRAINT [DF_DCInventoryData_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT            NULL,
    CONSTRAINT [PK_DCInventoryData] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DCInventoryData_DistributionCompany] FOREIGN KEY ([DistributionCompanyID]) REFERENCES [dbo].[DistributionCompany] ([ID]),
    CONSTRAINT [FK_DCInventoryData_ImportFileLog] FOREIGN KEY ([ImportFileLogID]) REFERENCES [dbo].[ImportFileLog] ([ID]),
    CONSTRAINT [FK_DCInventoryData_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_DCInventoryData_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID])
);



