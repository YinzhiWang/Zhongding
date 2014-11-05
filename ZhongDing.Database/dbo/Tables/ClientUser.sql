CREATE TABLE [dbo].[ClientUser] (
    [ID]         INT           NOT NULL,
    [ClientName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ClientUser] PRIMARY KEY CLUSTERED ([ID] ASC)
);

