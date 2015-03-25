CREATE TABLE [dbo].[ImportFileLog] (
    [ID]               INT             IDENTITY (1, 1) NOT NULL,
    [ImportDataTypeID] INT             NOT NULL,
    [FileName]         NVARCHAR (500)  NULL,
    [FilePath]         NVARCHAR (2000) NULL,
    [ImportBeginDate]  DATETIME        NULL,
    [ImportEndDate]    DATETIME        NULL,
    [ImportStatusID]   INT             NOT NULL,
    [IsDeleted]        BIT             CONSTRAINT [DF_ImportFileLog_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]        DATETIME        CONSTRAINT [DF_ImportFileLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT             NULL,
    [LastModifiedOn]   DATETIME        NULL,
    [LastModifiedBy]   INT             NULL,
    CONSTRAINT [PK_ImportFileLog] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ImportFileLog_ImportDataType] FOREIGN KEY ([ImportDataTypeID]) REFERENCES [dbo].[ImportDataType] ([ID]),
    CONSTRAINT [FK_ImportFileLog_ImportStatus] FOREIGN KEY ([ImportStatusID]) REFERENCES [dbo].[ImportStatus] ([ID])
);





