CREATE TABLE [dbo].[DCInventoryData] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [DistributionCompanyID]  INT            NULL,
    [ImportFileLogID]        INT            NOT NULL,
    [ProductID]              INT            NOT NULL,
    [ProductName]            NVARCHAR (255) NULL,
    [ProductCode]            NVARCHAR (50)  NULL,
    [ProductSpecificationID] INT            NOT NULL,
    [ProductSpecification]   NVARCHAR (50)  NULL,
    [UnitName]               NVARCHAR (50)  NULL,
    [Qty]                    INT            NULL,
    [CreatedOn]              DATETIME       CONSTRAINT [DF_DCInventoryData_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT            NULL,
    CONSTRAINT [PK_DCInventoryData] PRIMARY KEY CLUSTERED ([ID] ASC)
);

