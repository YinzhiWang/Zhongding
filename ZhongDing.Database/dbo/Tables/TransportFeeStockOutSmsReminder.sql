CREATE TABLE [dbo].[TransportFeeStockOutSmsReminder] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [TransportFeeStockOutID] INT            NOT NULL,
    [Status]                 INT            NOT NULL,
    [MobileNumber]           NVARCHAR (100) NOT NULL,
    [Content]                NVARCHAR (256) NOT NULL,
    [IsDeleted]              BIT            NOT NULL,
    [CreatedOn]              DATETIME       NOT NULL,
    [CreatedBy]              INT            NULL,
    [LastModifiedOn]         DATETIME       NULL,
    [LastModifiedBy]         INT            NULL,
    CONSTRAINT [PK__StockOut__3214EC27BD492916] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TransportFeeStockOutSmsReminder_TransportFeeStockOutID] FOREIGN KEY ([TransportFeeStockOutID]) REFERENCES [dbo].[TransportFeeStockOut] ([ID])
);



