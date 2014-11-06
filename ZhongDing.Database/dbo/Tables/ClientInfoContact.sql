CREATE TABLE [dbo].[ClientInfoContact] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [ClientInfoID]   INT            NOT NULL,
    [ContactName]    NVARCHAR (50)  NULL,
    [PhoneNumber]    NVARCHAR (50)  NULL,
    [Address]        NVARCHAR (255) NULL,
    [Comment]        NVARCHAR (255) NULL,
    [IsDeleted]      BIT            NULL,
    [CreatedOn]      DATETIME       NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    CONSTRAINT [PK_ClientInfoContact] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientInfoContact_ClientInfo] FOREIGN KEY ([ClientInfoID]) REFERENCES [dbo].[ClientInfo] ([ID])
);

