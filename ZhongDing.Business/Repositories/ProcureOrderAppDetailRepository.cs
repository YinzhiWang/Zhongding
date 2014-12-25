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
    public class ProcureOrderAppDetailRepository : BaseRepository<ProcureOrderAppDetail>, IProcureOrderAppDetailRepository
    {

        public IList<UIProcureOrderAppDetail> GetUIList(UISearchProcureOrderAppDetail uiSearchObj = null)
        {
            IList<UIProcureOrderAppDetail> uiEntities = new List<UIProcureOrderAppDetail>();

            IQueryable<ProcureOrderAppDetail> query = null;

            List<Expression<Func<ProcureOrderAppDetail, bool>>> whereFuncs = new List<Expression<Func<ProcureOrderAppDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ProcureOrderAppID > 0)
                    whereFuncs.Add(x => x.ProcureOrderApplicationID.Equals(uiSearchObj.ProcureOrderAppID));

                if (uiSearchObj.WarehouseID > 0)
                    whereFuncs.Add(x => x.WarehouseID.Equals(uiSearchObj.WarehouseID));

                if (uiSearchObj.SupplierID > 0)
                    whereFuncs.Add(x => x.ProcureOrderApplication.SupplierID == uiSearchObj.SupplierID);

                if (uiSearchObj.ExcludeIDs != null
                    && uiSearchObj.ExcludeIDs.Count() > 0)
                    whereFuncs.Add(x => !uiSearchObj.ExcludeIDs.Contains(x.ID));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join po in DB.ProcureOrderApplication on q.ProcureOrderApplicationID equals po.ID
                              join s in DB.Supplier on po.SupplierID equals s.ID
                              join w in DB.Warehouse on q.WarehouseID equals w.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join um in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              select new UIProcureOrderAppDetail()
                              {
                                  ID = q.ID,
                                  ProcureOrderAppID = q.ProcureOrderApplicationID,
                                  ProductID = q.ProductID,
                                  ProductSpecificationID = q.ProductSpecificationID,
                                  WarehouseID = q.WarehouseID,
                                  OrderCode = po.OrderCode,
                                  Warehouse = w.Name,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  FactoryName = s.FactoryName,
                                  ProcureCount = q.ProcureCount,
                                  ProcurePrice = q.ProcurePrice,
                                  NumberOfPackages = (decimal)q.ProcureCount / (decimal)(ps.NumberInLargePackage.HasValue ? ps.NumberInLargePackage.Value : 1),
                                  InQty = DB.StockInDetail.Any(x => x.IsDeleted == false && x.ProcureOrderAppDetailID == q.ID)
                                  ? DB.StockInDetail.Where(x => x.IsDeleted == false && x.ProcureOrderAppDetailID == q.ID).Sum(x => x.InQty) : 0,
                                  ToBeInQty = q.ProcureCount - (DB.StockInDetail.Any(x => x.IsDeleted == false && x.ProcureOrderAppDetailID == q.ID)
                                  ? DB.StockInDetail.Where(x => x.IsDeleted == false && x.ProcureOrderAppDetailID == q.ID).Sum(x => x.InQty) : 0),
                                  TotalAmount = q.TotalAmount,
                                  TaxAmount = q.TaxAmount
                              }).ToList();

            }

            return uiEntities;
        }

        public IList<UIProcureOrderAppDetail> GetUIList(UISearchProcureOrderAppDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIProcureOrderAppDetail> uiEntities = new List<UIProcureOrderAppDetail>();

            int total = 0;

            IQueryable<ProcureOrderAppDetail> query = null;

            List<Expression<Func<ProcureOrderAppDetail, bool>>> whereFuncs = new List<Expression<Func<ProcureOrderAppDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ProcureOrderAppID > 0)
                    whereFuncs.Add(x => x.ProcureOrderApplicationID.Equals(uiSearchObj.ProcureOrderAppID));

                if (uiSearchObj.WarehouseID > 0)
                    whereFuncs.Add(x => x.WarehouseID.Equals(uiSearchObj.WarehouseID));

                if (uiSearchObj.SupplierID > 0)
                    whereFuncs.Add(x => x.ProcureOrderApplication.SupplierID == uiSearchObj.SupplierID);

                if (uiSearchObj.ExcludeIDs != null
                    && uiSearchObj.ExcludeIDs.Count() > 0)
                    whereFuncs.Add(x => !uiSearchObj.ExcludeIDs.Contains(x.ID));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join po in DB.ProcureOrderApplication on q.ProcureOrderApplicationID equals po.ID
                              join s in DB.Supplier on po.SupplierID equals s.ID
                              join w in DB.Warehouse on q.WarehouseID equals w.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join um in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              select new UIProcureOrderAppDetail()
                              {
                                  ID = q.ID,
                                  ProcureOrderAppID = q.ProcureOrderApplicationID,
                                  ProductID = q.ProductID,
                                  ProductSpecificationID = q.ProductSpecificationID,
                                  WarehouseID = q.WarehouseID,
                                  OrderCode = po.OrderCode,
                                  Warehouse = w.Name,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  FactoryName = s.FactoryName,
                                  ProcureCount = q.ProcureCount,
                                  ProcurePrice = q.ProcurePrice,
                                  NumberOfPackages = (decimal)q.ProcureCount / (decimal)(ps.NumberInLargePackage.HasValue ? ps.NumberInLargePackage.Value : 1),
                                  InQty = DB.StockInDetail.Any(x => x.IsDeleted == false && x.ProcureOrderAppDetailID == q.ID)
                                  ? DB.StockInDetail.Where(x => x.IsDeleted == false && x.ProcureOrderAppDetailID == q.ID).Sum(x => x.InQty) : 0,
                                  ToBeInQty = q.ProcureCount - (DB.StockInDetail.Any(x => x.IsDeleted == false && x.ProcureOrderAppDetailID == q.ID)
                                  ? DB.StockInDetail.Where(x => x.IsDeleted == false && x.ProcureOrderAppDetailID == q.ID).Sum(x => x.InQty) : 0),
                                  TotalAmount = q.TotalAmount,
                                  TaxAmount = q.TaxAmount
                              }).ToList();

            }

            totalRecords = total;

            return uiEntities;
        }

    }
}
