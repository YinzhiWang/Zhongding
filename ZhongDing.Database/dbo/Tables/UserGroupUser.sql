CREATE TABLE [dbo].[UserGroupUser] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [UserGroupID]    INT      NOT NULL,
    [UserID]         INT      NOT NULL,
    [IsDeleted]      BIT      NOT NULL,
    [CreatedOn]      DATETIME NOT NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserGroupUser_UserGroupID] FOREIGN KEY ([UserGroupID]) REFERENCES [dbo].[UserGroup] ([ID]),
    CONSTRAINT [FK_UserGroupUser_UserID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

