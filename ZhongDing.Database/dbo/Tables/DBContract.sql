CREATE TABLE [dbo].[DBContract] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [ContractCode]           NVARCHAR (50)  NULL,
    [ClientUserID]           INT            NULL,
    [IsTempContract]         BIT            NULL,
    [DepartmentID]           INT            NULL,
    [InChargeUserID]         INT            NULL,
    [ProductID]              INT            NULL,
    [ProductSpecificationID] INT            NULL,
    [PromotionExpense]       FLOAT (53)     NULL,
    [ContractExpDate]        DATETIME       NULL,
    [IsNew]                  BIT            NULL,
    [HospitalTypeID]         INT            NULL,
    [Comment]                NVARCHAR (255) NULL,
    [IsDeleted]              BIT            CONSTRAINT [DF_DBContract_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]              DATETIME       CONSTRAINT [DF_DBContract_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT            NULL,
    [LastModifiedOn]         DATETIME       NULL,
    [LastModifiedBy]         INT            NULL,
    CONSTRAINT [PK_DBContract] PRIMARY KEY CLUSTERED ([ID] ASC)
);



