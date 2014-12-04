CREATE TABLE [dbo].[PaymentStatus] (
    [ID]                INT           IDENTITY (1, 1) NOT NULL,
    [PaymentStatusName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_PaymentStatus] PRIMARY KEY CLUSTERED ([ID] ASC)
);

