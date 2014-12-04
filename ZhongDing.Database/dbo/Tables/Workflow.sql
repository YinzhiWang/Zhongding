CREATE TABLE [dbo].[Workflow] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [WorkflowName] NVARCHAR (50) NULL,
    [IsActive]     BIT           NULL,
    [IsDeleted]    BIT           NULL,
    CONSTRAINT [PK_Workflow] PRIMARY KEY CLUSTERED ([ID] ASC)
);

