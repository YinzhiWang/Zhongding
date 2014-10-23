CREATE TABLE [dbo].[Users] (
    [UserID]         INT              IDENTITY (1, 1) NOT NULL,
    [AspnetUserID]   UNIQUEIDENTIFIER NULL,
    [UserName]       NVARCHAR (256)   NULL,
    [FullName]       NVARCHAR (50)    NULL,
    [DepartmentID]   INT              NULL,
    [Position]       NVARCHAR (50)    NULL,
    [MobilePhone]    VARCHAR (50)     NULL,
    [EnrollDate]     DATETIME         NULL,
    [BasicSalary]    MONEY            NULL,
    [PositionSalary] MONEY            NULL,
    [PhoneAllowance] MONEY            NULL,
    [MealAllowance]  MONEY            NULL,
    [OfficeExpense]  MONEY            NULL,
    [BonusPay]       MONEY            NULL,
    [IsDeleted]      BIT              CONSTRAINT [DF_Users_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedOn]      DATETIME         NULL,
    [CreatedOn]      DATETIME         CONSTRAINT [DF_Users_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT              NULL,
    [LastModifiedOn] DATETIME         NULL,
    [LastModifiedBy] INT              NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [FK_Users_aspnet_Users] FOREIGN KEY ([AspnetUserID]) REFERENCES [dbo].[aspnet_Users] ([UserId])
);



