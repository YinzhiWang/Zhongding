CREATE TABLE [dbo].[HospitalType] (
    [ID]       INT           IDENTITY (1, 1) NOT NULL,
    [TypeName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_HospitalType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

