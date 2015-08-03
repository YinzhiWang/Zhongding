CREATE TABLE [dbo].[SupplierContact] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [SupplierID]     INT            NOT NULL,
    [ContactName]    NVARCHAR (50)  NULL,
    [PhoneNumber]    NVARCHAR (50)  NULL,
    [Address]        NVARCHAR (255) NULL,
    [Comment]        NVARCHAR (255) NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF_SupplierContact_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME       CONSTRAINT [DF_SupplierContact_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    CONSTRAINT [PK_SupplierContact] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierContact_SupplierID] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID])
);

