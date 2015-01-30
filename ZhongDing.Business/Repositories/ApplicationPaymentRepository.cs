﻿using System;
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

                if (uiSearchObj.ApplicationID > 0)
                    whereFuncs.Add(x => x.ApplicationID == uiSearchObj.ApplicationID);

                if (uiSearchObj.WorkflowID > 0)
                    whereFuncs.Add(x => x.WorkflowID.Equals(uiSearchObj.WorkflowID));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              select new UIApplicationPayment()
                              {
                                  ID = q.ID,
                                  FromBankAccountID = q.FromBankAccountID,
                                  FromAccount = q.FromAccount,
                                  ToBankAccountID = q.ToBankAccountID,
                                  ToAccount = q.ToAccount,
                                  Amount = q.Amount,
                                  Fee = q.Fee
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

                if (uiSearchObj.ApplicationID > 0)
                    whereFuncs.Add(x => x.ApplicationID == uiSearchObj.ApplicationID);

                if (uiSearchObj.WorkflowID > 0)
                    whereFuncs.Add(x => x.WorkflowID.Equals(uiSearchObj.WorkflowID));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              select new UIApplicationPayment()
                              {
                                  ID = q.ID,
                                  FromBankAccountID = q.FromBankAccountID,
                                  FromAccount = q.FromAccount,
                                  ToBankAccountID = q.ToBankAccountID,
                                  ToAccount = q.ToAccount,
                                  Amount = q.Amount,
                                  Fee = q.Fee
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}