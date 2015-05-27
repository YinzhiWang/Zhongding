CREATE TABLE [dbo].[UserGroup] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [GroupName]      NVARCHAR (256) NOT NULL,
    [IsDeleted]      BIT            NOT NULL,
    [CreatedOn]      DATETIME       NOT NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    [Comment]        NVARCHAR (256) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

