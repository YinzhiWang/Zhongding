CREATE TABLE [dbo].[ProductSpecification] (
    [ID]                   INT           IDENTITY (1, 1) NOT NULL,
    [Specification]        NVARCHAR (50) NULL,
    [ProductID]            INT           NULL,
    [UnitOfMeasurementID]  INT           NULL,
    [NumberInSmallPackage] INT           NULL,
    [NumberInLargePackage] INT           NULL,
    [IsDeleted]            BIT           CONSTRAINT [DF_ProductSpecification_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedOn]            DATETIME      NULL,
    [CreatedOn]            DATETIME      CONSTRAINT [DF_ProductSpecification_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]            INT           NULL,
    [LastModifiedOn]       DATETIME      NULL,
    [LastModifiedBy]       INT           NULL,
    CONSTRAINT [PK_ProductSpecification] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProductSpecification_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_ProductSpecification_UnitOfMeasurement] FOREIGN KEY ([UnitOfMeasurementID]) REFERENCES [dbo].[UnitOfMeasurement] ([ID])
);

