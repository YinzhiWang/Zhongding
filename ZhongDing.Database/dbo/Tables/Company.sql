CREATE TABLE [dbo].[Company] (
    [ID]                      INT             IDENTITY (1, 1) NOT NULL,
    [CompanyCode]             NVARCHAR (50)   NULL,
    [CompanyName]             NVARCHAR (100)  NULL,
    [ProviderTexRatio]        DECIMAL (18, 8) NULL,
    [ClientTaxHighRatio]      DECIMAL (18, 8) NULL,
    [ClientTaxLowRatio]       DECIMAL (18, 8) NULL,
    [EnableTaxDeduction]      BIT             NULL,
    [ClientTaxDeductionRatio] DECIMAL (18, 8) NULL,
    [IsDeleted]               BIT             CONSTRAINT [DF_Company_IsDeleted_1] DEFAULT ((0)) NOT NULL,
    [CreatedOn]               DATETIME        CONSTRAINT [DF_Company_CreatedOn_1] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]               INT             NULL,
    [LastModifiedOn]          DATETIME        NULL,
    [LastModifiedBy]          INT             NULL,
    CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([ID] ASC)
);



