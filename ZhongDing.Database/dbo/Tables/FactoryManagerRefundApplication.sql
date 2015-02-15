﻿CREATE TABLE [dbo].[FactoryManagerRefundApplication] (
    [ID]                     INT      IDENTITY (1, 1) NOT NULL,
    [CompanyID]              INT      NOT NULL,
    [ClientUserID]           INT      NULL,
    [ProductID]              INT      NOT NULL,
    [ProductSpecificationID] INT      NOT NULL,
    [BeginDate]              DATETIME NOT NULL,
    [EndDate]                DATETIME NOT NULL,
    [StockInQty]             INT      NOT NULL,
    [StockOutQty]            INT      NULL,
    [RefundPrice]            MONEY    NOT NULL,
    [RefundAmount]           MONEY    NOT NULL,
    [WorkflowStatusID]       INT      NOT NULL,
    [IsDeleted]              BIT      CONSTRAINT [DF_FactoryManagerRefundApplication_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]              DATETIME CONSTRAINT [DF_FactoryManagerRefundApplication_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT      NULL,
    [LastModifiedOn]         DATETIME NULL,
    [LastModifiedBy]         INT      NULL,
    CONSTRAINT [PK_FactoryManagerRefundApplication] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FactoryManagerRefundApplication_ClientUser] FOREIGN KEY ([ClientUserID]) REFERENCES [dbo].[ClientUser] ([ID]),
    CONSTRAINT [FK_FactoryManagerRefundApplication_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_FactoryManagerRefundApplication_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_FactoryManagerRefundApplication_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID]),
    CONSTRAINT [FK_FactoryManagerRefundApplication_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);

