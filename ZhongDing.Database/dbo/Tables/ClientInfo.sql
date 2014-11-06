CREATE TABLE [dbo].[ClientInfo] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [ClientUserID]    INT            NOT NULL,
    [ClientCompanyID] INT            NOT NULL,
    [ReceiverName]    NVARCHAR (50)  NULL,
    [PhoneNumber]     NVARCHAR (50)  NULL,
    [Fax]             NVARCHAR (50)  NULL,
    [ReceiverAddress] NVARCHAR (255) NULL,
    [ReceiptAddress]  NVARCHAR (255) NULL,
    [IsDeleted]       BIT            NULL,
    [CreatedOn]       DATETIME       NULL,
    [CreatedBy]       INT            NULL,
    [LastModifiedOn]  DATETIME       NULL,
    [LastModifiedBy]  INT            NULL,
    CONSTRAINT [PK_ClientInfo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ClientInfo_ClientCompany] FOREIGN KEY ([ClientCompanyID]) REFERENCES [dbo].[ClientCompany] ([ID]),
    CONSTRAINT [FK_ClientInfo_ClientUser] FOREIGN KEY ([ClientUserID]) REFERENCES [dbo].[ClientUser] ([ID])
);

