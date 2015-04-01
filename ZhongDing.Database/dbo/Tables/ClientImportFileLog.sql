CREATE TABLE [dbo].[ClientImportFileLog] (
    [ImportFileLogID] INT      NOT NULL,
    [ClientUserID]    INT      NOT NULL,
    [ClientCompanyID] INT      NOT NULL,
    [SettlementDate]  DATETIME NOT NULL,
    [IsDeleted]       BIT      CONSTRAINT [DF_ClientImportFileLog_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]       DATETIME CONSTRAINT [DF_ClientImportFileLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       INT      NULL,
    [LastModifiedOn]  DATETIME NULL,
    [LastModifiedBy]  INT      NULL,
    CONSTRAINT [PK_ClientImportFileLog] PRIMARY KEY CLUSTERED ([ImportFileLogID] ASC),
    CONSTRAINT [FK_ClientImportFileLog_ClientCompany] FOREIGN KEY ([ClientCompanyID]) REFERENCES [dbo].[ClientCompany] ([ID]),
    CONSTRAINT [FK_ClientImportFileLog_ClientUser] FOREIGN KEY ([ClientUserID]) REFERENCES [dbo].[ClientUser] ([ID]),
    CONSTRAINT [FK_ClientImportFileLog_ImportFileLog] FOREIGN KEY ([ImportFileLogID]) REFERENCES [dbo].[ImportFileLog] ([ID])
);



