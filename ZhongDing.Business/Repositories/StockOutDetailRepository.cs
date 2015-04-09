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
    public class StockOutDetailRepository : BaseRepository<StockOutDetail>, IStockOutDetailRepository
    {
        public IList<UIStockOutDetail> GetUIList(UISearchStockOutDetail uiSearchObj = null)
        {
            IList<UIStockOutDetail> uiEntities = new List<UIStockOutDetail>();

            IQueryable<StockOutDetail> query = null;

            List<Expression<Func<StockOutDetail, bool>>> whereFuncs = new List<Expression<Func<StockOutDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.StockOutID > 0)
                    whereFuncs.Add(x => x.StockOutID == uiSearchObj.StockOutID);

                if (uiSearchObj.WarehouseID > 0)
                    whereFuncs.Add(x => x.WarehouseID == uiSearchObj.WarehouseID);

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID == uiSearchObj.ProductID);

                if (uiSearchObj.DistributionCompanyID > 0)
                    whereFuncs.Add(x => x.StockOut.DistributionCompanyID == uiSearchObj.DistributionCompanyID);
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join so in DB.StockOut on q.StockOutID equals so.ID
                              join sop in DB.SalesOrderApplication on q.SalesOrderApplicationID equals sop.ID
                              join soad in DB.SalesOrderAppDetail on q.SalesOrderAppDetailID equals soad.ID
                              join w in DB.Warehouse on q.WarehouseID equals w.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join s in DB.Supplier on p.SupplierID equals s.ID into tempS
                              from ts in tempS.DefaultIfEmpty()
                              join um in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              select new UIStockOutDetail()
                              {
                                  ID = q.ID,
                                  StockOutID = q.StockOutID,
                                  SalesOrderApplicationID = q.SalesOrderApplicationID,
                                  SalesOrderAppDetailID = q.SalesOrderAppDetailID,
                                  ProductID = q.ProductID,
                                  ProductSpecificationID = q.ProductSpecificationID,
                                  OrderCode = sop.OrderCode,
                                  WarehouseID = q.WarehouseID,
                                  Warehouse = w.Name,
                                  //ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  FactoryName = ts == null ? string.Empty : ts.FactoryName,
                                  SalesQty = soad.Count,
                                  SalesPrice = soad.SalesPrice,
                                  OutQty = q.OutQty,
                                  TaxQty = q.TaxQty,
                                  TotalSalesAmount = q.TotalSalesAmount,
                                  ToBeOutQty = soad.Count - q.OutQty,
                                  NumberInLargePackage = ps.NumberInLargePackage,
                                  NumberOfPackages = q.OutQty / (ps.NumberInLargePackage.HasValue ? ps.NumberInLargePackage.Value : 1),
                                  BalanceQty = (DB.StockInDetail.Any(x => x.IsDeleted == false && x.WarehouseID == q.WarehouseID && x.ProductID == q.ProductID
                                      && x.ProductSpecificationID == q.ProductSpecificationID) ? DB.StockInDetail.Where(x => x.IsDeleted == false && x.WarehouseID == q.WarehouseID
                                          && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID).Sum(x => x.InQty) : 0) -
                                          (DB.StockOutDetail.Any(x => x.IsDeleted == false && x.WarehouseID == q.WarehouseID && x.ProductID == q.ProductID
                                          && x.ProductSpecificationID == q.ProductSpecificationID && x.ID != q.ID) ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.WarehouseID == q.WarehouseID
                                              && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID && x.ID != q.ID).Sum(x => x.OutQty) : 0),
                                  BatchNumber = q.BatchNumber,
                                  ExpirationDate = q.ExpirationDate,
                                  LicenseNumber = q.LicenseNumber
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIStockOutDetail> GetUIList(UISearchStockOutDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIStockOutDetail> uiEntities = new List<UIStockOutDetail>();

            int total = 0;

            IQueryable<StockOutDetail> query = null;

            List<Expression<Func<StockOutDetail, bool>>> whereFuncs = new List<Expression<Func<StockOutDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.StockOutID > 0)
                    whereFuncs.Add(x => x.StockOutID == uiSearchObj.StockOutID);

                if (uiSearchObj.WarehouseID > 0)
                    whereFuncs.Add(x => x.WarehouseID == uiSearchObj.WarehouseID);

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID == uiSearchObj.ProductID);

                if (uiSearchObj.DistributionCompanyID > 0)
                    whereFuncs.Add(x => x.StockOut.DistributionCompanyID == uiSearchObj.DistributionCompanyID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join so in DB.StockOut on q.StockOutID equals so.ID
                              join sop in DB.SalesOrderApplication on q.SalesOrderApplicationID equals sop.ID
                              join soad in DB.SalesOrderAppDetail on q.SalesOrderAppDetailID equals soad.ID
                              join w in DB.Warehouse on q.WarehouseID equals w.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join s in DB.Supplier on p.SupplierID equals s.ID into tempS
                              from ts in tempS.DefaultIfEmpty()
                              join um in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              select new UIStockOutDetail()
                              {
                                  ID = q.ID,
                                  StockOutID = q.StockOutID,
                                  SalesOrderApplicationID = q.SalesOrderApplicationID,
                                  SalesOrderAppDetailID = q.SalesOrderAppDetailID,
                                  ProductID = q.ProductID,
                                  ProductSpecificationID = q.ProductSpecificationID,
                                  OrderCode = sop.OrderCode,
                                  WarehouseID = q.WarehouseID,
                                  Warehouse = w.Name,
                                  //ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  FactoryName = ts == null ? string.Empty : ts.FactoryName,
                                  SalesQty = soad.Count,
                                  SalesPrice = soad.SalesPrice,
                                  OutQty = q.OutQty,
                                  TaxQty = q.TaxQty,
                                  TotalSalesAmount = q.TotalSalesAmount,
                                  ToBeOutQty = soad.Count - q.OutQty,
                                  NumberInLargePackage = ps.NumberInLargePackage,
                                  NumberOfPackages = q.OutQty / (ps.NumberInLargePackage.HasValue ? ps.NumberInLargePackage.Value : 1),
                                  BalanceQty = (DB.StockInDetail.Any(x => x.IsDeleted == false && x.WarehouseID == q.WarehouseID && x.ProductID == q.ProductID
                                      && x.ProductSpecificationID == q.ProductSpecificationID) ? DB.StockInDetail.Where(x => x.IsDeleted == false && x.WarehouseID == q.WarehouseID
                                          && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID).Sum(x => x.InQty) : 0) -
                                          (DB.StockOutDetail.Any(x => x.IsDeleted == false && x.WarehouseID == q.WarehouseID && x.ProductID == q.ProductID
                                          && x.ProductSpecificationID == q.ProductSpecificationID) ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.WarehouseID == q.WarehouseID
                                              && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID).Sum(x => x.OutQty) : 0),
                                  BatchNumber = q.BatchNumber,
                                  ExpirationDate = q.ExpirationDate,
                                  LicenseNumber = q.LicenseNumber
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }

        public IList<UIStockOutDetail> GetUIListForSmsReminder(UISearchStockOutDetail uiSearchObj)
        {
            IList<UIStockOutDetail> uiEntities = new List<UIStockOutDetail>();

            IQueryable<StockOutDetail> query = null;

            List<Expression<Func<StockOutDetail, bool>>> whereFuncs = new List<Expression<Func<StockOutDetail, bool>>>();
            if (uiSearchObj != null)
            {
                if (uiSearchObj.StockOutID > 0)
                    whereFuncs.Add(x => x.StockOutID == uiSearchObj.StockOutID);
            }
            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join um in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              select new UIStockOutDetail()
                              {
                                  ID = q.ID,
                                  ProductName = p.ProductName,
                                  NumberOfPackages = q.OutQty / (ps.NumberInLargePackage.HasValue ? ps.NumberInLargePackage.Value : 1),
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                              }).ToList();
            }

            return uiEntities;
        }


        public IList<UIStockOutDetail> GetClientInvoiceChooseStockOutDetailUIList(UISearchStockOutDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIStockOutDetail> uiEntities = new List<UIStockOutDetail>();

            int total = 0;

            IQueryable<StockOutDetail> query = null;

            List<Expression<Func<StockOutDetail, bool>>> whereFuncs = new List<Expression<Func<StockOutDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.StockOutID > 0)
                    whereFuncs.Add(x => x.StockOutID == uiSearchObj.StockOutID);

                if (uiSearchObj.WarehouseID > 0)
                    whereFuncs.Add(x => x.WarehouseID == uiSearchObj.WarehouseID);

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID == uiSearchObj.ProductID);

                if (uiSearchObj.DistributionCompanyID > 0)
                    whereFuncs.Add(x => x.StockOut.DistributionCompanyID == uiSearchObj.DistributionCompanyID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join so in DB.StockOut on q.StockOutID equals so.ID
                              join sop in DB.SalesOrderApplication on q.SalesOrderApplicationID equals sop.ID
                              join soad in DB.SalesOrderAppDetail on q.SalesOrderAppDetailID equals soad.ID
                              join w in DB.Warehouse on q.WarehouseID equals w.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join s in DB.Supplier on p.SupplierID equals s.ID into tempS
                              from ts in tempS.DefaultIfEmpty()
                              join um in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              join clientUser in DB.ClientUser on so.ClientUserID equals clientUser.ID into tempClientUsers
                              from tempClientUser in tempClientUsers.DefaultIfEmpty()
                              select new UIStockOutDetail()
                              {
                                  ID = q.ID,
                                  StockOutID = q.StockOutID,
                                  SalesOrderApplicationID = q.SalesOrderApplicationID,
                                  SalesOrderAppDetailID = q.SalesOrderAppDetailID,
                                  ProductID = q.ProductID,
                                  ProductSpecificationID = q.ProductSpecificationID,
                                  OrderCode = sop.OrderCode,
                                  WarehouseID = q.WarehouseID,
                                  Warehouse = w.Name,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  FactoryName = ts == null ? string.Empty : ts.FactoryName,
                                  SalesQty = soad.Count,
                                  SalesPrice = soad.SalesPrice,
                                  OutQty = q.OutQty,
                                  TaxQty = q.TaxQty,
                                  TotalSalesAmount = q.TotalSalesAmount,
                                  ToBeOutQty = soad.Count - q.OutQty,
                                  NumberInLargePackage = ps.NumberInLargePackage,
                                  NumberOfPackages = q.OutQty / (ps.NumberInLargePackage.HasValue ? ps.NumberInLargePackage.Value : 1),
                                  BatchNumber = q.BatchNumber,
                                  ExpirationDate = q.ExpirationDate,
                                  LicenseNumber = q.LicenseNumber,
                                  ClientName = tempClientUser == null ? "" : tempClientUser.ClientName,
                                  ClientInvoiceDetailTotalAmount = DB.ClientInvoiceDetail.Any(x => x.IsDeleted == false && x.StockOutDetailID == q.ID) ?
                                  DB.ClientInvoiceDetail.Where(x => x.IsDeleted == false && x.StockOutDetailID == q.ID).Sum(x => x.Amount) : 0,
                              }).ToList();
            }

            totalRecords = total;
            uiEntities.ForEach(x =>
            {
                x.NotTaxQty = x.TaxQty.Value - (int)(x.ClientInvoiceDetailTotalAmount / x.SalesPrice);

            });
            return uiEntities;
        }
    }
}
