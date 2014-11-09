CREATE TABLE [dbo].[SupplierContractFile] (
    [ID]             INT             IDENTITY (1, 1) NOT NULL,
    [ContractID]     INT             NULL,
    [FileName]       NVARCHAR (255)  NULL,
    [FilePath]       NVARCHAR (512)  NULL,
    [Comment]        NVARCHAR (1000) NULL,
    [IsDeleted]      BIT             CONSTRAINT [DF_SupplierContractFile_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME        CONSTRAINT [DF_SupplierContractFile_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT             NULL,
    [LastModifiedOn] DATETIME        NULL,
    [LastModifiedBy] INT             NULL,
    CONSTRAINT [PK_SupplierContractFile] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierContractFile_SupplierContract] FOREIGN KEY ([ContractID]) REFERENCES [dbo].[SupplierContract] ([ID])
);





