CREATE TABLE [dbo].[ReimbursementDetailTransportFee] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [ReimbursementDetailID] INT            NOT NULL,
    [TransportFeeID]        INT            NOT NULL,
    [Amount]                MONEY          NOT NULL,
    [Comment]               NVARCHAR (256) NULL,
    [IsDeleted]             BIT            DEFAULT ((0)) NOT NULL,
    [CreatedOn]             DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedBy]             INT            NULL,
    [LastModifiedOn]        DATETIME       NULL,
    [LastModifiedBy]        INT            NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ReimbursementDetailTransportFee_ReimbursementDetailID] FOREIGN KEY ([ReimbursementDetailID]) REFERENCES [dbo].[ReimbursementDetail] ([ID]),
    CONSTRAINT [FK_ReimbursementDetailTransportFee_TransportFeeID] FOREIGN KEY ([TransportFeeID]) REFERENCES [dbo].[TransportFee] ([ID])
);



