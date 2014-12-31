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
    public class SalesOrderAppDetailRepository : BaseRepository<SalesOrderAppDetail>, ISalesOrderAppDetailRepository
    {
        public IList<UIDaBaoAppDetail> GetUIList(UISearchDaBaoAppDetail uiSearchObj = null)
        {
            IList<UIDaBaoAppDetail> uiEntities = new List<UIDaBaoAppDetail>();

            IQueryable<SalesOrderAppDetail> query = null;

            List<Expression<Func<SalesOrderAppDetail, bool>>> whereFuncs = new List<Expression<Func<SalesOrderAppDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.SalesOrderApplicationID > 0)
                    whereFuncs.Add(x => x.SalesOrderApplicationID == uiSearchObj.SalesOrderApplicationID);
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

            IQueryable<SalesOrderAppDetail> query = null;

            List<Expression<Func<SalesOrderAppDetail, bool>>> whereFuncs = new List<Expression<Func<SalesOrderAppDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.SalesOrderApplicationID > 0)
                    whereFuncs.Add(x => x.SalesOrderApplicationID == uiSearchObj.SalesOrderApplicationID);
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
