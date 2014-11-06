CREATE TABLE [dbo].[CertificateType] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [CertificateType] NVARCHAR (50) NULL,
    [OwnerTypeID]     INT           NULL,
    CONSTRAINT [PK_CertificateType] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CertificateType_OwnerType] FOREIGN KEY ([OwnerTypeID]) REFERENCES [dbo].[OwnerType] ([ID])
);



