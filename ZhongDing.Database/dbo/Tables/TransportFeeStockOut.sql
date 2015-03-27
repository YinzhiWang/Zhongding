CREATE TABLE [dbo].[TransportFeeStockOut] (
    [ID]                                INT      IDENTITY (1, 1) NOT NULL,
    [TransportFeeID]                    INT      NOT NULL,
    [StockOutID]                        INT      NOT NULL,
    [IsDeleted]                         BIT      NOT NULL,
    [CreatedOn]                         DATETIME NOT NULL,
    [CreatedBy]                         INT      NULL,
    [LastModifiedOn]                    DATETIME NULL,
    [LastModifiedBy]                    INT      NULL,
    [TransportFeeStockOutSmsReminderID] INT      NULL,
    CONSTRAINT [PK__Transpor__3214EC270FEBC4A8] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TransportFeeStockOut_StockOutID] FOREIGN KEY ([StockOutID]) REFERENCES [dbo].[StockOut] ([ID]),
    CONSTRAINT [FK_TransportFeeStockOut_TransportFeeID] FOREIGN KEY ([TransportFeeID]) REFERENCES [dbo].[TransportFee] ([ID]),
    CONSTRAINT [FK_TransportFeeStockOut_TransportFeeStockOutSmsReminderID] FOREIGN KEY ([TransportFeeStockOutSmsReminderID]) REFERENCES [dbo].[TransportFeeStockOutSmsReminder] ([ID])
);



