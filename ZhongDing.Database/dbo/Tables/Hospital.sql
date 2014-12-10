CREATE TABLE [dbo].[Hospital] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [HospitalName]   NVARCHAR (50) NULL,
    [IsDeleted]      BIT           CONSTRAINT [DF_Hospital_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME      CONSTRAINT [DF_Hospital_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT           NULL,
    [LastModifiedOn] DATETIME      NULL,
    [LastModifiedBy] INT           NULL,
    CONSTRAINT [PK_Hospital] PRIMARY KEY CLUSTERED ([ID] ASC)
);





