CREATE TABLE [dbo].[DepartmentProductRecord] (
    [ID]             INT      NOT NULL,
    [DepartmentID]   INT      IDENTITY (1, 1) NOT NULL,
    [ProductID]      INT      NOT NULL,
    [Year]           INT      NOT NULL,
    [Task]           INT      NULL,
    [Actual]         INT      NULL,
    [IsDeleted]      BIT      NULL,
    [CreatedOn]      DATETIME NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    CONSTRAINT [PK_DepartmentProductRecord] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DepartmentProductRecord_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([ID]),
    CONSTRAINT [FK_DepartmentProductRecord_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID])
);

