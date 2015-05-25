CREATE TABLE [dbo].[SalesOrderApplicationImportData] (
    [ID]                             INT           IDENTITY (1, 1) NOT NULL,
    [SalesOrderApplicationID]        INT           NULL,
    [SaleOrderType]                  NVARCHAR (50) NOT NULL,
    [OrderCode]                      NVARCHAR (50) NULL,
    [OrderDate]                      DATETIME      NULL,
    [CreatedOn]                      DATETIME      NOT NULL,
    [CreatedBy]                      INT           NULL,
    [LastModifiedOn]                 DATETIME      NULL,
    [LastModifiedBy]                 INT           NULL,
    [SaleApplicationImportFileLogID] INT           NULL,
    CONSTRAINT [PK_SalesOrderApplicationImportData] PRIMARY KEY CLUSTERED ([ID] ASC)
);

