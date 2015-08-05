CREATE TABLE [dbo].[DBContractHospital] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [DBContractID]   INT      NOT NULL,
    [HospitalCodeID] INT      NOT NULL,
    [IsDeleted]      BIT      CONSTRAINT [DF_DBContractHospital_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME CONSTRAINT [DF_DBContractHospital_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    CONSTRAINT [PK_DBContractHospital] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DBContractHospital_DBContract] FOREIGN KEY ([DBContractID]) REFERENCES [dbo].[DBContract] ([ID]),
    CONSTRAINT [FK_DBContractHospital_HospitalCodeID] FOREIGN KEY ([HospitalCodeID]) REFERENCES [dbo].[HospitalCode] ([ID])
);



