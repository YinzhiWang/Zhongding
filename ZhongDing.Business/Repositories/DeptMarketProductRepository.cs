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
    public class DeptMarketProductRepository : BaseRepository<DeptMarketProduct>, IDeptMarketProductRepository
    {
        public IList<UIDeptMarketProduct> GetUIList(UISearchDeptMarketProduct uiSearchObj = null)
        {
            IList<UIDeptMarketProduct> uiEntities = new List<UIDeptMarketProduct>();

            IQueryable<DeptMarketProduct> query = null;

            List<Expression<Func<DeptMarketProduct, bool>>> whereFuncs = new List<Expression<Func<DeptMarketProduct, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.MarketDivisionID > 0)
                    whereFuncs.Add(x => x.MarketDivisionID == uiSearchObj.MarketDivisionID);
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join p in DB.Product on q.ProductID equals p.ID
                              select new UIDeptMarketProduct()
                              {
                                  ID = q.ID,
                                  MarketDivisionID = q.MarketDivisionID,
                                  ProductName = p.ProductName,
                                  Q1Task = q.Q1Task,
                                  Q2Task = q.Q2Task,
                                  Q3Task = q.Q3Task,
                                  Q4Task = q.Q4Task,
                                  SubtotalTask = (q.Q1Task ?? 0) + (q.Q2Task ?? 0)
                                  + (q.Q3Task ?? 0) + (q.Q4Task ?? 0)

                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIDeptMarketProduct> GetUIList(UISearchDeptMarketProduct uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDeptMarketProduct> uiEntities = new List<UIDeptMarketProduct>();

            int total = 0;

            IQueryable<DeptMarketProduct> query = null;

            List<Expression<Func<DeptMarketProduct, bool>>> whereFuncs = new List<Expression<Func<DeptMarketProduct, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.MarketDivisionID > 0)
                    whereFuncs.Add(x => x.MarketDivisionID == uiSearchObj.MarketDivisionID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join p in DB.Product on q.ProductID equals p.ID
                              select new UIDeptMarketProduct()
                              {
                                  ID = q.ID,
                                  MarketDivisionID = q.MarketDivisionID,
                                  ProductName = p.ProductName,
                                  Q1Task = q.Q1Task,
                                  Q2Task = q.Q2Task,
                                  Q3Task = q.Q3Task,
                                  Q4Task = q.Q4Task,
                                  SubtotalTask = (q.Q1Task ?? 0) + (q.Q2Task ?? 0)
                                  + (q.Q3Task ?? 0) + (q.Q4Task ?? 0)

                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
