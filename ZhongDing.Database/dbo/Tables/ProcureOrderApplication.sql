CREATE TABLE [dbo].[ProcureOrderApplication] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [OrderCode]        NVARCHAR (50) NULL,
    [SupplierID]       INT           NOT NULL,
    [OrderDate]        DATETIME      CONSTRAINT [DF_ProcureOrderApplication_OrderDate] DEFAULT (getdate()) NOT NULL,
    [EstDeliveryDate]  DATETIME      NULL,
    [IsStop]           BIT           CONSTRAINT [DF_ProcureOrderApplication_IsStop] DEFAULT ((0)) NOT NULL,
    [StoppedOn]        DATETIME      NULL,
    [StoppedBy]        INT           NULL,
    [WorkflowStatusID] INT           NOT NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_ProcureOrderApplication_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]        DATETIME      CONSTRAINT [DF_ProcureOrderApplication_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT           NULL,
    [LastModifiedOn]   DATETIME      NULL,
    [LastModifiedBy]   INT           NULL,
    CONSTRAINT [PK_ClientOrderApplication] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProcureOrderApplication_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID]),
    CONSTRAINT [FK_ProcureOrderApplication_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);



