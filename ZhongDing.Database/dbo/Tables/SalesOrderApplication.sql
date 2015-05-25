CREATE TABLE [dbo].[SalesOrderApplication] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [SaleOrderTypeID] INT           NOT NULL,
    [OrderCode]       NVARCHAR (50) NOT NULL,
    [OrderDate]       DATETIME      CONSTRAINT [DF_SalesOrderApplication_OrderDate] DEFAULT (getdate()) NOT NULL,
    [IsStop]          BIT           CONSTRAINT [DF_SalesOrderApplication_IsStop] DEFAULT ((0)) NOT NULL,
    [StoppedOn]       DATETIME      NULL,
    [StoppedBy]       INT           NULL,
    [IsDeleted]       BIT           CONSTRAINT [DF_SalesOrderApplication_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]       DATETIME      CONSTRAINT [DF_SalesOrderApplication_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       INT           NULL,
    [LastModifiedOn]  DATETIME      NULL,
    [LastModifiedBy]  INT           NULL,
    [IsImport]        BIT           CONSTRAINT [DF_SalesOrderApplication_IsImport] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_SalesOrderApplication] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SalesOrderApplication_SaleOrderType] FOREIGN KEY ([SaleOrderTypeID]) REFERENCES [dbo].[SaleOrderType] ([ID])
);









