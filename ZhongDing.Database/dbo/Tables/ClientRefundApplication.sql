﻿CREATE TABLE [dbo].[ClientRefundApplication] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [ClientSaleAppID]  INT           NOT NULL,
    [CompanyID]        INT           NOT NULL,
    [ClientUserID]     INT           NOT NULL,
    [ClientCompanyID]  INT           NOT NULL,
    [SaleOrderTypeID]  INT           NOT NULL,
    [DeliveryModeID]   INT           NULL,
    [OrderCode]        NVARCHAR (50) NOT NULL,
    [OrderDate]        DATETIME      CONSTRAINT [DF_ClientRefundApplication_OrderDate] DEFAULT (getdate()) NOT NULL,
    [IsStop]           BIT           CONSTRAINT [DF_ClientRefundApplication_IsStop] DEFAULT ((0)) NOT NULL,
    [RefundAmount]     MONEY         NULL,
    [WorkflowStatusID] INT           NOT NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_ClientRefundApplication_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]        DATETIME      CONSTRAINT [DF_ClientRefundApplication_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT           NULL,
    [LastModifiedOn]   DATETIME      NULL,
    [LastModifiedBy]   INT           NULL,
    CONSTRAINT [PK_ClientRefundApplication] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientRefundApplication_ClientCompany] FOREIGN KEY ([ClientCompanyID]) REFERENCES [dbo].[ClientCompany] ([ID]),
    CONSTRAINT [FK_ClientRefundApplication_ClientSaleApplication] FOREIGN KEY ([ClientSaleAppID]) REFERENCES [dbo].[ClientSaleApplication] ([ID]),
    CONSTRAINT [FK_ClientRefundApplication_ClientUser] FOREIGN KEY ([ClientUserID]) REFERENCES [dbo].[ClientUser] ([ID]),
    CONSTRAINT [FK_ClientRefundApplication_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_ClientRefundApplication_SaleOrderType] FOREIGN KEY ([SaleOrderTypeID]) REFERENCES [dbo].[SaleOrderType] ([ID]),
    CONSTRAINT [FK_ClientRefundApplication_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);
