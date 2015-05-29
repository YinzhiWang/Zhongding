
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


---- start --- 11/16/2014 -- 初始化客户证照类型数据 -- by lihong 
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
---- end --- 11/16/2014 -- 初始化客户证照类型数据 -- by lihong 


---- start --- 11/18/2014 -- 初始化基本单位数据 -- by lihong 
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
SET IDENTITY_INSERT [dbo].[UnitOfMeasurement] ON
INSERT INTO [dbo].[UnitOfMeasurement] ([ID], [UnitName]) VALUES (1, N'支')
INSERT INTO [dbo].[UnitOfMeasurement] ([ID], [UnitName]) VALUES (2, N'条')
INSERT INTO [dbo].[UnitOfMeasurement] ([ID], [UnitName]) VALUES (3, N'瓶')
INSERT INTO [dbo].[UnitOfMeasurement] ([ID], [UnitName]) VALUES (4, N'盒')
INSERT INTO [dbo].[UnitOfMeasurement] ([ID], [UnitName]) VALUES (5, N'袋')
SET IDENTITY_INSERT [dbo].[UnitOfMeasurement] OFF
COMMIT TRANSACTION
---- end --- 11/18/2014 -- 初始化基本单位数据 -- by lihong 

---- start --- 12/04/2014 -- 初始化工作流数据 -- by Yinzhi 
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
SET IDENTITY_INSERT [dbo].[Workflow] ON
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName],[IsActive], [IsDeleted]) VALUES (1, N'采购订单',1,0)
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName],[IsActive], [IsDeleted]) VALUES (2, N'采购入库单',1,0)
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName],[IsActive], [IsDeleted]) VALUES (3, N'大包申请单',1,0)
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName],[IsActive], [IsDeleted]) VALUES (4, N'大包配送订单',1,0)
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName],[IsActive], [IsDeleted]) VALUES (5, N'大包出库单',1,0)
SET IDENTITY_INSERT [dbo].[Workflow] OFF

SET IDENTITY_INSERT [dbo].[WorkflowStep] ON
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID],[StepName], [IsDeleted]) VALUES (1,1, N'订单新增',0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID],[StepName], [IsDeleted]) VALUES (2,1, N'订单审核',0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID],[StepName], [IsDeleted]) VALUES (3,1, N'支付信息审核',0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID],[StepName], [IsDeleted]) VALUES (4,1, N'出纳',0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID],[StepName], [IsDeleted]) VALUES (5,1, N'修改订单',0)

INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID],[StepName], [IsDeleted]) VALUES (6,2, N'入库单新增',0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID],[StepName], [IsDeleted]) VALUES (7,2, N'仓库入库操作',0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID],[StepName], [IsDeleted]) VALUES (8,2, N'修改入库单',0)

INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID],[StepName], [IsDeleted]) VALUES (9,3, N'大包申请新增',0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID],[StepName], [IsDeleted]) VALUES (10,3, N'行政审核',0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID],[StepName], [IsDeleted]) VALUES (11,3, N'修改大包申请单',0)


INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID],[StepName], [IsDeleted]) VALUES (12,4, N'修改大包配送订单',0)

INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID],[StepName], [IsDeleted]) VALUES (13,5, N'大包出库单新增',0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID],[StepName], [IsDeleted]) VALUES (14,5, N'仓库出库操作',0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID],[StepName], [IsDeleted]) VALUES (15,5, N'修改出库单',0)

SET IDENTITY_INSERT [dbo].[WorkflowStep] OFF


COMMIT TRANSACTION
---- end --- 12/04/2014 -- 初始化工作流数据 -- by Yinzhi 

---- start --- 12/04/2014 -- 初始化支付类型和状态数据 -- by Yinzhi
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION

SET IDENTITY_INSERT [dbo].[PaymentType] ON
INSERT INTO [dbo].[PaymentType] ([ID], [PaymentTypeName]) VALUES (1, N'收入')
INSERT INTO [dbo].[PaymentType] ([ID], [PaymentTypeName]) VALUES (2, N'支出')
SET IDENTITY_INSERT [dbo].[PaymentType] OFF

