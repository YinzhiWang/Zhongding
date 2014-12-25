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
    public class StockInDetailRepository : BaseRepository<StockInDetail>, IStockInDetailRepository
    {
        public IList<UIStockInDetail> GetUIList(UISearchStockInDetail uiSearchObj = null)
        {
            IList<UIStockInDetail> uiEntities = new List<UIStockInDetail>();

            IQueryable<StockInDetail> query = null;

            List<Expression<Func<StockInDetail, bool>>> whereFuncs = new List<Expression<Func<StockInDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.StockInID > 0)
                    whereFuncs.Add(x => x.StockInID == uiSearchObj.StockInID);

                if (uiSearchObj.WarehouseID > 0)
                    whereFuncs.Add(x => x.WarehouseID == uiSearchObj.WarehouseID);

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID == uiSearchObj.ProductID);
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join si in DB.StockIn on q.StockInID equals si.ID
                              join po in DB.ProcureOrderApplication on q.ProcureOrderAppID equals po.ID
                              join pod in DB.ProcureOrderAppDetail on q.ProcureOrderAppDetailID equals pod.ID
                              join w in DB.Warehouse on q.WarehouseID equals w.ID
                              join s in DB.Supplier on si.SupplierID equals s.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join um in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              select new UIStockInDetail()
                              {
                                  ID = q.ID,
                                  StockInID = q.StockInID,
                                  ProcureOrderAppID = q.ProcureOrderAppID,
                                  ProcureOrderAppDetailID = q.ProcureOrderAppDetailID,
                                  ProductID = q.ProductID,
                                  ProductSpecificationID = q.ProductSpecificationID,
                                  OrderCode = po.OrderCode,
                                  WarehouseID = q.WarehouseID,
                                  Warehouse = w.Name,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  FactoryName = s.FactoryName,
                                  ProcureCount = pod.ProcureCount,
                                  ProcurePrice = pod.ProcurePrice,
                                  InQty = q.InQty,
                                  ToBeInQty = pod.ProcureCount - q.InQty,
                                  NumberOfPackages = q.InQty / (ps.NumberInLargePackage.HasValue ? ps.NumberInLargePackage.Value : 1),
                                  BatchNumber = q.BatchNumber,
                                  ExpirationDate = q.ExpirationDate,
                                  LicenseNumber = q.LicenseNumber,
                                  IsMortgagedProduct = q.IsMortgagedProduct
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIStockInDetail> GetUIList(UISearchStockInDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIStockInDetail> uiEntities = new List<UIStockInDetail>();

            int total = 0;

            IQueryable<StockInDetail> query = null;

            List<Expression<Func<StockInDetail, bool>>> whereFuncs = new List<Expression<Func<StockInDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.StockInID > 0)
                    whereFuncs.Add(x => x.StockInID == uiSearchObj.StockInID);

                if (uiSearchObj.WarehouseID > 0)
                    whereFuncs.Add(x => x.WarehouseID == uiSearchObj.WarehouseID);

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID == uiSearchObj.ProductID);

                if (uiSearchObj.SupplierID > 0)
                    whereFuncs.Add(x => x.ProcureOrderApplication.SupplierID == uiSearchObj.SupplierID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join si in DB.StockIn on q.StockInID equals si.ID
                              join po in DB.ProcureOrderApplication on q.ProcureOrderAppID equals po.ID
                              join pod in DB.ProcureOrderAppDetail on q.ProcureOrderAppDetailID equals pod.ID
                              join w in DB.Warehouse on q.WarehouseID equals w.ID
                              join s in DB.Supplier on si.SupplierID equals s.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join um in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              select new UIStockInDetail()
                              {
                                  ID = q.ID,
                                  StockInID = q.StockInID,
                                  ProcureOrderAppID = q.ProcureOrderAppID,
                                  ProcureOrderAppDetailID = q.ProcureOrderAppDetailID,
                                  ProductID = q.ProductID,
                                  ProductSpecificationID = q.ProductSpecificationID,
                                  OrderCode = po.OrderCode,
                                  WarehouseID = q.WarehouseID,
                                  Warehouse = w.Name,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  FactoryName = s.FactoryName,
                                  ProcureCount = pod.ProcureCount,
                                  ProcurePrice = pod.ProcurePrice,
                                  InQty = q.InQty,
                                  ToBeInQty = pod.ProcureCount - q.InQty,
                                  NumberOfPackages = q.InQty / (ps.NumberInLargePackage.HasValue ? ps.NumberInLargePackage.Value : 1),
                                  BatchNumber = q.BatchNumber,
                                  ExpirationDate = q.ExpirationDate,
                                  LicenseNumber = q.LicenseNumber,
                                  IsMortgagedProduct = q.IsMortgagedProduct
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
