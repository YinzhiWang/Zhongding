CREATE TABLE [dbo].[Supplier] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [SupplierCode]   NVARCHAR (50)  NULL,
    [SupplierName]   NVARCHAR (100) NULL,
    [District]       NVARCHAR (50)  NULL,
    [FactoryName]    NVARCHAR (100) NULL,
    [ContactPerson]  NVARCHAR (50)  NULL,
    [PhoneNumber]    NVARCHAR (20)  NULL,
    [ContactAddress] NVARCHAR (255) NULL,
    [Fax]            NVARCHAR (20)  NULL,
    [PostalCode]     NVARCHAR (10)  NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF_Supplier_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedOn]      DATETIME       NULL,
    [CreatedOn]      DATETIME       CONSTRAINT [DF_Supplier_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED ([ID] ASC)
);

