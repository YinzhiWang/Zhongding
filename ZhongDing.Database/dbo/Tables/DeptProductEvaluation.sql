CREATE TABLE [dbo].[DeptProductEvaluation] (
    [ID]               INT        IDENTITY (1, 1) NOT NULL,
    [DepartmentID]     INT        NOT NULL,
    [ProductID]        INT        NOT NULL,
    [InvestigateRatio] FLOAT (53) NULL,
    [SalesRatio]       FLOAT (53) NULL,
    [IsDeleted]        BIT        CONSTRAINT [DF_DeptProductEvaluation_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]        DATETIME   CONSTRAINT [DF_DeptProductEvaluation_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT        NULL,
    [LastModfiiedOn]   DATETIME   NULL,
    [LastModifiedBy]   INT        NULL,
    CONSTRAINT [PK_DeptProductEvaluation] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DeptProductEvaluation_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([ID]),
    CONSTRAINT [FK_DeptProductEvaluation_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID])
);

