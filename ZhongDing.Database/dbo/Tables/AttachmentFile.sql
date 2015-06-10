CREATE TABLE [dbo].[AttachmentFile] (
    [ID]                    INT             IDENTITY (1, 1) NOT NULL,
    [AttachmentHostTableID] INT             NOT NULL,
    [AttachmenTypeID]       INT             NOT NULL,
    [FileName]              NVARCHAR (255)  NULL,
    [FilePath]              NVARCHAR (512)  NULL,
    [Comment]               NVARCHAR (1000) NULL,
    [IsDeleted]             BIT             DEFAULT ((0)) NOT NULL,
    [CreatedOn]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedBy]             INT             NULL,
    [LastModifiedOn]        DATETIME        NULL,
    [LastModifiedBy]        INT             NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

