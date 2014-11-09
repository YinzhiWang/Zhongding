CREATE TABLE [dbo].[SupplierTaskAssignment] (
    [ID]             INT      IDENTITY (1, 1) NOT NULL,
    [SupplierID]     INT      NULL,
    [ContractID]     INT      NULL,
    [YearOfTask]     INT      NULL,
    [MonthOfTask]    INT      NULL,
    [Quantity]       INT      NULL,
    [IsDeleted]      BIT      CONSTRAINT [DF_SupplierTaskAssignment_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]      DATETIME CONSTRAINT [DF_SupplierTaskAssignment_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT      NULL,
    [LastModifiedOn] DATETIME NULL,
    [LastModifiedBy] INT      NULL,
    CONSTRAINT [PK_SupplierTaskAssignment] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierTaskAssignment_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID]),
    CONSTRAINT [FK_SupplierTaskAssignment_SupplierContract] FOREIGN KEY ([ContractID]) REFERENCES [dbo].[SupplierContract] ([ID])
);



