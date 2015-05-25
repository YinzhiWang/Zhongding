CREATE TABLE [dbo].[ClientSaleApplicationImportData] (
    [ID]                                INT            IDENTITY (1, 1) NOT NULL,
    [SalesOrderApplicationImportDataID] INT            NOT NULL,
    [ClientUserName]                    NVARCHAR (256) NULL,
    [ClientCompanyName]                 NVARCHAR (256) NULL,
    [CreatedOn]                         DATETIME       NOT NULL,
    [CreatedBy]                         INT            NULL,
    [LastModifiedOn]                    DATETIME       NULL,
    [LastModifiedBy]                    INT            NULL,
    CONSTRAINT [PK_ClientSaleApplicationImportData] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientSaleApplicationImportData_SalesOrderApplicationImportDataID] FOREIGN KEY ([SalesOrderApplicationImportDataID]) REFERENCES [dbo].[SalesOrderApplicationImportData] ([ID])
);

