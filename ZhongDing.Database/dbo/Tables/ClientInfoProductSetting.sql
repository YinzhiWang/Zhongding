CREATE TABLE [dbo].[ClientInfoProductSetting] (
    [ID]                     INT      IDENTITY (1, 1) NOT NULL,
    [ClientInfoID]           INT      NOT NULL,
    [ProductID]              INT      NOT NULL,
    [ProductSpecificationID] INT      NOT NULL,
    [HighPrice]              MONEY    NULL,
    [BasicPrice]             MONEY    NULL,
    [UseFlowData]            BIT      CONSTRAINT [DF_ClientInfoProductSetting_UseFlowData] DEFAULT ((0)) NOT NULL,
    [DepartmentID]           INT      NULL,
    [DeptMarketID]           INT      NULL,
    [MonthlyTask]            INT      NULL,
    [RefundPrice]            MONEY    NULL,
    [IsDeleted]              BIT      CONSTRAINT [DF_ClientInfoProductSetting_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]              DATETIME CONSTRAINT [DF_ClientInfoProductSetting_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT      NULL,
    [LastModifiedOn]         DATETIME NULL,
    [LastModifiedBy]         INT      NULL,
    CONSTRAINT [PK_ClientInfoProductSetting] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientInfoProductSetting_ClientInfo] FOREIGN KEY ([ClientInfoID]) REFERENCES [dbo].[ClientInfo] ([ID]),
    CONSTRAINT [FK_ClientInfoProductSetting_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([ID]),
    CONSTRAINT [FK_ClientInfoProductSetting_DeptMarket] FOREIGN KEY ([DeptMarketID]) REFERENCES [dbo].[DeptMarket] ([ID]),
    CONSTRAINT [FK_ClientInfoProductSetting_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_ClientInfoProductSetting_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID])
);







