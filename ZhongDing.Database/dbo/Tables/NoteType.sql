CREATE TABLE [dbo].[NoteType] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [NoteTypeName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_NoteType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

