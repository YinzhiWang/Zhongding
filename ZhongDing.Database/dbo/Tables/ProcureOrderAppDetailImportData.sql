CREATE TABLE [dbo].[ProcureOrderAppDetailImportData] (
    [ID]                                  INT            IDENTITY (1, 1) NOT NULL,
    [ProcureOrderApplicationImportDataID] INT            NOT NULL,
    [ProcureOrderAppDetailID]             INT            NOT NULL,
    [WarehouseName]                       NVARCHAR (256) NULL,
    [ProductName]                         NVARCHAR (256) NULL,
    [Specification]                       NVARCHAR (256) NULL,
    [ProcureCount]                        INT            NULL,
    [ProcurePrice]                        DECIMAL (18)   NULL,
    [TotalAmount]                         DECIMAL (18)   NULL,
    [CreatedOn]                           DATETIME       NOT NULL,
    [CreatedBy]                           INT            NULL,
    [UnitOfMeasurement]                   NVARCHAR (256) NULL,
    CONSTRAINT [PK_ProcureOrderAppDetailImportFileLog] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProcureOrderAppDetailImportData_ProcureOrderApplicationImportDataID] FOREIGN KEY ([ProcureOrderApplicationImportDataID]) REFERENCES [dbo].[ProcureOrderApplicationImportData] ([ID])
);

