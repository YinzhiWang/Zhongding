CREATE TABLE [dbo].[DBContractTaskAssignment] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [Month]          INT      NULL,
    [Numbers]        INT      NULL,
    [IsDeleted]      BIT      CONSTRAINT [DF_DBContractTaskAssignment_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME CONSTRAINT [DF_DBContractTaskAssignment_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    CONSTRAINT [PK_DBContractTaskAssignment] PRIMARY KEY CLUSTERED ([ID] ASC)
);



