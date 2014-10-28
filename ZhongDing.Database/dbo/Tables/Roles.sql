CREATE TABLE [dbo].[Roles] (
    [RoleID]          INT              IDENTITY (1, 1) NOT NULL,
    [AspnetRoleID]    UNIQUEIDENTIFIER NULL,
    [RoleName]        NVARCHAR (256)   COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [IsSystemDefault] BIT              CONSTRAINT [DF_Roles_IsSystemDefault_1] DEFAULT ((0)) NOT NULL,
    [IsDeleted]       BIT              CONSTRAINT [DF_Roles_IsDeleted_1] DEFAULT ((0)) NOT NULL,
    [DeletedOn]       DATETIME         NULL,
    [CreatedOn]       DATETIME         CONSTRAINT [DF_Roles_CreatedOn_1] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       INT              NULL,
    [LastModifiedOn]  DATETIME         NULL,
    [LastModifiedBy]  INT              NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleID] ASC),
    CONSTRAINT [FK_Roles_aspnet_Roles] FOREIGN KEY ([AspnetRoleID]) REFERENCES [dbo].[aspnet_Roles] ([RoleId])
);

