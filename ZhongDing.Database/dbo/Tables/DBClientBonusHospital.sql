CREATE TABLE [dbo].[DBClientBonusHospital] (
    [ID]              INT      IDENTITY (1, 1) NOT NULL,
    [DBClientBonusID] INT      NOT NULL,
    [HospitalID]      INT      NOT NULL,
    [CreatedOn]       DATETIME CONSTRAINT [DF_DBClientBonusHospital_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       INT      NULL,
    CONSTRAINT [PK_DBClientBonusHospital] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DBClientBonusHospital_DBClientBonus] FOREIGN KEY ([DBClientBonusID]) REFERENCES [dbo].[DBClientBonus] ([ID]),
    CONSTRAINT [FK_DBClientBonusHospital_Hospital] FOREIGN KEY ([HospitalID]) REFERENCES [dbo].[Hospital] ([ID])
);

