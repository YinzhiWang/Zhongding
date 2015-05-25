CREATE TABLE [dbo].[ClientSaleApplicationImportFileLog] (
    [ID]              INT      IDENTITY (1, 1) NOT NULL,
    [ImportFileLogID] INT      NOT NULL,
    [CreatedOn]       DATETIME NOT NULL,
    [CreatedBy]       INT      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientSaleApplicationImportFileLog_ImportFileLogID] FOREIGN KEY ([ImportFileLogID]) REFERENCES [dbo].[ImportFileLog] ([ID])
);

