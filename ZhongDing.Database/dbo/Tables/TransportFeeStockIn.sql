CREATE TABLE [dbo].[TransportFeeStockIn] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [TransportFeeID] INT      NOT NULL,
    [StockInID]      INT      NOT NULL,
    [IsDeleted]      BIT      NOT NULL,
    [CreatedOn]      DATETIME NOT NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

