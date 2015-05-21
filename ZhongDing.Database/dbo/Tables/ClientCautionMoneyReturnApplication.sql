CREATE TABLE [dbo].[ClientCautionMoneyReturnApplication] (
    [ID]                   INT            IDENTITY (1, 1) NOT NULL,
    [ClientCautionMoneyID] INT            NOT NULL,
    [ApplyDate]            DATETIME       NOT NULL,
    [Amount]               MONEY          NOT NULL,
    [Reason]               NVARCHAR (256) NULL,
    [IsDeleted]            BIT            NOT NULL,
    [CreatedOn]            DATETIME       NOT NULL,
    [CreatedBy]            INT            NULL,
    [LastModifiedOn]       DATETIME       NULL,
    [LastModifiedBy]       INT            NULL,
    [WorkflowStatusID]     INT            NOT NULL,
    [PaidDate]             DATETIME       NULL,
    [PaidBy]               INT            NULL,
    [IsStop]               BIT            CONSTRAINT [DF_ClientCautionMoneyReturnApplication_IsStop] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__ClientCa__3214EC279CC58CA0] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientCautionMoneyReturnApplication_ClientCautionMoneyID] FOREIGN KEY ([ClientCautionMoneyID]) REFERENCES [dbo].[ClientCautionMoney] ([ID]),
    CONSTRAINT [FK_ClientCautionMoneyReturnApplication_WorkflowStatusID] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
);

