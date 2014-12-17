CREATE TABLE [dbo].[WorkflowStatus] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [StatusName] NVARCHAR (50)  NULL,
    [Comment]    NVARCHAR (500) NULL,
    [IsDeleted]  BIT            CONSTRAINT [DF_WorkflowStatus_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WorkflowStatus] PRIMARY KEY CLUSTERED ([ID] ASC)
);

