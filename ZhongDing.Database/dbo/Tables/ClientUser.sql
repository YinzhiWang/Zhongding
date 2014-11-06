CREATE TABLE [dbo].[ClientUser] (
    [ID]             INT           NOT NULL,
    [ClientName]     NVARCHAR (50) NOT NULL,
    [IsDeleted]      BIT           NULL,
    [CreatedOn]      DATETIME      NULL,
    [CreatedBy]      INT           NULL,
    [LastModifiedOn] DATETIME      NULL,
    [LastModifiedBy] INT           NULL,
    CONSTRAINT [PK_ClientUser] PRIMARY KEY CLUSTERED ([ID] ASC)
);



