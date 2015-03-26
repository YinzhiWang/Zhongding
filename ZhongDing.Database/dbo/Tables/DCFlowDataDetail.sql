CREATE TABLE [dbo].[DCFlowDataDetail] (
    [ID]                   INT             IDENTITY (1, 1) NOT NULL,
    [DCFlowDataID]         INT             NOT NULL,
    [DBContractID]         INT             NOT NULL,
    [ContractCode]         NVARCHAR (50)   NULL,
    [IsTempContract]       BIT             NULL,
    [HospitalID]           INT             NOT NULL,
    [HospitalName]         NVARCHAR (50)   NULL,
    [ClientUserID]         INT             NOT NULL,
    [ClientUserName]       NVARCHAR (50)   NULL,
    [InChargeUserID]       INT             NULL,
    [InChargeUserFullName] NVARCHAR (50)   NULL,
    [UnitOfMeasurementID]  INT             NULL,
    [UnitName]             NVARCHAR (50)   NULL,
    [SaleDate]             DATETIME        NOT NULL,
    [SaleQty]              INT             NOT NULL,
    [Comment]              NVARCHAR (1000) NULL,
    [IsDeleted]            BIT             CONSTRAINT [DF_DCFlowDataDetail_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]            DATETIME        CONSTRAINT [DF_DCFlowDataDetail_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]            INT             NULL,
    [LastModifiedOn]       DATETIME        NULL,
    [LastModifiedBy]       INT             NULL,
    CONSTRAINT [PK_DCFlowDataDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DCFlowDataDetail_DBContract] FOREIGN KEY ([DBContractID]) REFERENCES [dbo].[DBContract] ([ID]),
    CONSTRAINT [FK_DCFlowDataDetail_DCFlowData] FOREIGN KEY ([DCFlowDataID]) REFERENCES [dbo].[DCFlowData] ([ID]),
    CONSTRAINT [FK_DCFlowDataDetail_Hospital] FOREIGN KEY ([HospitalID]) REFERENCES [dbo].[Hospital] ([ID])
);



