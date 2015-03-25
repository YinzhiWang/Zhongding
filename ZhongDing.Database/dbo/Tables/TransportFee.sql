CREATE TABLE [dbo].[TransportFee] (
    [ID]                     INT             IDENTITY (1, 1) NOT NULL,
    [TransportFeeType]       INT             NOT NULL,
    [TransportCompanyID]     INT             NOT NULL,
    [TransportCompanyNumber] NVARCHAR (100)  NULL,
    [Driver]                 NVARCHAR (50)   NULL,
    [DriverTelephone]        NVARCHAR (50)   NULL,
    [StartPlace]             NVARCHAR (100)  NULL,
    [StartPlaceTelephone]    NVARCHAR (100)  NULL,
    [EndPlace]               NVARCHAR (100)  NULL,
    [EndPlaceTelephone]      NVARCHAR (100)  NULL,
    [Fee]                    DECIMAL (18, 8) NOT NULL,
    [SendDate]               DATETIME        NOT NULL,
    [Remark]                 NVARCHAR (1000) NULL,
    [IsDeleted]              BIT             NOT NULL,
    [CreatedOn]              DATETIME        NOT NULL,
    [CreatedBy]              INT             NULL,
    [LastModifiedOn]         DATETIME        NULL,
    [LastModifiedBy]         INT             NULL,
    CONSTRAINT [PK__Transpor__3214EC2719B47C73] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TransportFee_TransportCompanyID] FOREIGN KEY ([TransportCompanyID]) REFERENCES [dbo].[TransportCompany] ([ID])
);

