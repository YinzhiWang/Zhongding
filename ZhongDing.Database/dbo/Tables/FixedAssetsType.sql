CREATE TABLE [dbo].[FixedAssetsType] (
    [ID]             INT            NOT NULL,
    [Name]           NVARCHAR (256) NOT NULL,
    [IsDeleted]      BIT            DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

