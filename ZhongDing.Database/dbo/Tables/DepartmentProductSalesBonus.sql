CREATE TABLE [dbo].[DepartmentProductSalesBonus] (
    [ID]                INT        IDENTITY (1, 1) NOT NULL,
    [SalesPlanID]       INT        NOT NULL,
    [SalesPlanTypeID]   INT        NOT NULL,
    [CompareOperatorID] INT        NOT NULL,
    [SalesPrice]        MONEY      NULL,
    [BonusRatio]        FLOAT (53) NULL,
    [IsDeleted]         BIT        CONSTRAINT [DF_DepartmentProductSalesBonus_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]         DATETIME   CONSTRAINT [DF_DepartmentProductSalesBonus_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]         INT        NULL,
    [LastModifiedOn]    DATETIME   NULL,
    [LastModifiedBy]    INT        NULL,
    CONSTRAINT [PK_DepartmentProductSalesBonus] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DepartmentProductSalesBonus_DepartmentProductSalesPlan] FOREIGN KEY ([SalesPlanID]) REFERENCES [dbo].[DepartmentProductSalesPlan] ([ID])
);





