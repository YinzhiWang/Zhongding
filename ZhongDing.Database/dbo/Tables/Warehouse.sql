CREATE TABLE [dbo].[Warehouse] (
    [ID]             INT             NOT NULL,
    [CompanyID]      INT             NOT NULL,
    [Name]           NVARCHAR (255)  NULL,
    [Address]        NVARCHAR (255)  NULL,
    [Comment]        NVARCHAR (1000) NULL,
    [SaleTypeID]     INT             NULL,
    [IsDeleted]      NCHAR (10)      NULL,
    [CreatedOn]      DATETIME        NULL,
    [CreatedBy]      INT             NULL,
    [LastModifiedOn] NCHAR (10)      NULL,
    [LastModifiedBy] INT             NULL,
    CONSTRAINT [PK_Warehouse] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Warehouse_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_Warehouse_SaleType] FOREIGN KEY ([SaleTypeID]) REFERENCES [dbo].[SaleType] ([ID]),
    CONSTRAINT [FK_Warehouse_Warehouse] FOREIGN KEY ([ID]) REFERENCES [dbo].[Warehouse] ([ID])
);

