CREATE TABLE [dbo].[Warehouse] (
    [ID]             INT             IDENTITY (1, 1) NOT NULL,
    [CompanyID]      INT             NOT NULL,
    [WarehouseCode]  NVARCHAR (50)   NULL,
    [Name]           NVARCHAR (255)  NULL,
    [Address]        NVARCHAR (255)  NULL,
    [Comment]        NVARCHAR (1000) NULL,
    [SaleTypeID]     INT             NULL,
    [IsDeleted]      BIT             CONSTRAINT [DF_Warehouse_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME        CONSTRAINT [DF_Warehouse_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT             NULL,
    [LastModifiedOn] DATETIME        NULL,
    [LastModifiedBy] INT             NULL,
    CONSTRAINT [PK_Warehouse] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Warehouse_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_Warehouse_SaleType] FOREIGN KEY ([SaleTypeID]) REFERENCES [dbo].[SaleType] ([ID])
);





