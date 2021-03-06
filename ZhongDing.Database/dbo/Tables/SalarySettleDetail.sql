﻿CREATE TABLE [dbo].[SalarySettleDetail] (
    [ID]                           INT      IDENTITY (1, 1) NOT NULL,
    [SalarySettleID]               INT      NOT NULL,
    [UserID]                       INT      NOT NULL,
    [BasicSalary]                  MONEY    NOT NULL,
    [WorkDay]                      INT      NOT NULL,
    [MealAllowance]                MONEY    NULL,
    [PositionSalary]               MONEY    NULL,
    [BonusPay]                     MONEY    NULL,
    [WorkAgeSalary]                MONEY    NOT NULL,
    [PhoneAllowance]               MONEY    NULL,
    [OfficeExpense]                MONEY    NULL,
    [OtherAllowance]               MONEY    NOT NULL,
    [NeedPaySalary]                MONEY    NOT NULL,
    [NeedDeduct]                   MONEY    NOT NULL,
    [HolidayDeductOfSalary]        MONEY    NOT NULL,
    [HolidayDeductOfMealAllowance] MONEY    NOT NULL,
    [RealPaySalary]                MONEY    NOT NULL,
    [IsPayed]                      BIT      CONSTRAINT [DF__SalarySet__IsPay__3C3FDE67] DEFAULT ((0)) NOT NULL,
    [ApplicationPaymentID]         INT      NULL,
    [IsDeleted]                    BIT      CONSTRAINT [DF__SalarySet__IsDel__3D3402A0] DEFAULT ((0)) NOT NULL,
    [CreatedOn]                    DATETIME CONSTRAINT [DF__SalarySet__Creat__3E2826D9] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                    INT      NULL,
    [LastModifiedOn]               DATETIME NULL,
    [LastModifiedBy]               INT      NULL,
    CONSTRAINT [PK__SalarySe__3214EC276709445C] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SalarySettleDetail_ApplicationPaymentID] FOREIGN KEY ([ApplicationPaymentID]) REFERENCES [dbo].[ApplicationPayment] ([ID]),
    CONSTRAINT [FK_SalarySettleDetail_SalarySettleID] FOREIGN KEY ([SalarySettleID]) REFERENCES [dbo].[SalarySettle] ([ID]),
    CONSTRAINT [FK_SalarySettleDetail_UserID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

