CREATE TABLE [dbo].[StockInImportData] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [StockInID]              INT            NULL,
    [StockInImportFileLogID] INT            NOT NULL,
    [Code]                   NVARCHAR (50)  NULL,
    [EntryDate]              DATETIME       NULL,
    [SupplierName]           NVARCHAR (256) NOT NULL,
    [CreatedOn]              DATETIME       NOT NULL,
    [CreatedBy]              INT            NULL,
    [LastModifiedOn]         DATETIME       NULL,
    [LastModifiedBy]         INT            NULL,
    CONSTRAINT [PK_StockInImportData] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_StockInImportData_StockInImportFileLogID] FOREIGN KEY ([StockInImportFileLogID]) REFERENCES [dbo].[StockInImportFileLog] ([ID])
);

