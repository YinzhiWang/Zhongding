CREATE TABLE [dbo].[DCImportFileLog] (
    [ImportFileLogID]       INT      NOT NULL,
    [DistributionCompanyID] INT      NOT NULL,
    [SettlementDate]        DATETIME NOT NULL,
    [IsDeleted]             BIT      CONSTRAINT [DF_DCImportFileLog_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]             DATETIME CONSTRAINT [DF_DCImportFileLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]             INT      NULL,
    [LastModifiedOn]        DATETIME NULL,
    [LastModifiedBy]        INT      NULL,
    CONSTRAINT [PK_DCImportFileLog] PRIMARY KEY CLUSTERED ([ImportFileLogID] ASC),
    CONSTRAINT [FK_DCImportFileLog_ImportFileLog] FOREIGN KEY ([ImportFileLogID]) REFERENCES [dbo].[ImportFileLog] ([ID])
);

