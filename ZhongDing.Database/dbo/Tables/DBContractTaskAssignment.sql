CREATE TABLE [dbo].[DBContractTaskAssignment] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [Month]          INT      NULL,
    [Numbers]        INT      NULL,
    [IsDeleted]      BIT      NULL,
    [CreatedOn]      DATETIME NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    CONSTRAINT [PK_DBContractTaskAssignment] PRIMARY KEY CLUSTERED ([ID] ASC)
);

