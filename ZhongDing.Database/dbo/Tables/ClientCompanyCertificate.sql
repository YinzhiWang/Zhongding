CREATE TABLE [dbo].[ClientCompanyCertificate] (
    [ID]              INT      IDENTITY (1, 1) NOT NULL,
    [ClientCompanyID] INT      NOT NULL,
    [CertificateID]   INT      NULL,
    [IsDeleted]       BIT      CONSTRAINT [DF_ClientCompanyCertificate_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]       DATETIME CONSTRAINT [DF_ClientCompanyCertificate_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       INT      NULL,
    [LastModifiedOn]  DATETIME NULL,
    [LastModifiedBy]  INT      NULL,
    CONSTRAINT [PK_ClientCompanyCertificate] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientCompanyCertificate_Certificate] FOREIGN KEY ([CertificateID]) REFERENCES [dbo].[Certificate] ([ID]),
    CONSTRAINT [FK_ClientCompanyCertificate_ClientCompany] FOREIGN KEY ([ClientCompanyID]) REFERENCES [dbo].[ClientCompany] ([ID])
);

