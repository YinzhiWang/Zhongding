CREATE TABLE [dbo].[DistributionCompany] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [SerialNo]       NVARCHAR (50)  NULL,
    [Name]           NVARCHAR (50)  NOT NULL,
    [ReceiverName]   NVARCHAR (50)  NULL,
    [PhoneNumber]    NVARCHAR (50)  NULL,
    [Address]        NVARCHAR (255) NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF_DistributionCompany_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME       CONSTRAINT [DF_DistributionCompany_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    CONSTRAINT [PK_DistributionCompany] PRIMARY KEY CLUSTERED ([ID] ASC)
);







