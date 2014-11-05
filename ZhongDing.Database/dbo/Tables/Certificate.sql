CREATE TABLE [dbo].[Certificate] (
    [ID]                INT            IDENTITY (1, 1) NOT NULL,
    [CertificateTypeID] INT            NULL,
    [OwnerTypeID]       INT            NULL,
    [IsGotten]          BIT            NULL,
    [EffectiveFrom]     DATETIME       NULL,
    [EffectiveTo]       DATETIME       NULL,
    [IsNeedAlert]       BIT            NULL,
    [AlertBeforeDays]   INT            NULL,
    [Comment]           NVARCHAR (MAX) NULL,
    [IsDeleted]         BIT            CONSTRAINT [DF_SupplierCertificateFile_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedOn]         DATETIME       NULL,
    [CreatedOn]         DATETIME       CONSTRAINT [DF_SupplierCertificateFile_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]         INT            NULL,
    [LastModifiedOn]    DATETIME       NULL,
    [LastModifiedBy]    INT            NULL,
    CONSTRAINT [PK_CertificateFile] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Certificate_CertificateType] FOREIGN KEY ([CertificateTypeID]) REFERENCES [dbo].[CertificateType] ([ID]),
    CONSTRAINT [FK_CertificateFile_OwnerType] FOREIGN KEY ([OwnerTypeID]) REFERENCES [dbo].[OwnerType] ([ID])
);

