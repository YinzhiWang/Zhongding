using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Common.Extension;

namespace ZhongDing.Business.Repositories
{
    public class SalarySettleDetailRepository : BaseRepository<SalarySettleDetail>, ISalarySettleDetailRepository
    {
        public IList<Domain.UIObjects.UISalarySettleDetail> GetUIList(Domain.UISearchObjects.UISearchSalarySettleDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UISalarySettleDetail> uiEntitys = new List<UISalarySettleDetail>();
            int total = 0;


            List<Expression<Func<SalarySettleDetail, bool>>> whereFuncs = new List<Expression<Func<SalarySettleDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                {
                    whereFuncs.Add(x => x.ID == uiSearchObj.ID);
                }

            }
            decimal? decimalNull = null;
            int? intNull = null;
            var settleDate = new DateTime(uiSearchObj.SettleDate.Year, uiSearchObj.SettleDate.Month, 1);
            var query1 = from department in DB.Department.Where(x => x.ID == uiSearchObj.DepartmentID && x.IsDeleted == false)
                         join user in DB.Users.Where(x => x.IsDeleted == false && x.DepartmentID == uiSearchObj.DepartmentID) on department.ID equals user.DepartmentID
                         select user;
            var r1 = query1.ToList();
            var query2 = from salarySettle in DB.SalarySettle.Where(x => x.IsDeleted == false && x.SettleDate == settleDate && x.DepartmentID == uiSearchObj.DepartmentID)
                         join salarySettleDetail in DB.SalarySettleDetail.Where(x => x.IsDeleted == false) on salarySettle.ID equals salarySettleDetail.SalarySettleID
                         select salarySettleDetail;
            var r2 = query2.ToList();
            var query = from q1 in query1
                        join q2 in query2 on q1.UserID equals q2.UserID into tempSalarySettleDetailList
                        from tempSalarySettleDetail in tempSalarySettleDetailList.DefaultIfEmpty()
                        select new UISalarySettleDetail
                        {
                            ID = tempSalarySettleDetail == null ? 0 : tempSalarySettleDetail.ID,
                            UserID = q1.UserID,
                            FullName = q1.FullName,
                            EnrollDate = q1.EnrollDate,
                            BasicSalary = tempSalarySettleDetail == null ? decimalNull : tempSalarySettleDetail.BasicSalary,
                            BonusPay = tempSalarySettleDetail == null ? decimalNull : tempSalarySettleDetail.BonusPay,
                            HolidayDeductOfMealAllowance = tempSalarySettleDetail == null ? decimalNull : tempSalarySettleDetail.HolidayDeductOfMealAllowance,
                            HolidayDeductOfSalary = tempSalarySettleDetail == null ? decimalNull : tempSalarySettleDetail.HolidayDeductOfSalary,
                            MealAllowance = tempSalarySettleDetail == null ? decimalNull : tempSalarySettleDetail.MealAllowance,
                            NeedDeduct = tempSalarySettleDetail == null ? decimalNull : tempSalarySettleDetail.NeedDeduct,
                            NeedPaySalary = tempSalarySettleDetail == null ? decimalNull : tempSalarySettleDetail.NeedPaySalary,
                            OfficeExpense = tempSalarySettleDetail == null ? decimalNull : tempSalarySettleDetail.OfficeExpense,
                            OtherAllowance = tempSalarySettleDetail == null ? decimalNull : tempSalarySettleDetail.OtherAllowance,
                            PhoneAllowance = tempSalarySettleDetail == null ? decimalNull : tempSalarySettleDetail.PhoneAllowance,
                            PositionSalary = tempSalarySettleDetail == null ? decimalNull : tempSalarySettleDetail.PositionSalary,
                            RealPaySalary = tempSalarySettleDetail == null ? decimalNull : tempSalarySettleDetail.RealPaySalary,
                            WorkAgeSalary = tempSalarySettleDetail == null ? decimalNull : tempSalarySettleDetail.WorkAgeSalary,
                            WorkDay = tempSalarySettleDetail == null ? intNull : tempSalarySettleDetail.WorkDay,

                            DefaultBasicSalary = q1.BasicSalary,
                            DefaultPositionSalary = q1.PositionSalary,
                            DefaultPhoneAllowance = q1.PhoneAllowance,
                            DefaultMealAllowance = q1.MealAllowance,
                            DefaultOfficeExpense = q1.OfficeExpense,
                            DefaultBonusPay = q1.BonusPay,
                        };
            total = query.Count();
            uiEntitys = query.OrderByDescending(x => x.FullName)
                    .Skip(pageSize * pageIndex).Take(pageSize).ToList();
            uiEntitys.ForEach(x =>
            {
                x.Order = pageIndex * pageSize + uiEntitys.IndexOf(x) + 1;
                if (!x.BasicSalary.HasValue)
                    x.BasicSalary = x.DefaultBasicSalary;
                if (!x.PositionSalary.HasValue)
                    x.PositionSalary = x.DefaultPositionSalary;
                if (!x.PhoneAllowance.HasValue)
                    x.PhoneAllowance = x.DefaultPhoneAllowance;
                if (!x.MealAllowance.HasValue)
                    x.MealAllowance = x.DefaultMealAllowance;
                if (!x.OfficeExpense.HasValue)
                    x.OfficeExpense = x.DefaultOfficeExpense;
                if (!x.BonusPay.HasValue)
                    x.BonusPay = x.DefaultBonusPay;
            });

            totalRecords = total;
            return uiEntitys;
        }


        public IList<UISalarySettleDetail> GetUIListForAppPayment(Domain.UISearchObjects.UISearchSalarySettleDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UISalarySettleDetail> uiEntitys = new List<UISalarySettleDetail>();
            int total = 0;

            int? intNull = null;
            var query = from salarySettleDetail in DB.SalarySettleDetail
                        join user in DB.Users on salarySettleDetail.UserID equals user.UserID
                        where salarySettleDetail.IsDeleted == false && salarySettleDetail.SalarySettleID == uiSearchObj.SalarySettleID
                        select new UISalarySettleDetail()
                        {
                            ID = salarySettleDetail.ID,
                            ApplicationPaymentID = salarySettleDetail.ApplicationPaymentID,
                            RealPaySalary = salarySettleDetail.RealPaySalary,
                            FullName = user.FullName,
                            IsPayed = salarySettleDetail.IsPayed,
                            FromBankAccountID = salarySettleDetail.ApplicationPayment != null ? salarySettleDetail.ApplicationPayment.FromBankAccountID : intNull,
                            FromAccount = salarySettleDetail.ApplicationPayment != null ? salarySettleDetail.ApplicationPayment.FromAccount : "",
                        };
            total = query.Count();
            uiEntitys = query.OrderByDescending(x => x.FullName)
                    .Skip(pageSize * pageIndex).Take(pageSize).ToList();

            uiEntitys.ForEach(x =>
            {
                x.Order = pageIndex * pageSize + uiEntitys.IndexOf(x) + 1;

            });
            totalRecords = total;
            return uiEntitys;
        }

    }
}
