CREATE TABLE [dbo].[ClientOrderApplication] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [OrderCode]        NVARCHAR (50) NULL,
    [SupplierID]       INT           NULL,
    [EstDeliveryDate]  DATETIME      NULL,
    [IsStop]           BIT           NULL,
    [StoppedOn]        DATETIME      NULL,
    [StoppedBy]        INT           NULL,
    [WorkflowStatusID] INT           NULL,
    [IsDeleted]        BIT           NULL,
    [CreatedOn]        DATETIME      NULL,
    [CreatedBy]        INT           NULL,
    [LastModifiedOn]   DATETIME      NULL,
    [LastModifiedBy]   INT           NULL,
    CONSTRAINT [PK_ClientOrderApplication] PRIMARY KEY CLUSTERED ([ID] ASC)
);

