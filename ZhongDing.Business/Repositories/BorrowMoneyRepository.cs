using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Common.Extension;

namespace ZhongDing.Business.Repositories
{
    public class BorrowMoneyRepository : BaseRepository<BorrowMoney>, IBorrowMoneyRepository
    {
        public IList<Domain.UIObjects.UIBorrowMoney> GetUIList(Domain.UISearchObjects.UISearchBorrowMoney uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIBorrowMoney> uiEntites = new List<UIBorrowMoney>();
            int total = 0;

            IQueryable<BorrowMoney> query = null;

            List<Expression<Func<BorrowMoney, bool>>> whereFuncs = new List<Expression<Func<BorrowMoney, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.Status.BiggerThanZero())
                    whereFuncs.Add(x => x.Status == uiSearchObj.Status);
                if (uiSearchObj.BorrowName.IsNotNullOrEmpty())
                {
                    whereFuncs.Add(x => x.BorrowName.Contains(uiSearchObj.BorrowName));
                }
                if (uiSearchObj.BeginDate.HasValue)
                {
                    whereFuncs.Add(x => x.BorrowDate >= uiSearchObj.BeginDate);
                }
                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.BorrowDate < uiSearchObj.EndDate);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntites = (from q in query
                             select new UIBorrowMoney()
                             {
                                 ID = q.ID,
                                 BorrowAmount = q.BorrowAmount,
                                 BorrowDate = q.BorrowDate,
                                 BorrowName = q.BorrowName,
                                 Comment = q.Comment,
                                 ReturnDate = q.ReturnDate,
                                 Status = q.Status,
                                 ReturnAmount = DB.ApplicationPayment.Where(x => x.IsDeleted == false && x.WorkflowID == (int)EWorkflow.BorrowMoneyManagement
                                                && x.ApplicationID == q.ID && x.PaymentStatusID == (int)EPaymentStatus.Paid
                                                && x.PaymentTypeID == (int)EPaymentType.Income).Any() ?
                                                DB.ApplicationPayment.Where(x => x.IsDeleted == false && x.WorkflowID == (int)EWorkflow.BorrowMoneyManagement
                                                && x.ApplicationID == q.ID && x.PaymentStatusID == (int)EPaymentStatus.Paid
                                                && x.PaymentTypeID == (int)EPaymentType.Income).Sum(x => x.Amount) : 0M

                             }).ToList();
            }

            totalRecords = total;

            return uiEntites;
        }








        public UIBorrowMoneyBalance CalculateBalance(Domain.UISearchObjects.UISearchBorrowMoney uiSearchObj)
        {
            IList<UIBorrowMoney> uiEntites = new List<UIBorrowMoney>();
            int total = 0;

            IQueryable<BorrowMoney> queryBorrowMoney = null;

            List<Expression<Func<BorrowMoney, bool>>> whereFuncs = new List<Expression<Func<BorrowMoney, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.Status.BiggerThanZero())
                    whereFuncs.Add(x => x.Status == uiSearchObj.Status);
                if (uiSearchObj.BorrowName.IsNotNullOrEmpty())
                {
                    whereFuncs.Add(x => x.BorrowName.Contains(uiSearchObj.BorrowName));
                }
                if (uiSearchObj.BeginDate.HasValue)
                {
                    whereFuncs.Add(x => x.BorrowDate >= uiSearchObj.BeginDate);
                }
                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.BorrowDate < uiSearchObj.EndDate);
                }
            }

            queryBorrowMoney = GetList(whereFuncs);

            //总计借款
            decimal totalBorrowAmount = queryBorrowMoney.Any() ? queryBorrowMoney.Sum(x => x.BorrowAmount) : 0M;

            var queryTotalReturnedAmount = from q in queryBorrowMoney
                                           join applicationPayment in DB.ApplicationPayment.Where(x => x.WorkflowID == (int)EWorkflow.BorrowMoneyManagement 
                                               && x.IsDeleted == false
                                               &&x.PaymentTypeID == (int)EPaymentType.Income)
                                           on q.ID equals applicationPayment.ApplicationID
                                           select new { applicationPayment.Amount };

            decimal totalReturnedAmount = queryTotalReturnedAmount.Any() ? queryTotalReturnedAmount.Sum(x => x.Amount).Value : 0M;


            return new UIBorrowMoneyBalance() { TotalBorrowAmount = totalBorrowAmount, TotalReturnedAmount = totalReturnedAmount };
        }

    }
}
