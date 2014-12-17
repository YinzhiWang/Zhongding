CREATE TABLE [dbo].[ApplicationNote] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationID]  INT            NULL,
    [WorkflowID]     INT            NOT NULL,
    [WorkflowStepID] INT            NOT NULL,
    [Note]           NVARCHAR (500) NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF_ApplicationNotes_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME       CONSTRAINT [DF_ApplicationNotes_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    CONSTRAINT [PK_ApplicationNotes] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ApplicationNote_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID]),
    CONSTRAINT [FK_ApplicationNote_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
);

