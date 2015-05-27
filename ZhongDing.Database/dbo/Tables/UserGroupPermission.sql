CREATE TABLE [dbo].[UserGroupPermission] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [UserGroupID]    INT      NOT NULL,
    [PermissionID]   INT      NOT NULL,
    [Value]          INT      NOT NULL,
    [IsDeleted]      BIT      NOT NULL,
    [CreatedOn]      DATETIME NOT NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserGroupPermission_PermissionID] FOREIGN KEY ([PermissionID]) REFERENCES [dbo].[Permission] ([ID]),
    CONSTRAINT [FK_UserGroupPermission_UserGroupID] FOREIGN KEY ([UserGroupID]) REFERENCES [dbo].[UserGroup] ([ID])
);

