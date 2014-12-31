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
    public class DaBaoRequestAppDetailRepository : BaseRepository<DaBaoRequestAppDetail>, IDaBaoRequestAppDetailRepository
    {
        public IList<UIDaBaoAppDetail> GetUIList(UISearchDaBaoAppDetail uiSearchObj = null)
        {
            IList<UIDaBaoAppDetail> uiEntities = new List<UIDaBaoAppDetail>();

            IQueryable<DaBaoRequestAppDetail> query = null;

            List<Expression<Func<DaBaoRequestAppDetail, bool>>> whereFuncs = new List<Expression<Func<DaBaoRequestAppDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DaBaoRequestApplicationID > 0)
                    whereFuncs.Add(x => x.DaBaoRequestApplicationID == uiSearchObj.DaBaoRequestApplicationID);
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join um in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              select new UIDaBaoAppDetail()
                              {
                                  ID = q.ID,
                                  ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  SalesPrice = q.SalesPrice,
                                  Count = q.Count,
                                  TotalSalesAmount = q.TotalSalesAmount
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIDaBaoAppDetail> GetUIList(UISearchDaBaoAppDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDaBaoAppDetail> uiEntities = new List<UIDaBaoAppDetail>();

            int total = 0;

            IQueryable<DaBaoRequestAppDetail> query = null;

            List<Expression<Func<DaBaoRequestAppDetail, bool>>> whereFuncs = new List<Expression<Func<DaBaoRequestAppDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DaBaoRequestApplicationID > 0)
                    whereFuncs.Add(x => x.DaBaoRequestApplicationID == uiSearchObj.DaBaoRequestApplicationID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join um in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              select new UIDaBaoAppDetail()
                              {
                                  ID = q.ID,
                                  ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  SalesPrice = q.SalesPrice,
                                  Count = q.Count,
                                  TotalSalesAmount = q.TotalSalesAmount
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
