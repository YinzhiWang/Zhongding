CREATE TABLE [dbo].[DBClientSettlement] (
    [ID]                    INT             IDENTITY (1, 1) NOT NULL,
    [SettlementDate]        DATETIME        NOT NULL,
    [HospitalTypeID]        INT             NOT NULL,
    [WorkflowStatusID]      INT             NOT NULL,
    [SettlementOperateDate] DATETIME        NULL,
    [Comment]               NVARCHAR (1000) NULL,
    [IsDeleted]             BIT             CONSTRAINT [DF_DBClientSettlement_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]             DATETIME        CONSTRAINT [DF_DBClientSettlement_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]             INT             NULL,
    [LastModifiedOn]        DATETIME        NULL,
    [LastModifiedBy]        INT             NULL,
    CONSTRAINT [PK_DBClientSettlement] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DBClientSettlement_HospitalType] FOREIGN KEY ([HospitalTypeID]) REFERENCES [dbo].[HospitalType] ([ID]),
    CONSTRAINT [FK_DBClientSettlement_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);

