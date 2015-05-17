CREATE TABLE [dbo].[ProcureOrderApplicationImportFileLog] (
    [ID]              INT      IDENTITY (1, 1) NOT NULL,
    [ImportFileLogID] INT      NOT NULL,
    [CreatedOn]       DATETIME NOT NULL,
    [CreatedBy]       INT      NULL,
    CONSTRAINT [PK_ProcureOrderApplicationImportFileLog] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProcureOrderApplicationImportFileLog_ImportFileLogID] FOREIGN KEY ([ImportFileLogID]) REFERENCES [dbo].[ImportFileLog] ([ID])
);

