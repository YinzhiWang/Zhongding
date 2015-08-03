CREATE TABLE [dbo].[ReimbursementDetail] (
    [ID]                       INT             IDENTITY (1, 1) NOT NULL,
    [ReimbursementID]          INT             NOT NULL,
    [ReimbursementTypeID]      INT             NOT NULL,
    [ReimbursementTypeChildID] INT             NULL,
    [StartDate]                DATETIME        NULL,
    [EndDate]                  DATETIME        NULL,
    [Amount]                   MONEY           NOT NULL,
    [Quantity]                 INT             NULL,
    [Comment]                  NVARCHAR (1024) NULL,
    [IsDeleted]                BIT             CONSTRAINT [DF_ReimbursementDetail_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn]                DATETIME        NOT NULL,
    [CreatedBy]                INT             NULL,
    [LastModifiedOn]           DATETIME        NULL,
    [LastModifiedBy]           INT             NULL,
    CONSTRAINT [PK__Reimburs__3214EC27CE253E7D] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ReimbursementDetail_ReimbursementID] FOREIGN KEY ([ReimbursementID]) REFERENCES [dbo].[Reimbursement] ([ID]),
    CONSTRAINT [FK_ReimbursementDetail_ReimbursementTypeChildID] FOREIGN KEY ([ReimbursementTypeChildID]) REFERENCES [dbo].[ReimbursementType] ([ID]),
    CONSTRAINT [FK_ReimbursementDetail_ReimbursementTypeID] FOREIGN KEY ([ReimbursementTypeID]) REFERENCES [dbo].[ReimbursementType] ([ID])
);



