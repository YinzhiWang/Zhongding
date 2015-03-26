CREATE TABLE [dbo].[TransportFeeStockOut] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [TransportFeeID] INT      NOT NULL,
    [StockOutID]     INT      NOT NULL,
    [IsDeleted]      BIT      NOT NULL,
    [CreatedOn]      DATETIME NOT NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

