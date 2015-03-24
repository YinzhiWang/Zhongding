CREATE TABLE [dbo].[ImportErrorLog] (
    [ID]              INT            NOT NULL,
    [ImportFileLogID] INT            NOT NULL,
    [ErrorRowIndex]   INT            NULL,
    [ErrorRowData]    NVARCHAR (MAX) NULL,
    [ErrorMsg]        NVARCHAR (MAX) NULL,
    [CreatedOn]       DATETIME       CONSTRAINT [DF_ImportErrorLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       INT            NULL,
    CONSTRAINT [PK_ImportErrorLog] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ImportErrorLog_ImportFileLog] FOREIGN KEY ([ImportFileLogID]) REFERENCES [dbo].[ImportFileLog] ([ID])
);

