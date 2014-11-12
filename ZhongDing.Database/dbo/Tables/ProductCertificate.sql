CREATE TABLE [dbo].[ProductCertificate] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [ProductID]      INT      NOT NULL,
    [CertificateID]  INT      NOT NULL,
    [IsDeleted]      BIT      CONSTRAINT [DF_ProductCertificate_IsDeleted1] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME CONSTRAINT [DF_ProductCertificate_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    CONSTRAINT [PK_ProductCertificate] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProductCertificate_Certificate] FOREIGN KEY ([CertificateID]) REFERENCES [dbo].[Certificate] ([ID]),
    CONSTRAINT [FK_ProductCertificate_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID])
);



