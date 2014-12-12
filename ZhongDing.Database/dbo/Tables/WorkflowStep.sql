CREATE TABLE [dbo].[WorkflowStep] (
    [ID]         INT           IDENTITY (1, 1) NOT NULL,
    [WorkflowID] INT           NOT NULL,
    [StepName]   NVARCHAR (50) NULL,
    [IsDeleted]  BIT           CONSTRAINT [DF_WorkflowStep_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WorkflowStep] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
);



