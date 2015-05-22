CREATE TABLE [dbo].[StockInImportFileLog] (
    [ID]              INT      IDENTITY (1, 1) NOT NULL,
    [ImportFileLogID] INT      NOT NULL,
    [CreatedOn]       DATETIME NOT NULL,
    [CreatedBy]       INT      NULL,
    CONSTRAINT [PK_StockInImportFileLog] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_StockInImportFileLog_ImportFileLogID] FOREIGN KEY ([ImportFileLogID]) REFERENCES [dbo].[ImportFileLog] ([ID])
);

