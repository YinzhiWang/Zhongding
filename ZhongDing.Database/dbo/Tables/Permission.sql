CREATE TABLE [dbo].[Permission] (
    [ID]             INT            NOT NULL,
    [Name]           NVARCHAR (256) NOT NULL,
    [HasCreate]      BIT            NOT NULL,
    [HasEdit]        BIT            NOT NULL,
    [HasDelete]      BIT            NOT NULL,
    [HasView]        BIT            NOT NULL,
    [HasPrint]       BIT            NOT NULL,
    [HasExport]      BIT            NOT NULL,
    [IsDeleted]      BIT            NOT NULL,
    [CreatedOn]      DATETIME       CONSTRAINT [DF_Permission_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    CONSTRAINT [PK__Permissi__3214EC270782FB2E] PRIMARY KEY CLUSTERED ([ID] ASC)
);

