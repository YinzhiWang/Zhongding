CREATE TABLE [dbo].[DCFlowData] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [DistributionCompanyID]  INT            NULL,
    [ImportFileLogID]        INT            NOT NULL,
    [ProductID]              INT            NOT NULL,
    [ProductName]            NVARCHAR (255) NULL,
    [ProductCode]            NVARCHAR (50)  NULL,
    [ProductSpecificationID] INT            NOT NULL,
    [ProductSpecification]   NVARCHAR (50)  NULL,
    [SaleDate]               DATETIME       NOT NULL,
    [SaleQty]                INT            NOT NULL,
    [SettlementDate]         DATETIME       NOT NULL,
    [HospitalID]             INT            NULL,
    [FlowTo]                 NVARCHAR (255) NULL,
    [IsCorrectlyFlow]        BIT            NULL,
    [IsOverwritten]          BIT            NULL,
    [OldDCFlowDataID]        INT            NULL,
    [CreatedOn]              DATETIME       CONSTRAINT [DF_DCFlowData_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT            NULL,
    CONSTRAINT [PK_DCFlowData] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DCFlowData_DistributionCompany] FOREIGN KEY ([DistributionCompanyID]) REFERENCES [dbo].[DistributionCompany] ([ID]),
    CONSTRAINT [FK_DCFlowData_Hospital] FOREIGN KEY ([HospitalID]) REFERENCES [dbo].[Hospital] ([ID]),
    CONSTRAINT [FK_DCFlowData_ImportFileLog] FOREIGN KEY ([ImportFileLogID]) REFERENCES [dbo].[ImportFileLog] ([ID]),
    CONSTRAINT [FK_DCFlowData_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_DCFlowData_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID])
);



