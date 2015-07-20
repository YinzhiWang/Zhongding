CREATE TABLE [dbo].[CashFlowHistory] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [CashFlowDate]     DATETIME       NOT NULL,
    [CashFlowFileName] NVARCHAR (256) NOT NULL,
    [FilePath]         NVARCHAR (256) NOT NULL,
    [IsDeleted]        BIT            DEFAULT ((0)) NOT NULL,
    [CreatedOn]        DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT            NULL,
    [LastModifiedOn]   DATETIME       NULL,
    [LastModifiedBy]   INT            NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

