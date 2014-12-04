CREATE TABLE [dbo].[PaymentType] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [PaymentTypeName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_PaymentTypeID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

