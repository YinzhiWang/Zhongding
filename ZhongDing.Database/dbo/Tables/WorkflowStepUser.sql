CREATE TABLE [dbo].[WorkflowStepUser] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [WorkflowStepID] INT      NOT NULL,
    [UserID]         INT      NOT NULL,
    [IsDeleted]      BIT      NULL,
    [CreatedOn]      DATETIME NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    CONSTRAINT [PK_WorkflowUser] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_WorkflowStepUser_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID]),
    CONSTRAINT [FK_WorkflowStepUser_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
);

