CREATE TABLE [dbo].[ReimbursementType] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [ParentID]       INT            NULL,
    [Name]           NVARCHAR (256) NOT NULL,
    [Comment]        NVARCHAR (255) NULL,
    [IsDeleted]      BIT            NOT NULL,
    [CreatedOn]      DATETIME       NOT NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    CONSTRAINT [PK__Reimburs__3214EC276C62A346] PRIMARY KEY CLUSTERED ([ID] ASC)
);

