using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class DeptProductSalesPlanRepository : BaseRepository<DepartmentProductSalesPlan>,
        IDeptProductSalesPlanRepository
    {
        public IList<UIDeptProductSalesPlan> GetUIList(UISearchDeptProductSalesPlan uiSearchObj = null)
        {
            IList<UIDeptProductSalesPlan> uiEntities = new List<UIDeptProductSalesPlan>();

            IQueryable<DepartmentProductSalesPlan> query = null;

            List<Expression<Func<DepartmentProductSalesPlan, bool>>> whereFuncs = new List<Expression<Func<DepartmentProductSalesPlan, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DepartmentID > 0)
                    whereFuncs.Add(x => x.DepartmentID.Equals(uiSearchObj.DepartmentID));

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID.Equals(uiSearchObj.ProductID));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join d in DB.Department on q.DepartmentID equals d.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              select new UIDeptProductSalesPlan()
                              {
                                  ID = q.ID,
                                  DepartmentName = d.DepartmentName,
                                  ProductName = p.ProductName,
                                  //IsFixedOfInside = q.IsFixedOfInside,
                                  //FixedRatioOfInside = q.FixedRatioOfInside,
                                  //IsFixedOfOutside = q.IsFixedOfOutside,
                                  //FixedRatioOfOutside = q.FixedRatioOfOutside,

                                  InsideBonusDescription = q.IsFixedOfInside
                                  ? GlobalConst.DeptProductSalesBonusTypes.FIXED + "：" + q.FixedRatioOfInside * 100 + "%"
                                  : GlobalConst.DeptProductSalesBonusTypes.FLOATED,
                                  OutsideBonusDescription = q.IsFixedOfOutside
                                  ? GlobalConst.DeptProductSalesBonusTypes.FIXED + "：" + q.FixedRatioOfOutside * 100 + "%"
                                  : GlobalConst.DeptProductSalesBonusTypes.FLOATED,

                              }).ToList();

                //foreach (var uiEntity in uiEntities)
                //{
                //    uiEntity.InsideBonusDescription = uiEntity.IsFixedOfInside
                //              ? GlobalConst.DepartmentProductSalesBonusTypes.FIXED + "：" + string.Format("{0:P2}", uiEntity.FixedRatioOfInside)
                //              : GlobalConst.DepartmentProductSalesBonusTypes.FLOATED;

                //    uiEntity.OutsideBonusDescription = uiEntity.IsFixedOfOutside
                //              ? GlobalConst.DepartmentProductSalesBonusTypes.FIXED + "：" + string.Format("{0:P2}", uiEntity.FixedRatioOfOutside)
                //              : GlobalConst.DepartmentProductSalesBonusTypes.FLOATED;
                //}
            }

            return uiEntities;
        }

        public IList<UIDeptProductSalesPlan> GetUIList(UISearchDeptProductSalesPlan uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDeptProductSalesPlan> uiEntities = new List<UIDeptProductSalesPlan>();

            int total = 0;

            IQueryable<DepartmentProductSalesPlan> query = null;

            List<Expression<Func<DepartmentProductSalesPlan, bool>>> whereFuncs = new List<Expression<Func<DepartmentProductSalesPlan, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DepartmentID > 0)
                    whereFuncs.Add(x => x.DepartmentID.Equals(uiSearchObj.DepartmentID));

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID.Equals(uiSearchObj.ProductID));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join d in DB.Department on q.DepartmentID equals d.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              select new UIDeptProductSalesPlan()
                              {
                                  ID = q.ID,
                                  DepartmentName = d.DepartmentName,
                                  ProductName = p.ProductName,
                                  IsFixedOfInside = q.IsFixedOfInside,
                                  FixedRatioOfInside = q.FixedRatioOfInside,
                                  IsFixedOfOutside = q.IsFixedOfOutside,
                                  FixedRatioOfOutside = q.FixedRatioOfOutside
                                  //InsideBonusDescription = q.IsFixedOfInside
                                  //? GlobalConst.DepartmentProductSalesBonusTypes.FIXED + "：" + q.FixedRatioOfInside * 100 + "%"
                                  //: GlobalConst.DepartmentProductSalesBonusTypes.FLOATED,
                                  //OutsideBonusDescription = q.IsFixedOfOutside
                                  //? GlobalConst.DepartmentProductSalesBonusTypes.FIXED + "：" + q.FixedRatioOfOutside * 100 + "%"
                                  //: GlobalConst.DepartmentProductSalesBonusTypes.FLOATED,

                              }).ToList();

                foreach (var uiEntity in uiEntities)
                {
                    uiEntity.InsideBonusDescription = uiEntity.IsFixedOfInside
                              ? GlobalConst.DeptProductSalesBonusTypes.FIXED + "：" + string.Format("{0:P2}", uiEntity.FixedRatioOfInside)
                              : GlobalConst.DeptProductSalesBonusTypes.FLOATED;

                    uiEntity.OutsideBonusDescription = uiEntity.IsFixedOfOutside
                              ? GlobalConst.DeptProductSalesBonusTypes.FIXED + "：" + string.Format("{0:P2}", uiEntity.FixedRatioOfOutside)
                              : GlobalConst.DeptProductSalesBonusTypes.FLOATED;
                }
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
