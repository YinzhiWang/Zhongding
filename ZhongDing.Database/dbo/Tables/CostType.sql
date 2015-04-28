CREATE TABLE [dbo].[CostType] (
    [ID]       INT           IDENTITY (1, 1) NOT NULL,
    [TypeName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_CostType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

