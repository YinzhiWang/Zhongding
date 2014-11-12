CREATE TABLE [dbo].[ClientCompany] (
    [ID]             INT            NOT NULL,
    [Name]           NVARCHAR (50)  NULL,
    [District]       NVARCHAR (50)  NULL,
    [Address]        NVARCHAR (255) NULL,
    [PostalCode]     NVARCHAR (50)  NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF_ClientCompany_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME       CONSTRAINT [DF_ClientCompany_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    CONSTRAINT [PK_ClientCompany] PRIMARY KEY CLUSTERED ([ID] ASC)
);



