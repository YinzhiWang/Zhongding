CREATE TABLE [dbo].[ProcureOrderApplicationImportData] (
    [ID]                                     INT            IDENTITY (1, 1) NOT NULL,
    [EstDeliveryDate]                        DATETIME       NOT NULL,
    [ProcureOrderApplicationImportFileLogID] INT            NOT NULL,
    [ProcureOrderApplicationID]              INT            NOT NULL,
    [OrderCode]                              NVARCHAR (50)  NOT NULL,
    [SupplierName]                           NVARCHAR (256) NOT NULL,
    [OrderDate]                              DATETIME       NOT NULL,
    [CreatedOn]                              DATETIME       NOT NULL,
    [CreatedBy]                              INT            NULL,
    CONSTRAINT [PK_ProcureOrderApplicationImportData] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProcureOrderApplicationImportData_ProcureOrderApplicationImportFileLogID] FOREIGN KEY ([ProcureOrderApplicationImportFileLogID]) REFERENCES [dbo].[ProcureOrderApplicationImportFileLog] ([ID])
);

