﻿
---- Start--- 10/25/2014 -- by lihong 初始化membership datas
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
