CREATE TABLE [dbo].[StorageLocation] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (256) NOT NULL,
    [Comment]        NVARCHAR (256) NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF__StorageLo__IsDel__22800C64] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME       CONSTRAINT [DF__StorageLo__Creat__2374309D] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    CONSTRAINT [PK__StorageL__3214EC27381E0AC3] PRIMARY KEY CLUSTERED ([ID] ASC)
);

