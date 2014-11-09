CREATE TABLE [dbo].[Supplier] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [CompanyID]      INT            NULL,
    [SupplierCode]   NVARCHAR (50)  NULL,
    [SupplierName]   NVARCHAR (100) NULL,
    [District]       NVARCHAR (50)  NULL,
    [FactoryName]    NVARCHAR (100) NULL,
    [ContactPerson]  NVARCHAR (50)  NULL,
    [PhoneNumber]    NVARCHAR (20)  NULL,
    [ContactAddress] NVARCHAR (255) NULL,
    [Fax]            NVARCHAR (20)  NULL,
    [PostalCode]     NVARCHAR (10)  NULL,
    [IsProducer]     BIT            CONSTRAINT [DF_Supplier_IsProducer] DEFAULT ((0)) NOT NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF_Supplier_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME       CONSTRAINT [DF_Supplier_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Supplier_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID])
);





