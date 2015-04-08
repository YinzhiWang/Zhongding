CREATE TABLE [dbo].[DBContract] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [ContractCode]           NVARCHAR (50)  NULL,
    [ClientUserID]           INT            NULL,
    [IsTempContract]         BIT            NULL,
    [DepartmentID]           INT            NULL,
    [InChargeUserID]         INT            NULL,
    [ProductID]              INT            NULL,
    [ProductSpecificationID] INT            NULL,
    [PromotionExpense]       MONEY          NULL,
    [ContractExpDate]        DATETIME       NULL,
    [IsNew]                  BIT            NULL,
    [HospitalTypeID]         INT            NULL,
    [Comment]                NVARCHAR (255) NULL,
    [IsDeleted]              BIT            CONSTRAINT [DF_DBContract_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]              DATETIME       CONSTRAINT [DF_DBContract_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT            NULL,
    [LastModifiedOn]         DATETIME       NULL,
    [LastModifiedBy]         INT            NULL,
    CONSTRAINT [PK_DBContract] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_DBContract_ClientUser] FOREIGN KEY ([ClientUserID]) REFERENCES [dbo].[ClientUser] ([ID]),
    CONSTRAINT [FK_DBContract_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([ID]),
    CONSTRAINT [FK_DBContract_HospitalType] FOREIGN KEY ([HospitalTypeID]) REFERENCES [dbo].[HospitalType] ([ID]),
    CONSTRAINT [FK_DBContract_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_DBContract_ProductSpecification] FOREIGN KEY ([ProductSpecificationID]) REFERENCES [dbo].[ProductSpecification] ([ID]),
    CONSTRAINT [FK_DBContract_Users] FOREIGN KEY ([InChargeUserID]) REFERENCES [dbo].[Users] ([UserID])
);









