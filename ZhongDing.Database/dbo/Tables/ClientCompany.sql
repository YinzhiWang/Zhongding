CREATE TABLE [dbo].[ClientCompany] (
    [ID]             INT            NOT NULL,
    [Name]           NVARCHAR (50)  NULL,
    [District]       NVARCHAR (50)  NULL,
    [Address]        NVARCHAR (255) NULL,
    [PostalCode]     NVARCHAR (50)  NULL,
    [IsDeleted]      BIT            NULL,
    [CreatedOn]      DATETIME       NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    CONSTRAINT [PK_ClientCompany] PRIMARY KEY CLUSTERED ([ID] ASC)
);

