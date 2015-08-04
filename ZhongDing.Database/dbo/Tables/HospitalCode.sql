CREATE TABLE [dbo].[HospitalCode] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [Code]           NVARCHAR (50)  NOT NULL,
    [Comment]        NVARCHAR (200) NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF__HospitalN__IsDel__7E97B1A9] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME       CONSTRAINT [DF__HospitalN__Creat__7F8BD5E2] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT            NULL,
    [LastModifiedOn] DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    CONSTRAINT [PK__Hospital__3214EC273BA567B5] PRIMARY KEY CLUSTERED ([ID] ASC)
);

