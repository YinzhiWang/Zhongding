CREATE TABLE [dbo].[SupplierCertificate] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [SupplierID]     INT      NOT NULL,
    [CertificateID]  INT      NULL,
    [IsDeleted]      BIT      CONSTRAINT [DF_SupplierCertificate_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME CONSTRAINT [DF_SupplierCertificate_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    CONSTRAINT [PK_SupplierCertificate] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierCertificate_Certificate] FOREIGN KEY ([CertificateID]) REFERENCES [dbo].[Certificate] ([ID]),
    CONSTRAINT [FK_SupplierCertificate_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID])
);





