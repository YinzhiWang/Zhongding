CREATE TABLE [dbo].[DepartmentProductRecord] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [DepartmentID]   INT      NOT NULL,
    [ProductID]      INT      NOT NULL,
    [Year]           INT      NOT NULL,
    [Task]           INT      NULL,
    [Actual]         INT      NULL,
    [IsDeleted]      BIT      CONSTRAINT [DF_DepartmentProductRecord_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME CONSTRAINT [DF_DepartmentProductRecord_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    CONSTRAINT [PK_DepartmentProductRecord] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DepartmentProductRecord_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([ID]),
    CONSTRAINT [FK_DepartmentProductRecord_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID])
);





