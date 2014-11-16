
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

---- start --- 11/06/2014 -- 初始化CertificateType, SaleType数据 -- by Yinzhi 
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
SET IDENTITY_INSERT [dbo].[CertificateType] ON
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (1, N'企业法人营业执照',2)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (2, N'药品经营许可证',2)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (3, N'药品经营质量管理规范认证证书',2)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (4, N'组织机构代码证',2)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (5, N'变更与记录',2)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (6, N'税务登记证',2)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (7, N'开户许可证',2)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (8, N'企业质量体系调查表',2)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (9, N'开票资料',2)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (10, N'法人委托书',2)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (11, N'委托我司委托书',2)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (12, N'质量保证协议书',2)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (13, N'购销合同',2)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (14, N'出库单原件样式',2)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (15, N'印章印模',2)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (16, N'税票样式',2)


INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (17, N'企业法人营业执照',4)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (18, N'药品生产许可证',4)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (19, N'变更与记录',4)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (20, N'药品GMP证书',4)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (21, N'组织机构代码证',4)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (22, N'税务登记证',4)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (23, N'开户许可证',4)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (24, N'企业质量体系调查表',4)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (25, N'质量保证协议书',4)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (26, N'开票资料',4)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (27, N'法人委托书',4)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (28, N'委托供货商委托书',4)


INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (29, N'药品有效期',5)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (30, N'药品注册批件',5)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (31, N'药品再注册批件',5)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (32, N'质量标准',5)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (33, N'物价批文',5)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (34, N'省检报告',5)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (35, N'商标注册证',5)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (36, N'中国商品条码系统成员证书',5)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (37, N'药品批准文号',5)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (38, N'说明书，样盒',5)

SET IDENTITY_INSERT [dbo].[CertificateType] OFF

SET IDENTITY_INSERT [dbo].[SaleType] ON
INSERT INTO [dbo].[SaleType] ([ID], [SaleType]) VALUES (1, N'高价')
INSERT INTO [dbo].[SaleType] ([ID], [SaleType]) VALUES (2, N'低价')
SET IDENTITY_INSERT [dbo].[SaleType] OFF

SET IDENTITY_INSERT [dbo].[ProductCategory] ON
INSERT INTO [dbo].[ProductCategory] ([ID], [CategoryName]) VALUES (1, N'基药')
INSERT INTO [dbo].[ProductCategory] ([ID], [CategoryName]) VALUES (2, N'招商')
INSERT INTO [dbo].[ProductCategory] ([ID], [CategoryName]) VALUES (3, N'混合')
SET IDENTITY_INSERT [dbo].[ProductCategory] OFF

COMMIT TRANSACTION

---- end ---- 11/6/2014 -- 初始化CertificateType, SaleType数据 -- by Yinzhi 

---- start --- 11/13/2014 -- 初始化DeptDistrict -- by Yinzhi 
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
SET IDENTITY_INSERT [dbo].[DeptDistrict] ON
INSERT INTO [dbo].[DeptDistrict] ([ID], [DepartmentTypeID], [DistrictName]) VALUES (1,1, N'南宁')
INSERT INTO [dbo].[DeptDistrict] ([ID], [DepartmentTypeID], [DistrictName]) VALUES (2,1, N'柳州')
INSERT INTO [dbo].[DeptDistrict] ([ID], [DepartmentTypeID], [DistrictName]) VALUES (3,1, N'百色')
INSERT INTO [dbo].[DeptDistrict] ([ID], [DepartmentTypeID], [DistrictName]) VALUES (4,2, N'桂南')
INSERT INTO [dbo].[DeptDistrict] ([ID], [DepartmentTypeID], [DistrictName]) VALUES (5,2, N'桂北')

SET IDENTITY_INSERT [dbo].[DeptDistrict] OFF

SET IDENTITY_INSERT [dbo].[DeptMarket] ON
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (1,1, N'南宁')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (2,1, N'崇左')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (3,1, N'百色')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (4,1, N'钦防北')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (5,2, N'柳州')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (6,2, N'来宾')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (7,2, N'河池')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (8,2, N'桂林')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (9,3, N'梧州')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (10,3, N'贺州（除苍梧、岑溪）')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (11,3, N'贵港市区')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (12,3, N'桂平')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (13,3, N'博白')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (14,3, N'北流')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (15,3, N'玉林市区')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (16,3, N'容县')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (17,3, N'兴业')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (18,3, N'陆川')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (19,3, N'岑溪')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (20,3, N'苍梧')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (21,3, N'平南')

INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (22,4, N'南宁')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (23,4, N'百色')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (24,4, N'钦州')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (25,4, N'防城')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (26,4, N'北海')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (27,4, N'崇左')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (28,4, N'贵港')


INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (29,5, N'柳州')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (30,5, N'来宾')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (31,5, N'桂林')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (32,5, N'河池')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (33,5, N'玉林')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (34,5, N'梧州')
INSERT INTO [dbo].[DeptMarket] ([ID], [DeptDistrictID], [MarketName]) VALUES (35,5, N'贺州')

SET IDENTITY_INSERT [dbo].[DeptMarket] OFF


COMMIT TRANSACTION

---- end ---- 11/13/2014 -- 初始化DeptDistrict -- by Yinzhi 


---- start --- 10/31/2014 -- 初始化客户证照类型数据 -- by lihong 
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
ALTER TABLE [dbo].[CertificateType] DROP CONSTRAINT [FK_CertificateType_OwnerType]
SET IDENTITY_INSERT [dbo].[CertificateType] ON
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (39, N'企业法人营业执照', 3)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (40, N'药品经营许可证', 3)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (41, N'组织机构代码证', 3)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (42, N'变更与记录', 3)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (43, N'税务登记证', 3)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (44, N'开户许可证', 3)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (45, N'企业质量体系调查表', 3)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (46, N'开票资料', 3)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (47, N'采购委托书', 3)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (48, N'质量保证协议书', 3)
INSERT INTO [dbo].[CertificateType] ([ID], [CertificateType], [OwnerTypeID]) VALUES (49, N'购销合同', 3)
SET IDENTITY_INSERT [dbo].[CertificateType] OFF
ALTER TABLE [dbo].[CertificateType]
    ADD CONSTRAINT [FK_CertificateType_OwnerType] FOREIGN KEY ([OwnerTypeID]) REFERENCES [dbo].[OwnerType] ([ID])
COMMIT TRANSACTION
---- end --- 10/31/2014 -- 初始化客户证照类型数据 -- by lihong 
