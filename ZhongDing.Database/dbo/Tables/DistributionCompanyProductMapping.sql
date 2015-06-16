CREATE TABLE [dbo].[DistributionCompanyProductMapping] (
    [ID]                                      INT            IDENTITY (1, 1) NOT NULL,
    [DistributionCompanyID]                   INT            NULL,
    [DistributionCompanyName]                 NVARCHAR (256) NOT NULL,
    [DistributionCompanyProductCode]          NVARCHAR (256) NOT NULL,
    [DistributionCompanyProductName]          NVARCHAR (256) NOT NULL,
    [DistributionCompanyProductSpecification] NVARCHAR (256) NOT NULL,
    [ProductCode]                             NVARCHAR (256) NOT NULL,
    [ProductName]                             NVARCHAR (256) NOT NULL,
    [ProductSpecificationName]                NVARCHAR (256) NOT NULL,
    [ProductID]                               INT            NULL,
    [ProductSpecificationID]                  INT            NULL,
    [IsDeleted]                               BIT            CONSTRAINT [DF__Distribut__IsDel__797DF6D1] DEFAULT ((0)) NOT NULL,
    [CreatedOn]                               DATETIME       CONSTRAINT [DF__Distribut__Creat__7A721B0A] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                               INT            NULL,
    [LastModifiedOn]                          DATETIME       NULL,
    [LastModifiedBy]                          INT            NULL,
    CONSTRAINT [PK__Distribu__3214EC271EBE6CCF] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DistributionCompanyProductMapping_DistributionCompanyID] FOREIGN KEY ([DistributionCompanyID]) REFERENCES [dbo].[DistributionCompany] ([ID]),
    CONSTRAINT [FK_DistributionCompanyProductMapping_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_DistributionCompanyProductMapping_ProductSpecificationID] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID])
);

