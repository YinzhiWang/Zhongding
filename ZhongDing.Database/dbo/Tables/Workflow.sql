CREATE TABLE [dbo].[Workflow] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [WorkflowName] NVARCHAR (50) NULL,
    [IsActive]     BIT           CONSTRAINT [DF_Workflow_IsActive] DEFAULT ((0)) NOT NULL,
    [IsDeleted]    BIT           CONSTRAINT [DF_Workflow_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Workflow] PRIMARY KEY CLUSTERED ([ID] ASC)
);



