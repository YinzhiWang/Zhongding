CREATE TABLE [dbo].[DistributionCompany] (
    [ID]             INT            NULL,
    [Name]           NVARCHAR (50)  NOT NULL,
    [ReceiverName]   NVARCHAR (50)  NULL,
    [PhoneNumber]    NVARCHAR (50)  NULL,
    [Address]        NVARCHAR (255) NULL,
    [IsDeleted]      BIT            NULL,
    [CreatedOn]      DATETIME       NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL
);

