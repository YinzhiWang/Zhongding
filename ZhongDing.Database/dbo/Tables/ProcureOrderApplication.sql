CREATE TABLE [dbo].[ProcureOrderApplication] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [OrderCode]        NVARCHAR (50) NULL,
    [SupplierID]       INT           NULL,
    [EstDeliveryDate]  DATETIME      NULL,
    [IsStop]           BIT           NULL,
    [StoppedOn]        DATETIME      NULL,
    [StoppedBy]        INT           NULL,
    [WorkflowStatusID] INT           NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_ProcureOrderApplication_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]        DATETIME      CONSTRAINT [DF_ProcureOrderApplication_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT           NULL,
    [LastModifiedOn]   DATETIME      NULL,
    [LastModifiedBy]   INT           NULL,
    CONSTRAINT [PK_ClientOrderApplication] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProcureOrderApplication_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID])
);

