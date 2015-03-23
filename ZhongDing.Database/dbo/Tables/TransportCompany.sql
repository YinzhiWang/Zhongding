CREATE TABLE [dbo].[TransportCompany] (
    [ID]              INT             IDENTITY (1, 1) NOT NULL,
    [CompanyName]     NVARCHAR (100)  NOT NULL,
    [Telephone]       NVARCHAR (50)   NOT NULL,
    [CompanyAddress]  NVARCHAR (100)  NOT NULL,
    [Driver]          NVARCHAR (50)   NULL,
    [DriverTelephone] NVARCHAR (50)   NULL,
    [Remark]          NVARCHAR (1000) NULL,
    [IsDeleted]       BIT             NOT NULL,
    [CreatedOn]       DATETIME        NOT NULL,
    [CreatedBy]       INT             NULL,
    [LastModifiedOn]  DATETIME        NULL,
    [LastModifiedBy]  INT             NULL,
    CONSTRAINT [PK_TransportCompany] PRIMARY KEY CLUSTERED ([ID] ASC)
);



