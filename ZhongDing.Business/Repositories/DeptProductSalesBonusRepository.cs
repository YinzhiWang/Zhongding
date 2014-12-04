using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class DeptProductSalesBonusRepository : BaseRepository<DepartmentProductSalesBonus>, IDeptProductSalesBonusRepository
    {
        public IList<UIDeptProductSalesBonus> GetUIList(UISearchDeptProductSalesBonus uiSearchObj = null)
        {
            IList<UIDeptProductSalesBonus> uiEntities = new List<UIDeptProductSalesBonus>();

            IQueryable<DepartmentProductSalesBonus> query = null;

            List<Expression<Func<DepartmentProductSalesBonus, bool>>> whereFuncs = new List<Expression<Func<DepartmentProductSalesBonus, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.SalesPlanID > 0)
                    whereFuncs.Add(x => x.SalesPlanID == uiSearchObj.SalesPlanID);

                if (uiSearchObj.SalesPlanTypeID > 0)
                    whereFuncs.Add(x => x.SalesPlanTypeID == uiSearchObj.SalesPlanTypeID);
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              select new UIDeptProductSalesBonus()
                              {
                                  ID = q.ID,
                                  SalesPlanID = q.SalesPlanID,
                                  SalesPlanTypeID = q.SalesPlanTypeID,
                                  CompareOperatorID = q.CompareOperatorID,
                                  CompareOperator = q.CompareOperatorID == (int)ECompareOperatorType.GreaterThan
                                  ? GlobalConst.CompareOperatorTypes.GREATER_THAN
                                  : (q.CompareOperatorID == (int)ECompareOperatorType.EqualTo
                                  ? GlobalConst.CompareOperatorTypes.EQUAL_TO : GlobalConst.CompareOperatorTypes.LESS_THAN),
                                  SalesPrice = q.SalesPrice,
                                  BonusRatio = q.BonusRatio,
                                  IsDeleted = q.IsDeleted
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIDeptProductSalesBonus> GetUIList(UISearchDeptProductSalesBonus uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDeptProductSalesBonus> uiEntities = new List<UIDeptProductSalesBonus>();

            int total = 0;

            IQueryable<DepartmentProductSalesBonus> query = null;

            List<Expression<Func<DepartmentProductSalesBonus, bool>>> whereFuncs = new List<Expression<Func<DepartmentProductSalesBonus, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.SalesPlanID > 0)
                    whereFuncs.Add(x => x.SalesPlanID == uiSearchObj.SalesPlanID);

                if (uiSearchObj.SalesPlanTypeID > 0)
                    whereFuncs.Add(x => x.SalesPlanTypeID == uiSearchObj.SalesPlanTypeID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              select new UIDeptProductSalesBonus()
                              {
                                  ID = q.ID,
                                  SalesPlanID = q.SalesPlanID,
                                  SalesPlanTypeID = q.SalesPlanTypeID,
                                  CompareOperatorID = q.CompareOperatorID,
                                  CompareOperator = q.CompareOperatorID == (int)ECompareOperatorType.GreaterThan
                                  ? GlobalConst.CompareOperatorTypes.GREATER_THAN
                                  : (q.CompareOperatorID == (int)ECompareOperatorType.EqualTo
                                  ? GlobalConst.CompareOperatorTypes.EQUAL_TO : GlobalConst.CompareOperatorTypes.LESS_THAN),
                                  SalesPrice = q.SalesPrice,
                                  BonusRatio = q.BonusRatio,
                                  IsDeleted = q.IsDeleted
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
