CREATE TABLE [dbo].[DepartmentProductSalesPlan] (
    [ID]                  INT        IDENTITY (1, 1) NOT NULL,
    [DepartmentID]        INT        NOT NULL,
    [ProductID]           INT        NOT NULL,
    [IsFixedOfInside]     BIT        NOT NULL,
    [FixedRatioOfInside]  FLOAT (53) NULL,
    [IsFixedOfOutside]    BIT        NOT NULL,
    [FixedRatioOfOutside] FLOAT (53) NULL,
    [IsDeleted]           BIT        CONSTRAINT [DF_DepartmentProductSalesPlan_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]           DATETIME   CONSTRAINT [DF_DepartmentProductSalesPlan_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]           INT        NULL,
    [LastModifiedOn]      DATETIME   NULL,
    [LastModifiedBy]      INT        NULL,
    CONSTRAINT [PK_DepartSalesPlan] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DepartmentProductSalesPlan_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([ID]),
    CONSTRAINT [FK_DepartmentProductSalesPlan_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID])
);