SET IDENTITY_INSERT [dbo].[PaymentStatus] ON
INSERT INTO [dbo].[PaymentStatus] ([ID], [PaymentStatusName]) VALUES (1, N'待支付')
INSERT INTO [dbo].[PaymentStatus] ([ID], [PaymentStatusName]) VALUES (2, N'已支付')
SET IDENTITY_INSERT [dbo].[PaymentStatus] OFF

COMMIT TRANSACTION
---- end --- 12/04/2014 -- 初始化工作流数据 -- by Yinzhi 


---- start --- 12/19/2014 -- 初始化工作流状态和关联数据 -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION

SET IDENTITY_INSERT [dbo].[WorkflowStatus] ON
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (1, N'暂存', N'未提交的单据，可修改', 0)
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (2, N'提交', N'已提交的单据，不可修改', 0)
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (3, N'已审核', N'已审核的单据，不可修改', 0)
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (4, N'退回', N'未通过审核的单据，可修改', 0)
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (5, N'支付信息待审核', N'已有支付信息的待审核的单据，不可修改', 0)
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (6, N'待支付', N'待支付的单据，不可修改', 0)
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (7, N'支付信息退回', N'支付信息未通过审核的单据，可修改', 0)
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (8, N'已支付', N'已支付的单据，不可修改', 0)
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (9, N'待入库', N'待入库的采购订单，可修改', 0)
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (10, N'已入库', N'已入库的采购订单，不可修改', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStatus] OFF


ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]

SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] ON
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (1, 1, 1, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (2, 1, 3, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (3, 1, 4, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (4, 1, 7, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (5, 2, 2, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (6, 3, 5, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (7, 4, 6, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (8, 5, 1, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (9, 5, 3, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (10, 5, 4, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (11, 5, 7, 0)
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] OFF

ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])

COMMIT TRANSACTION

---- start --- 12/19/2014 -- 初始化工作流状态和关联数据 -- by lihong

---- start --- 12/25/2014 -- 初始化工作流状态关联数据(入库单相关) -- by lihong

SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] ON
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (12, 6, 1, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (13, 7, 9, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (14, 8, 1, 0)
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] OFF
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
COMMIT TRANSACTION

---- start --- 12/25/2014 -- 初始化工作流状态关联数据(入库单相关) -- by lihong


---- start --- 12/31/2014 -- 初始化工作流状态关联数据(大包申请单和订单相关) -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
SET IDENTITY_INSERT [dbo].[WorkflowStatus] ON
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (11, N'已生成配送订单', N'大包申请单已生产配送订单，不可修改', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStatus] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] ON
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (15, 9, 1, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (16, 9, 4, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (17, 10, 2, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (18, 11, 1, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (19, 11, 4, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (20, 12, 2, 0)
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] OFF
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
COMMIT TRANSACTION
---- end --- 12/31/2014 -- 初始化工作流状态关联数据(大包申请单和订单相关) -- by lihong


---- start --- 1/13/2015 -- 初始化工作流状态关联数据(大包出库单相关) -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
SET IDENTITY_INSERT [dbo].[WorkflowStatus] ON
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (12, N'待出库', N'待出库的出库单，不可修改', 0)
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (13, N'已出库', N'已出库的出库单，不可修改', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStatus] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] ON
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (21, 13, 1, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (22, 14, 12, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (23, 15, 1, 0)
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] OFF
ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
COMMIT TRANSACTION
---- end --- 1/13/2015 -- 初始化工作流状态关联数据(大包出库单相关) -- by lihong


---- start --- 1/15/2015 -- 初始化备注类型数据 -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
SET IDENTITY_INSERT [dbo].[NoteType] ON
INSERT INTO [dbo].[NoteType] ([ID], [NoteTypeName]) VALUES (1, N'单据备注')
INSERT INTO [dbo].[NoteType] ([ID], [NoteTypeName]) VALUES (2, N'单据审核意见')
SET IDENTITY_INSERT [dbo].[NoteType] OFF
COMMIT TRANSACTION
---- end --- 1/15/2015 -- 初始化备注类型数据 -- by lihong


---- start --- 1/23/2015 -- 初始化订单类型、工作流状态关联数据(客户订单相关)数据 -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
BEGIN TRANSACTION
SET IDENTITY_INSERT [dbo].[SaleOrderType] ON
INSERT INTO [dbo].[SaleOrderType] ([ID], [TypeName]) VALUES (1, N'大包配送模式')
INSERT INTO [dbo].[SaleOrderType] ([ID], [TypeName]) VALUES (2, N'招商模式')
INSERT INTO [dbo].[SaleOrderType] ([ID], [TypeName]) VALUES (3, N'挂靠模式')
SET IDENTITY_INSERT [dbo].[SaleOrderType] OFF

ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]

SET IDENTITY_INSERT [dbo].[Workflow] ON
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName], [IsActive], [IsDeleted]) VALUES (6, N'客户订单', 1, 0)
SET IDENTITY_INSERT [dbo].[Workflow] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStatus] ON
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (14, N'发货中', N'订单已在发货中', 0)
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (15, N'已完成', N'订单已全部发货完成', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStatus] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStep] ON
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (16, 6, N'客户订单新增', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (17, 6, N'客户订单审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (18, 6, N'客户订单中止', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (19, 6, N'修改客户订单', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStep] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] ON
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (24, 16, 1, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (25, 16, 4, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (26, 17, 2, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (27, 18, 3, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (28, 18, 14, 0)
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] OFF

ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])

COMMIT TRANSACTION
---- end --- 1/23/2015 -- 初始化订单类型、工作流状态关联数据(客户订单相关)数据 -- by lihong


---- start --- 1/29/2015 -- 初始化订单类型、工作流状态关联数据(客户出库单相关)数据 -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]
UPDATE [dbo].[WorkflowStep] SET [StepName]=N'大包出库单出库操作' WHERE [ID]=14
UPDATE [dbo].[WorkflowStep] SET [StepName]=N'修改大包出库单' WHERE [ID]=15

SET IDENTITY_INSERT [dbo].[Workflow] ON
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName], [IsActive], [IsDeleted]) VALUES (7, N'客户订单出库单', 1, 0)
SET IDENTITY_INSERT [dbo].[Workflow] OFF

SET IDENTITY_INSERT [dbo].[WorkflowStep] ON
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (20, 7, N'客户订单出库单新增', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (21, 7, N'客户出库单出库操作', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (22, 7, N'修改客户订单出库单', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStep] OFF

SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] ON
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (29, 20, 1, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (30, 21, 12, 0)
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] OFF

ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
COMMIT TRANSACTION
---- end --- 1/29/2015 -- 初始化订单类型、工作流状态关联数据(客户出库单相关)数据 -- by lihong


---- start --- 2/4/2015 -- 初始化工作流状态关联数据(供应商返款)数据 -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]

SET IDENTITY_INSERT [dbo].[Workflow] ON
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName], [IsActive], [IsDeleted]) VALUES (8, N'供应商返款', 1, 0)
SET IDENTITY_INSERT [dbo].[Workflow] OFF

SET IDENTITY_INSERT [dbo].[WorkflowStep] ON
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (23, 8, N'修改供应商返款', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStep] OFF

ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
COMMIT TRANSACTION

---- end --- 2/4/2015 -- 初始化工作流状态关联数据(供应商返款)数据 -- by lihong

---- start --- 2/11/2015 -- 初始化工作流状态关联数据(客户返款)数据 -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]
SET IDENTITY_INSERT [dbo].[Workflow] ON
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName], [IsActive], [IsDeleted]) VALUES (9, N'客户返款', 1, 0)
SET IDENTITY_INSERT [dbo].[Workflow] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStatus] ON
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (16, N'财务主管审核通过', N'财务主管审核通过', 0)
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (17, N'部门领导审核通过', N'部门领导审核通过', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStatus] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStep] ON
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (24, 9, N'客户返款新增', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (25, 9, N'客户返款财务主管审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (26, 9, N'客户返款部门经理审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (27, 9, N'客户返款出纳支付', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (28, 9, N'修改客户返款', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStep] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] ON
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (31, 24, 1, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (32, 24, 4, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (33, 25, 2, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (34, 26, 16, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (35, 27, 17, 0)
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] OFF
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
COMMIT TRANSACTION

---- end --- 2/11/2015 -- 初始化工作流状态关联数据(客户返款)数据 -- by lihong

---- start --- 2/15/2015 -- 初始化工作流状态关联数据(厂家经理返款)数据 -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]

SET IDENTITY_INSERT [dbo].[Workflow] ON
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName], [IsActive], [IsDeleted]) VALUES (10, N'厂家经理返款', 1, 0)
SET IDENTITY_INSERT [dbo].[Workflow] OFF

SET IDENTITY_INSERT [dbo].[WorkflowStep] ON
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (29, 10, N'厂家经理返款新增', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (30, 10, N'厂家经理返款财务主管审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (31, 10, N'厂家经理返款部门经理审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (32, 10, N'厂家经理返款出纳支付', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (33, 10, N'修改厂家经理返款', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStep] OFF

SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] ON
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (36, 29, 1, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (37, 29, 4, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (38, 30, 2, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (39, 31, 16, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (40, 32, 17, 0)
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] OFF
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
COMMIT TRANSACTION

---- start --- 2/15/2015 -- 初始化工作流状态关联数据(厂家经理返款)数据 -- by lihong

---- start --- 2/17/2015 -- 初始化工作流状态关联数据(供应商任务返款)数据 -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]
SET IDENTITY_INSERT [dbo].[Workflow] ON
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName], [IsActive], [IsDeleted]) VALUES (11, N'供应商任务返款', 1, 0)
SET IDENTITY_INSERT [dbo].[Workflow] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStep] ON
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (34, 11, N'修改供应商任务返款', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStep] OFF
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
COMMIT TRANSACTION
---- end --- 2/17/2015 -- 初始化工作流状态关联数据(供应商任务返款)数据 -- by lihong

---- start --- 3/17/2015 -- 初始化工作流状态关联数据(客户奖励返款)数据 -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]
SET IDENTITY_INSERT [dbo].[Workflow] ON
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName], [IsActive], [IsDeleted]) VALUES (12, N'客户任务奖励返款', 1, 0)
SET IDENTITY_INSERT [dbo].[Workflow] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStatus] ON
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (18, N'大区经理审核通过', N'大区经理审核通过', 0)
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (19, N'市场总管审核通过', N'市场总管审核通过', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStatus] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStep] ON
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (35, 12, N'客户奖励返款新增', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (36, 12, N'客户奖励返款大区经理审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (37, 12, N'客户奖励返款市场总管审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (38, 12, N'客户奖励返款财务主管审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (39, 12, N'客户奖励返款部门领导审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (40, 12, N'客户奖励返款出纳支付', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (41, 12, N'修改客户奖励返款', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStep] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] ON
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (41, 35, 1, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (42, 35, 4, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (43, 36, 2, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (44, 37, 18, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (45, 38, 19, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (46, 39, 16, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (47, 40, 17, 0)
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] OFF
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
COMMIT TRANSACTION
---- end --- 3/17/2015 -- 初始化工作流状态关联数据(客户奖励返款)数据 -- by lihong

---- start --- 3/27/2015 -- 删除CertificateType药品再注册批件 -- by lihong
----20150326客户反馈:批准文号和注册批件/再注册批件可以放在一起填写
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
BEGIN TRANSACTION
ALTER TABLE [dbo].[CertificateType] DROP CONSTRAINT [FK_CertificateType_OwnerType]
UPDATE [dbo].[CertificateType] SET [IsDeleted]=1 WHERE [ID]=31
ALTER TABLE [dbo].[CertificateType]
    ADD CONSTRAINT [FK_CertificateType_OwnerType] FOREIGN KEY ([OwnerTypeID]) REFERENCES [dbo].[OwnerType] ([ID])
COMMIT TRANSACTION
---- end --- 3/27/2015 -- 删除CertificateType药品再注册批件 -- by lihong

---- start --- 3/31/2015 -- 初始化医院性质数据 -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
SET IDENTITY_INSERT [dbo].[HospitalType] ON
INSERT INTO [dbo].[HospitalType] ([ID], [TypeName]) VALUES (1, N'基药')
INSERT INTO [dbo].[HospitalType] ([ID], [TypeName]) VALUES (2, N'招商')
SET IDENTITY_INSERT [dbo].[HospitalType] OFF
COMMIT TRANSACTION
---- end --- 3/31/2015 -- 初始化医院性质数据 -- by lihong

---- start --- 4/3/2015 -- 初始化导入数据类型数据 -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
SET IDENTITY_INSERT [dbo].[ImportDataType] ON
INSERT INTO [dbo].[ImportDataType] ([ID], [TypeName]) VALUES (1, N'配送公司流向数据')
INSERT INTO [dbo].[ImportDataType] ([ID], [TypeName]) VALUES (2, N'配送公司库存数据')
INSERT INTO [dbo].[ImportDataType] ([ID], [TypeName]) VALUES (3, N'商业客户流向数据')
INSERT INTO [dbo].[ImportDataType] ([ID], [TypeName]) VALUES (4, N'采购订单数据')
INSERT INTO [dbo].[ImportDataType] ([ID], [TypeName]) VALUES (5, N'收货入库单数据')
INSERT INTO [dbo].[ImportDataType] ([ID], [TypeName]) VALUES (6, N'客户订单数据')
INSERT INTO [dbo].[ImportDataType] ([ID], [TypeName]) VALUES (7, N'客户出库单数据')
INSERT INTO [dbo].[ImportDataType] ([ID], [TypeName]) VALUES (8, N'大包订单数据')
INSERT INTO [dbo].[ImportDataType] ([ID], [TypeName]) VALUES (9, N'大包出库单数据')
SET IDENTITY_INSERT [dbo].[ImportDataType] OFF
COMMIT TRANSACTION
---- start --- 4/3/2015 -- 初始化导入数据类型数据 -- by lihong

---- start --- 4/8/2015 -- 初始化工作流状态关联数据(大包客户提成结算)数据 -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]
SET IDENTITY_INSERT [dbo].[Workflow] ON
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName], [IsActive], [IsDeleted]) VALUES (13, N'大包客户提成结算', 1, 0)
SET IDENTITY_INSERT [dbo].[Workflow] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStatus] ON
INSERT INTO [dbo].[WorkflowStatus] ([ID], [StatusName], [Comment], [IsDeleted]) VALUES (20, N'未结算', N'未结算的单据(提成等)', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStatus] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStep] ON
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (42, 13, N'大包客户提成申请结算', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (43, 13, N'大包客户提成部门领导审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (44, 13, N'大包客户提成出纳支付', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (45, 13, N'修改大包客户提成', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStep] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] ON
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (48, 42, 20, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (49, 42, 4, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (50, 43, 2, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (51, 44, 17, 0)
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] OFF
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
COMMIT TRANSACTION
---- end --- 4/8/2015 -- 初始化工作流状态关联数据(大包客户提成结算)数据 -- by lihong

GO
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
GO
SET DATEFORMAT YMD
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
INSERT INTO [dbo].[InvoiceType] ([ID], [Name]) VALUES (1, N'高价')
INSERT INTO [dbo].[InvoiceType] ([ID], [Name]) VALUES (2, N'低价')
INSERT INTO [dbo].[InvoiceType] ([ID], [Name]) VALUES (3, N'平价')
COMMIT TRANSACTION
GO
--end 初始化 发票类型

---- start --- 4/17/2015 -- 初始化工作流状态关联数据(供应商和客户发票结算)数据 -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]
SET IDENTITY_INSERT [dbo].[Workflow] ON
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName], [IsActive], [IsDeleted]) VALUES (14, N'供应商发票结算', 1, 0)
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName], [IsActive], [IsDeleted]) VALUES (15, N'客户发票结算', 1, 0)
SET IDENTITY_INSERT [dbo].[Workflow] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStep] ON
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (46, 14, N'新增供应商发票结算', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (47, 14, N'修改供应商发票结算', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (48, 15, N'新增客户发票结算', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (49, 15, N'客户发票结算财务主管审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (50, 15, N'客户发票结算部门领导审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (51, 15, N'客户发票结算出纳支付', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (52, 15, N'修改客户发票结算', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStep] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] ON
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (52, 48, 1, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (53, 48, 4, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (54, 49, 2, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (55, 50, 16, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (56, 51, 17, 0)
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] OFF
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
COMMIT TRANSACTION
---- end --- 4/17/2015 -- 初始化工作流状态关联数据(供应商和客户发票结算)数据 -- by lihong

---- start --- 4/22/2015 -- 初始化工作流状态关联数据(采购订单中止功能)数据 -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]
SET IDENTITY_INSERT [dbo].[WorkflowStep] ON
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (53, 1, N'中止采购订单', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStep] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] ON
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (57, 53, 14, 0)
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] OFF
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
COMMIT TRANSACTION
---- end --- 4/22/2015 -- 初始化工作流状态关联数据(采购订单中止功能)数据 -- by lihong

---- start --- 4/23/2015 -- 初始化工作流状态关联数据(大包收款管理)数据 -- by lihong
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]
SET IDENTITY_INSERT [dbo].[Workflow] ON
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName], [IsActive], [IsDeleted]) VALUES (16, N'大包收款', 1, 0)
SET IDENTITY_INSERT [dbo].[Workflow] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStep] ON
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (54, 16, N'新增大包收款', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (55, 16, N'修改大包收款', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStep] OFF
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
COMMIT TRANSACTION
---- end --- 4/23/2015 -- 初始化工作流状态关联数据(大包收款管理)数据 -- by lihong

---- start --- 4/29/2015 -- 初始化工作流状态关联数据(挂靠发票结算)，以及费用类型数据 -- by lihong

SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

BEGIN TRANSACTION
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
SET IDENTITY_INSERT [dbo].[Workflow] ON
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName], [IsActive], [IsDeleted]) VALUES (17, N'挂靠发票结算', 1, 0)
SET IDENTITY_INSERT [dbo].[Workflow] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] ON
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (58, 56, 1, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (59, 56, 4, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (60, 57, 2, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (61, 58, 16, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (62, 59, 17, 0)
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] OFF
SET IDENTITY_INSERT [dbo].[WorkflowStep] ON
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (56, 17, N'新增挂靠发票结算', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (57, 17, N'挂靠发票结算财务主管审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (58, 17, N'挂靠发票结算部门领导审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (59, 17, N'挂靠发票结算出纳支付', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (60, 17, N'修改挂靠发票结算', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStep] OFF
SET IDENTITY_INSERT [dbo].[CostType] ON
INSERT INTO [dbo].[CostType] ([ID], [TypeName]) VALUES (1, N'手续费')
INSERT INTO [dbo].[CostType] ([ID], [TypeName]) VALUES (2, N'托管费')
INSERT INTO [dbo].[CostType] ([ID], [TypeName]) VALUES (3, N'破损')
SET IDENTITY_INSERT [dbo].[CostType] OFF
ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
COMMIT TRANSACTION

---- end --- 4/29/2015 -- 初始化工作流状态关联数据(挂靠发票结算)，以及费用类型数据 -- by lihong




---- start --- 4/29/2015 -- 初始化工作流状态关联数据(供应商保证金申请) -- by Nwang

BEGIN TRANSACTION
GO
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
GO
SET IDENTITY_INSERT [dbo].[Workflow] ON
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName], [IsActive], [IsDeleted]) VALUES (18, N'供应商保证金申请', 1, 0)
SET IDENTITY_INSERT [dbo].[Workflow] OFF
GO
SET IDENTITY_INSERT [dbo].[WorkflowStep] ON
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (61, 18, N'新增供应商保证金申请', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (62, 18, N'供应商保证金部门领导审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (63, 18, N'供应商保证金财务主管审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (64, 18, N'供应商保证金出纳支付', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (65, 18, N'修改供应商保证金申请', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStep] OFF
GO
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] ON
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (63, 61, 1, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (64, 61, 4, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (65, 62, 2, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (66, 63, 17, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (67, 64, 16, 0)
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] OFF

ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
COMMIT TRANSACTION
---- end --- 4/29/2015 -- 初始化工作流状态关联数据(挂靠发票结算)，以及费用类型数据 -- by Nwang



---- start --- 4/30/2015 -- 初始化供应商保证金类型 -- by Nwang
BEGIN TRANSACTION
INSERT [CautionMoneyType] ([ID],[Name]) VALUES ( 1,N'增补保证金')
INSERT [CautionMoneyType] ([ID],[Name]) VALUES ( 2,N'市场保证金')
INSERT [CautionMoneyType] ([ID],[Name]) VALUES ( 3,N'开发保证金')
INSERT [CautionMoneyType] ([ID],[Name]) VALUES ( 4,N'销量保证金')
COMMIT TRANSACTION
---- end --- 4/30/2015 -- 初始化供应商保证金类型 -- by Nwang

---- start --- 5/20/2015 -- 初始化客户保证金类型 -- by Nwang
BEGIN TRANSACTION
UPDATE dbo.CautionMoneyType SET [Type]=1
INSERT [CautionMoneyType] ([ID],[Name],[Type]) VALUES ( 5,N'市场保证金',2)
INSERT [CautionMoneyType] ([ID],[Name],[Type]) VALUES ( 6,N'开发保证金',2)
INSERT [CautionMoneyType] ([ID],[Name],[Type]) VALUES ( 7,N'销量保证金',2)
INSERT [CautionMoneyType] ([ID],[Name],[Type]) VALUES ( 8,N'其他保证金',2)
COMMIT TRANSACTION
---- end --- 5/20/2015 -- 初始化客户保证金类型 -- by Nwang

---- start --- 5/20/2015 -- 初始化工作流状态关联数据(客户保证金) -- by Nwang

BEGIN TRANSACTION
GO
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
GO
SET IDENTITY_INSERT [dbo].[Workflow] ON
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName], [IsActive], [IsDeleted]) VALUES (19, N'客户保证金', 1, 0)
SET IDENTITY_INSERT [dbo].[Workflow] OFF
GO
ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
COMMIT TRANSACTION
---- end --- 5/20/2015 -- 初始化工作流状态关联数据(客户保证金) -- by Nwang

---- start --- 5/20/2015 -- 初始化工作流状态关联数据(客户保证金申请) -- by Nwang

BEGIN TRANSACTION
GO
ALTER TABLE [dbo].[WorkflowStep] DROP CONSTRAINT [FK_WorkflowStep_Workflow]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus]
ALTER TABLE [dbo].[WorkflowStepStatus] DROP CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep]
GO
SET IDENTITY_INSERT [dbo].[Workflow] ON
INSERT INTO [dbo].[Workflow] ([ID], [WorkflowName], [IsActive], [IsDeleted]) VALUES (20, N'客户保证金退回申请', 1, 0)
SET IDENTITY_INSERT [dbo].[Workflow] OFF
GO
SET IDENTITY_INSERT [dbo].[WorkflowStep] ON
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (66, 20, N'新增客户保证金退回申请', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (67, 20, N'客户保证金退回申请部门经理审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (68, 20, N'客户保证金退回申请财务主管审核', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (69, 20, N'客户保证金退回出纳支付', 0)
INSERT INTO [dbo].[WorkflowStep] ([ID], [WorkflowID], [StepName], [IsDeleted]) VALUES (70, 20, N'修改客户保证金退回申请', 0)
SET IDENTITY_INSERT [dbo].[WorkflowStep] OFF
GO
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] ON
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (68, 66, 1, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (69, 66, 4, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (70, 67, 2, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (71, 68, 17, 0)
INSERT INTO [dbo].[WorkflowStepStatus] ([ID], [WorkflowStepID], [WorkflowStatusID], [IsDeleted]) VALUES (72, 69, 16, 0)
SET IDENTITY_INSERT [dbo].[WorkflowStepStatus] OFF

ALTER TABLE [dbo].[WorkflowStep]
    ADD CONSTRAINT [FK_WorkflowStep_Workflow] FOREIGN KEY ([WorkflowID]) REFERENCES [dbo].[Workflow] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStatus] FOREIGN KEY ([WorkflowStatusID]) REFERENCES [dbo].[WorkflowStatus] ([ID])
ALTER TABLE [dbo].[WorkflowStepStatus]
    ADD CONSTRAINT [FK_WorkflowStepStatus_WorkflowStep] FOREIGN KEY ([WorkflowStepID]) REFERENCES [dbo].[WorkflowStep] ([ID])
COMMIT TRANSACTION
---- end --- 5/20/2015 -- 初始化工作流状态关联数据(客户保证金申请) -- by Nwang


---- start --- 5/27/2015 -- 初始化菜单模块权限 -- by Nwang
BEGIN TRANSACTION
ALTER TABLE [dbo].[UserGroupPermission] DROP CONSTRAINT [FK_UserGroupPermission_PermissionID]
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (1, N'账套管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:05:56.713', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (2, N'银行账号管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:07:48.913', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (3, N'供应商管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:08:13.907', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (4, N'商业单位管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:08:35.070', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (5, N'客户管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:08:48.620', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (6, N'仓库管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:09:03.063', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (7, N'配送公司管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:09:24.773', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (8, N'大包客户协议管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:09:45.333', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (9, N'商业客户流向管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:10:02.237', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (10, N'部门销售计划及提成策略
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:10:15.487', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (11, N'物流公司管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:10:27.837', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (12, N'货品管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:10:51.790', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (13, N'货品定价管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:11:05.190', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (14, N'库存货品信息
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:11:44.440', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (15, N'部门管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:12:17.983', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (16, N'员工管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:12:31.817', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (17, N'权限管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:12:43.750', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (18, N'报销类型管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:13:35.980', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (19, N'物流费用管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:23:12.960', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (20, N'担保收款
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:23:31.363', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (21, N'数据导入
', 1, 0, 0, 0, 0, 0, 0, '2015-05-26 21:23:46.160', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (22, N'部门月提成
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:24:06.440', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (23, N'采购订单报表
', 0, 0, 0, 1, 0, 1, 0, '2015-05-26 21:24:53.297', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (24, N'采购付款明细表
', 0, 0, 0, 1, 0, 1, 0, '2015-05-26 21:25:14.140', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (25, N'销售订单报表
', 0, 0, 0, 1, 0, 1, 0, '2015-05-26 21:25:26.920', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (26, N'出库明细表
', 0, 0, 0, 1, 0, 1, 0, '2015-05-26 21:25:39.633', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (27, N'入库明细表
', 0, 0, 0, 1, 0, 1, 0, '2015-05-26 21:25:52.307', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (28, N'采购计划表
', 0, 0, 0, 1, 0, 1, 0, '2015-05-26 21:26:05.093', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (29, N'库存汇总表
', 0, 0, 0, 1, 0, 1, 0, '2015-05-26 21:26:18.663', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (30, N'配送公司流向结算表
', 0, 0, 0, 1, 0, 1, 0, '2015-05-26 21:26:33.203', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (31, N'大包客户季度考核表
', 0, 0, 0, 1, 0, 1, 0, '2015-05-26 21:26:44.380', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (32, N'配送公司库存核对表
', 0, 0, 0, 1, 0, 1, 0, '2015-05-26 21:26:56.437', NULL, NULL, NULL)
INSERT INTO [dbo].[Permission] ([ID], [Name], [HasCreate], [HasEdit], [HasDelete], [HasView], [HasPrint], [HasExport], [IsDeleted], [CreatedOn], [CreatedBy], [LastModifiedOn], [LastModifiedBy]) VALUES (33, N'发票管理
', 1, 1, 1, 0, 0, 0, 0, '2015-05-26 21:27:13.770', NULL, NULL, NULL)
ALTER TABLE [dbo].[UserGroupPermission] WITH NOCHECK ADD CONSTRAINT [FK_UserGroupPermission_PermissionID] FOREIGN KEY ([PermissionID]) REFERENCES [dbo].[Permission] ([ID])
COMMIT TRANSACTION
---- end --- 5/27/2015 -- 初始化菜单模块权限 -- by Nwang


---- start --- 5/28/2015 -- 更新菜单模块权限 -- by Nwang
BEGIN TRANSACTION
GO
ALTER TABLE [dbo].[UserGroupPermission] DROP CONSTRAINT [FK_UserGroupPermission_PermissionID]

GO
UPDATE [dbo].[Permission] SET [HasCreate]=0, [HasDelete]=0 WHERE [ID]=13
UPDATE [dbo].[Permission] SET [HasCreate]=0, [HasEdit]=0, [HasDelete]=0, [HasView]=1 WHERE [ID]=14
GO
ALTER TABLE [dbo].[UserGroupPermission] WITH NOCHECK ADD CONSTRAINT [FK_UserGroupPermission_PermissionID] FOREIGN KEY ([PermissionID]) REFERENCES [dbo].[Permission] ([ID])
COMMIT TRANSACTION
GO
---- end --- 5/28/2015 -- 更新菜单模块权限 -- by Nwang




