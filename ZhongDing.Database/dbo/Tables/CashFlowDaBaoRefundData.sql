﻿CREATE TABLE [dbo].[CashFlowDaBaoRefundData] (
    [ID]                 INT      IDENTITY (1, 1) NOT NULL,
    [CashFlowBaseDataID] INT      NOT NULL,
    [CompanyID]          INT      NOT NULL,
    [Amount]             MONEY    NOT NULL,
    [IsDeleted]          BIT      DEFAULT ((0)) NOT NULL,
    [CreatedOn]          DATETIME DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          INT      NULL,
    [LastModifiedOn]     DATETIME NULL,
    [LastModifiedBy]     INT      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CashFlowDaBaoRefundData_CashFlowBaseDataID] FOREIGN KEY ([CashFlowBaseDataID]) REFERENCES [dbo].[CashFlowBaseData] ([ID]),
    CONSTRAINT [FK_CashFlowDaBaoRefundData_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID])
);

