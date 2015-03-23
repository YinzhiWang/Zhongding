CREATE TABLE [dbo].[ImportStatus] (
    [ID]         INT           IDENTITY (1, 1) NOT NULL,
    [StatusName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_ImportStatus] PRIMARY KEY CLUSTERED ([ID] ASC)
);

