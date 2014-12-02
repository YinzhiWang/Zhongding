CREATE TABLE [dbo].[DepartmentProductSalesBonus] (
    [ID]                INT        IDENTITY (1, 1) NOT NULL,
    [SalesPlanID]       INT        NOT NULL,
    [SalesPlanTypeID]   INT        NOT NULL,
    [CompareOperatorID] INT        NOT NULL,
    [SalesPrice]        MONEY      NULL,
    [BonusRatio]        FLOAT (53) NULL,
    [IsDeleted]         BIT        NULL,
    [CreatedOn]         DATETIME   NULL,
    [CreatedBy]         INT        NULL,
    [LastModifiedOn]    DATETIME   NULL,
    [LastModifiedBy]    INT        NULL,
    CONSTRAINT [PK_DepartmentProductSalesBonus] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DepartmentProductSalesBonus_DepartmentProductSalesPlan] FOREIGN KEY ([SalesPlanID]) REFERENCES [dbo].[DepartmentProductSalesPlan] ([ID])
);



