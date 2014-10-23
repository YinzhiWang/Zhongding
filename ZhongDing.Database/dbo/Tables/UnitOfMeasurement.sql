CREATE TABLE [dbo].[UnitOfMeasurement] (
    [ID]       INT           IDENTITY (1, 1) NOT NULL,
    [UnitName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_UnitOfMeasurement] PRIMARY KEY CLUSTERED ([ID] ASC)
);

