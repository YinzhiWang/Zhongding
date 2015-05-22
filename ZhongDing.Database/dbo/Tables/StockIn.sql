CREATE TABLE [dbo].[StockIn] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [Code]             NVARCHAR (50) NULL,
    [EntryDate]        DATETIME      NULL,
    [SupplierID]       INT           NOT NULL,
    [WorkflowStatusID] INT           NOT NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_StockIn_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]        DATETIME      CONSTRAINT [DF_StockIn_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT           NULL,
    [LastModifiedOn]   DATETIME      NULL,
    [LastModifiedBy]   INT           NULL,
    [IsImport]         BIT           CONSTRAINT [DF_StockIn_IsImport] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_StockIn] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_StockIn_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID]),
    CONSTRAINT [FK_StockIn_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);



