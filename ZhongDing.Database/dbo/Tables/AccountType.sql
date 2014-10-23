CREATE TABLE [dbo].[AccountType] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [AccountTypeName] NVARCHAR (100) NULL,
    CONSTRAINT [PK_AccountType] PRIMARY KEY CLUSTERED ([ID] ASC)
);



