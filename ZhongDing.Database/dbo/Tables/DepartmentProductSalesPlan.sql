CREATE TABLE [dbo].[DepartmentProductSalesPlan] (
    [ID]                  INT        NOT NULL,
    [DepartmentID]        INT        IDENTITY (1, 1) NOT NULL,
    [ProductID]           INT        NOT NULL,
    [IsFixedOfInside]     BIT        NOT NULL,
    [FixedRatioOfInside]  FLOAT (53) NULL,
    [IsFixedOfOutside]    BIT        NOT NULL,
    [FixedRatioOfOutside] FLOAT (53) NULL,
    [IsDeleted]           BIT        NULL,
    [CreatedOn]           DATETIME   NULL,
    [CreatedBy]           INT        NULL,
    [LastModifiedOn]      DATETIME   NULL,
    [LastModifiedBy]      INT        NULL,
    CONSTRAINT [PK_DepartSalesPlan] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DepartmentProductSalesPlan_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([ID]),
    CONSTRAINT [FK_DepartmentProductSalesPlan_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID])
);



