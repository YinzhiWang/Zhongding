using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class ApplicationPaymentRepository : BaseRepository<ApplicationPayment>, IApplicationPaymentRepository
    {
        public IList<UIApplicationPayment> GetUIList(UISearchApplicationPayment uiSearchObj = null)
        {
            IList<UIApplicationPayment> uiEntities = new List<UIApplicationPayment>();

            IQueryable<ApplicationPayment> query = null;

            List<Expression<Func<ApplicationPayment, bool>>> whereFuncs = new List<Expression<Func<ApplicationPayment, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.WorkflowID > 0)
                    whereFuncs.Add(x => x.WorkflowID.Equals(uiSearchObj.WorkflowID));

                if (uiSearchObj.ApplicationID > 0)
                    whereFuncs.Add(x => x.ApplicationID == uiSearchObj.ApplicationID);

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.PayDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.PayDate < uiSearchObj.EndDate);
                }

                if (uiSearchObj.PayDate.HasValue)
                    whereFuncs.Add(x => x.PayDate == uiSearchObj.PayDate);
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join cu in DB.Users on q.CreatedBy equals cu.UserID into tempCU
                              from tcu in tempCU.DefaultIfEmpty()
                              select new UIApplicationPayment()
                              {
                                  ID = q.ID,
                                  FromBankAccountID = q.FromBankAccountID,
                                  FromAccount = q.FromAccount,
                                  ToBankAccountID = q.ToBankAccountID,
                                  ToAccount = q.ToAccount,
                                  Amount = q.Amount,
                                  Fee = q.Fee,
                                  PayDate = q.PayDate,
                                  Comment = q.Comment,
                                  CreatedBy = tcu == null ? string.Empty : tcu.FullName
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIApplicationPayment> GetUIList(UISearchApplicationPayment uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIApplicationPayment> uiEntities = new List<UIApplicationPayment>();

            int total = 0;

            IQueryable<ApplicationPayment> query = null;

            List<Expression<Func<ApplicationPayment, bool>>> whereFuncs = new List<Expression<Func<ApplicationPayment, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.WorkflowID > 0)
                    whereFuncs.Add(x => x.WorkflowID.Equals(uiSearchObj.WorkflowID));

                if (uiSearchObj.ApplicationID > 0)
                    whereFuncs.Add(x => x.ApplicationID == uiSearchObj.ApplicationID);

                if (uiSearchObj.PaymentTypeID > 0)
                    whereFuncs.Add(x => x.PaymentTypeID == uiSearchObj.PaymentTypeID);

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.PayDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.PayDate < uiSearchObj.EndDate);
                }

                if (uiSearchObj.PayDate.HasValue)
                    whereFuncs.Add(x => x.PayDate == uiSearchObj.PayDate);

                if (uiSearchObj.PaymentTypeID > 0)
                {
                    whereFuncs.Add(x => x.PaymentTypeID == uiSearchObj.PaymentTypeID);

                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join cu in DB.Users on q.CreatedBy equals cu.UserID into tempCU
                              from tcu in tempCU.DefaultIfEmpty()
                              select new UIApplicationPayment()
                              {
                                  ID = q.ID,
                                  FromBankAccountID = q.FromBankAccountID,
                                  FromAccount = q.FromAccount,
                                  ToBankAccountID = q.ToBankAccountID,
                                  ToAccount = q.ToAccount,
                                  Amount = q.Amount,
                                  Fee = q.Fee,
                                  PayDate = q.PayDate,
                                  Comment = q.Comment,
                                  CreatedBy = tcu == null ? string.Empty : tcu.FullName
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
