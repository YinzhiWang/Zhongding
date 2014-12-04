CREATE TABLE [dbo].[ApplicationNotes] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationID]  INT            NULL,
    [WorkflowID]     INT            NULL,
    [WorkflowStepID] INT            NULL,
    [Note]           NVARCHAR (500) NULL,
    [IsDeleted]      BIT            NULL,
    [CreatedOn]      DATETIME       NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    CONSTRAINT [PK_ApplicationNotes] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ApplicationNotes_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID]),
    CONSTRAINT [FK_ApplicationNotes_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
);

