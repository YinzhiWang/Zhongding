CREATE TABLE [dbo].[ClientInfoProductSetting] (
    [ID]                     INT        IDENTITY (1, 1) NOT NULL,
    [ClientInfoID]           INT        NOT NULL,
    [ProductID]              INT        NOT NULL,
    [ProductSpecificationID] INT        NOT NULL,
    [HighPrice]              FLOAT (53) NULL,
    [BasicPrice]             FLOAT (53) NULL,
    [UseFlowData]            BIT        NULL,
    [DepartmentID]           INT        NULL,
    [DeptDistrictID]         INT        NULL,
    [MonthlyTask]            INT        NULL,
    [RefundPrice]            FLOAT (53) NULL,
    [IsDeleted]              BIT        CONSTRAINT [DF_ClientInfoProductSetting_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]              DATETIME   CONSTRAINT [DF_ClientInfoProductSetting_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT        NULL,
    [LastModifiedOn]         DATETIME   NULL,
    [LastModifiedBy]         INT        NULL,
    CONSTRAINT [PK_ClientInfoProductSetting] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientInfoProductSetting_ClientInfo] FOREIGN KEY ([ClientInfoID]) REFERENCES [dbo].[ClientInfo] ([ID]),
    CONSTRAINT [FK_ClientInfoProductSetting_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([ID]),
    CONSTRAINT [FK_ClientInfoProductSetting_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_ClientInfoProductSetting_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID])
);

