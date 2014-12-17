CREATE TABLE [dbo].[SalesOrderApplication] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [SaleOrderTypeID] INT           NOT NULL,
    [OrderCode]       NVARCHAR (50) NULL,
    [OrderDate]       DATETIME      NULL,
    [IsStop]          BIT           NULL,
    [StoppedOn]       DATETIME      NULL,
    [StoppedBy]       INT           NULL,
    [IsDeleted]       BIT           NULL,
    [CreatedOn]       DATETIME      NULL,
    [CreatedBy]       INT           NULL,
    [LastModifiedOn]  DATETIME      NULL,
    [LastModifiedBy]  INT           NULL,
    CONSTRAINT [PK_SalesOrderApplication] PRIMARY KEY CLUSTERED ([ID] ASC)
);

