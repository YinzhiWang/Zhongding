CREATE TABLE [dbo].[OwnerType] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [OwnerTypeName] NVARCHAR (100) NULL,
    CONSTRAINT [PK_OwnerType] PRIMARY KEY CLUSTERED ([ID] ASC)
);



