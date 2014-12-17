CREATE TABLE [dbo].[WorkflowStepStatus] (
    [ID]               INT IDENTITY (1, 1) NOT NULL,
    [WorkflowStepID]   INT NOT NULL,
    [WorkflowStatusID] INT NOT NULL,
    [IsDeleted]        BIT CONSTRAINT [DF_WorkflowStepStatus_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WorkflowStepStatus] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID]),
    CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
);

