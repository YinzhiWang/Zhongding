CREATE TABLE [dbo].[Hospital] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [DBContractID]   INT           NOT NULL,
    [HospitalName]   NVARCHAR (50) NULL,
    [IsDeleted]      BIT           NULL,
    [CreatedOn]      DATETIME      NULL,
    [CreatedBy]      INT           NULL,
    [LastModifiedOn] DATETIME      NULL,
    [LastModifiedBy] INT           NULL,
    CONSTRAINT [PK_Hospital] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Hospital_DBContract] FOREIGN KEY ([DBContractID]) REFERENCES [dbo].[DBContract] ([ID])
);

