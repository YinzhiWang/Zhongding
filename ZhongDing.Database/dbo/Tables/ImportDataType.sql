CREATE TABLE [dbo].[ImportDataType] (
    [ID]       INT           IDENTITY (1, 1) NOT NULL,
    [TypeName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_ImportDataType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

