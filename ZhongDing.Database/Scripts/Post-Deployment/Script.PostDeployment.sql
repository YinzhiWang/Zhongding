
---- Start--- 10/25/2014 -- 初始化membership datas--by lihong
set xact_abort on
go

begin transaction
go

--初始化aspnet_SchemaVersions数据
INSERT INTO dbo.aspnet_SchemaVersions
  VALUES (N'common', N'1', 1)
INSERT INTO dbo.aspnet_SchemaVersions
  VALUES (N'health monitoring', N'1', 1)
INSERT INTO dbo.aspnet_SchemaVersions
  VALUES (N'membership', N'1', 1)
INSERT INTO dbo.aspnet_SchemaVersions
  VALUES (N'personalization', N'1', 1)
INSERT INTO dbo.aspnet_SchemaVersions
  VALUES (N'profile', N'1', 1)
INSERT INTO dbo.aspnet_SchemaVersions
  VALUES (N'role manager', N'1', 1)

--初始化aspnet_Applications数据
INSERT INTO dbo.aspnet_Applications
  VALUES (N'ZhongDing', N'ZhongDing', N'{F0B20AB9-EC04-4C5C-9806-CA8981D01EBF}', N'众鼎医药咨询信息系统')

INSERT INTO dbo.aspnet_Users
  VALUES (N'{F0B20AB9-EC04-4C5C-9806-CA8981D01EBF}', N'{26C88AB6-B52C-4CA8-A781-FBAE293F3E93}', N'SystemAdmin', N'systemadmin', NULL, 0, GETDATE())

INSERT INTO dbo.aspnet_Membership
  (ApplicationId, UserId, Password, PasswordFormat, PasswordSalt, MobilePIN, Email, LoweredEmail, PasswordQuestion, PasswordAnswer, IsApproved, IsLockedOut, CreateDate, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart)
  VALUES (N'{F0B20AB9-EC04-4C5C-9806-CA8981D01EBF}', N'{26C88AB6-B52C-4CA8-A781-FBAE293F3E93}', N'e9qc5qPNvVwbpPyxP1gjDfj6twY=', 1, N'bMBzkD3p1bW//ZJRrLE27Q==', NULL, N'admin@zhongding.com', N'admin@zhongding.com', NULL, NULL, 1, 0, GETDATE(), GETDATE(), GETDATE(), '1754-01-01 00:00:00.000', 0, '1754-01-01 00:00:00.000', 0, '1754-01-01 00:00:00.000')

-- 初始化admin user数据
set identity_insert dbo.Users on

INSERT INTO dbo.Users
  (UserID, AspnetUserID, UserName, FullName)
  VALUES (1, N'{26C88AB6-B52C-4CA8-A781-FBAE293F3E93}', N'SystemAdmin', N'System Admin')

set identity_insert dbo.Users off

--初始化aspnet_Roles数据
INSERT INTO dbo.aspnet_Roles
  VALUES (N'{F0B20AB9-EC04-4C5C-9806-CA8981D01EBF}', N'{7DF96D93-77FB-4111-997E-B84478AE7DA3}', N'管理员', N'管理员', N'系统管理员角色')


--初始化Roles数据
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleID], [AspnetRoleID], [RoleName], [IsSystemDefault], [IsDeleted]) 
	VALUES (1, N'7df96d93-77fb-4111-997e-b84478ae7da3', N'管理员', 1, 0)

SET IDENTITY_INSERT [dbo].[Roles] OFF

--初始化aspnet_UsersInRoles数据
INSERT INTO dbo.aspnet_UsersInRoles
  VALUES (N'{26C88AB6-B52C-4CA8-A781-FBAE293F3E93}', N'{7DF96D93-77FB-4111-997E-B84478AE7DA3}')


go

commit transaction
go

----end--- 10/25/2014 -- 初始化membership datas -- by lihong 


---- start --- 10/29/2014 -- 初始化账套数据 -- by lihong 
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
SET IDENTITY_INSERT [dbo].[Company] ON
INSERT INTO [dbo].[Company] ([ID], [CompanyCode], [CompanyName], [CreatedOn], [CreatedBy]) VALUES (1, N'ZT001', N'英特康', GETDATE(), 1)
INSERT INTO [dbo].[Company] ([ID], [CompanyCode], [CompanyName], [CreatedOn], [CreatedBy]) VALUES (2, N'ZT002', N'万国康', GETDATE(), 1)
SET IDENTITY_INSERT [dbo].[Company] OFF
COMMIT TRANSACTION

GO

---- end ---- 10/29/2014 -- 初始化账套数据 -- by lihong 


---- start --- 10/31/2014 -- 初始化所有者类型和帐号类型数据 -- by lihong 
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
SET IDENTITY_INSERT [dbo].[OwnerType] ON
INSERT INTO [dbo].[OwnerType] ([ID], [OwnerTypeName]) VALUES (1, N'账套')
INSERT INTO [dbo].[OwnerType] ([ID], [OwnerTypeName]) VALUES (2, N'供应商')
INSERT INTO [dbo].[OwnerType] ([ID], [OwnerTypeName]) VALUES (3, N'客户')
INSERT INTO [dbo].[OwnerType] ([ID], [OwnerTypeName]) VALUES (4, N'生产企业')
INSERT INTO [dbo].[OwnerType] ([ID], [OwnerTypeName]) VALUES (5, N'货品')
SET IDENTITY_INSERT [dbo].[OwnerType] OFF

SET IDENTITY_INSERT [dbo].[AccountType] ON
INSERT INTO [dbo].[AccountType] ([ID], [AccountTypeName]) VALUES (1, N'公司')
INSERT INTO [dbo].[AccountType] ([ID], [AccountTypeName]) VALUES (2, N'私人')
SET IDENTITY_INSERT [dbo].[AccountType] OFF

COMMIT TRANSACTION

---- end ---- 10/31/2014 -- 初始化所有者类型和帐号类型数据 -- by lihong 

