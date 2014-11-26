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
    public class DeptProductEvaluationRepository : BaseRepository<DeptProductEvaluation>, IDeptProductEvaluationRepository
    {
        public IList<UIDeptProductEvaluation> GetUIList(UISearchDeptProductEvaluation uiSearchObj = null)
        {
            IList<UIDeptProductEvaluation> uiEntities = new List<UIDeptProductEvaluation>();

            IQueryable<DeptProductEvaluation> query = null;

            List<Expression<Func<DeptProductEvaluation, bool>>> whereFuncs = new List<Expression<Func<DeptProductEvaluation, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DepartmentID > 0)
                    whereFuncs.Add(x => x.DepartmentID.Equals(uiSearchObj.DepartmentID));

                if (!string.IsNullOrEmpty(uiSearchObj.ProductName))
                    whereFuncs.Add(x => x.Product.ProductName.Contains(uiSearchObj.ProductName));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join p in DB.Product on q.ProductID equals p.ID
                              select new UIDeptProductEvaluation()
                              {
                                  ID = q.ID,
                                  DepartmentID = q.DepartmentID,
                                  ProductName = p.ProductName,
                                  InvestigateRatio = q.InvestigateRatio,
                                  SalesRatio = q.SalesRatio,
                                  SubtotalRatio = q.InvestigateRatio + q.SalesRatio
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIDeptProductEvaluation> GetUIList(UISearchDeptProductEvaluation uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDeptProductEvaluation> uiEntities = new List<UIDeptProductEvaluation>();

            int total = 0;

            IQueryable<DeptProductEvaluation> query = null;

            List<Expression<Func<DeptProductEvaluation, bool>>> whereFuncs = new List<Expression<Func<DeptProductEvaluation, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DepartmentID > 0)
                    whereFuncs.Add(x => x.DepartmentID.Equals(uiSearchObj.DepartmentID));

                if (!string.IsNullOrEmpty(uiSearchObj.ProductName))
                    whereFuncs.Add(x => x.Product.ProductName.Contains(uiSearchObj.ProductName));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join p in DB.Product on q.ProductID equals p.ID
                              select new UIDeptProductEvaluation()
                              {
                                  ID = q.ID,
                                  DepartmentID = q.DepartmentID,
                                  ProductName = p.ProductName,
                                  InvestigateRatio = q.InvestigateRatio,
                                  SalesRatio = q.SalesRatio,
                                  SubtotalRatio = q.InvestigateRatio + q.SalesRatio
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
