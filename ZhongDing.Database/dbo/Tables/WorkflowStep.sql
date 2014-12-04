CREATE TABLE [dbo].[WorkflowStep] (
    [ID]         INT           IDENTITY (1, 1) NOT NULL,
    [WorkflowID] INT           NULL,
    [StepName]   NVARCHAR (50) NULL,
    [IsDeleted]  BIT           NULL,
    CONSTRAINT [PK_WorkflowStep] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
);

