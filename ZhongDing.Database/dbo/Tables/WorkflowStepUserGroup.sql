CREATE TABLE [dbo].[WorkflowStepUserGroup] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [WorkflowStepID] INT      NOT NULL,
    [UserGroupID]    INT      NOT NULL,
    [IsDeleted]      BIT      CONSTRAINT [DF_WorkflowStepUserGroup_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME CONSTRAINT [DF_WorkflowStepUserGroup_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    CONSTRAINT [PK_WorkflowUserGroup] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_WorkflowStepUserGroup_UserGroupID] FOREIGN KEY ([UserGroupID]) REFERENCES [dbo].[UserGroup] ([ID]),
    CONSTRAINT [FK_WorkflowStepUserGroup_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
);

