CREATE TABLE [dbo].[Department] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [DepartmentName]   NVARCHAR (255) NULL,
    [DirectorUserID]   INT            NULL,
    [DepartmentTypeID] INT            NOT NULL,
    [IsDeleted]        BIT            CONSTRAINT [DF_Department_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]        DATETIME       CONSTRAINT [DF_Department_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT            NULL,
    [LastModifiedOn]   DATETIME       NULL,
    [LastModifiedBy]   INT            NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Department_Users] FOREIGN KEY ([DirectorUserID]) REFERENCES [dbo].[Users] ([UserID])
);



