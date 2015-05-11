using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class SalesOrderAppDetailRepository : BaseRepository<SalesOrderAppDetail>, ISalesOrderAppDetailRepository
    {
        public IList<UISalesOrderAppDetail> GetUIList(UISearchSalesOrderAppDetail uiSearchObj = null)
        {
            IList<UISalesOrderAppDetail> uiEntities = new List<UISalesOrderAppDetail>();

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
                              join w in DB.Warehouse on q.WarehouseID equals w.ID into tempW
                              from tw in tempW.DefaultIfEmpty()
                              select new UISalesOrderAppDetail()
                              {
                                  ID = q.ID,
                                  ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  SalesPrice = q.SalesPrice,
                                  InvoicePrice = q.InvoicePrice,
                                  SalesQty = q.Count,
                                  TotalSalesAmount = q.TotalSalesAmount,
                                  GiftCount = q.GiftCount,
                                  NumberOfPackages = (decimal)(q.Count + q.GiftCount ?? 0) / (decimal)(ps.NumberInLargePackage.HasValue ? ps.NumberInLargePackage.Value : 1),
                                  Warehouse = tw == null ? string.Empty : tw.Name
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UISalesOrderAppDetail> GetUIList(UISearchSalesOrderAppDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISalesOrderAppDetail> uiEntities = new List<UISalesOrderAppDetail>();

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
                              join w in DB.Warehouse on q.WarehouseID equals w.ID into tempW
                              from tw in tempW.DefaultIfEmpty()
                              select new UISalesOrderAppDetail()
                              {
                                  ID = q.ID,
                                  ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  SalesPrice = q.SalesPrice,
                                  InvoicePrice = q.InvoicePrice,
                                  SalesQty = q.Count,
                                  TotalSalesAmount = q.TotalSalesAmount,
                                  GiftCount = q.GiftCount,
                                  NumberOfPackages = (decimal)(q.Count + q.GiftCount ?? 0) / (decimal)(ps.NumberInLargePackage.HasValue ? ps.NumberInLargePackage.Value : 1),
                                  Warehouse = tw == null ? string.Empty : tw.Name
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }

        public IList<UISalesOrderAppDetail> GetCanOutUIList(UISearchSalesOrderAppDetail uiSearchObj = null)
        {
            IList<UISalesOrderAppDetail> uiEntities = new List<UISalesOrderAppDetail>();

            IQueryable<SalesOrderAppDetail> query = null;

            List<Expression<Func<SalesOrderAppDetail, bool>>> whereFuncs = new List<Expression<Func<SalesOrderAppDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.SalesOrderApplicationID > 0)
                    whereFuncs.Add(x => x.SalesOrderApplicationID.Equals(uiSearchObj.SalesOrderApplicationID));

                if (uiSearchObj.WarehouseID > 0)
                    whereFuncs.Add(x => x.Product.StockInDetail.Any(y => y.WarehouseID == uiSearchObj.WarehouseID));

                if (uiSearchObj.SaleOrderTypeIDs != null
                    && uiSearchObj.SaleOrderTypeIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.SaleOrderTypeIDs.Contains(x.SalesOrderApplication.SaleOrderTypeID));

                if (uiSearchObj.DistributionCompanyID > 0)
                    whereFuncs.Add(x => x.SalesOrderApplication.DaBaoApplication.Any(y => y.DistributionCompanyID == uiSearchObj.DistributionCompanyID));

                if (uiSearchObj.IncludeIDs != null
                    && uiSearchObj.IncludeIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeIDs.Contains(x.ID));

                if (uiSearchObj.ExcludeIDs != null
                    && uiSearchObj.ExcludeIDs.Count() > 0)
                    whereFuncs.Add(x => !uiSearchObj.ExcludeIDs.Contains(x.ID));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join soa in DB.SalesOrderApplication on q.SalesOrderApplicationID equals soa.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join s in DB.Supplier on p.SupplierID equals s.ID into tempS
                              from ts in tempS.DefaultIfEmpty()
                              join um in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              join sid in DB.StockInDetail on new { q.ProductID, q.ProductSpecificationID, uiSearchObj.WarehouseID }
                              equals new { sid.ProductID, sid.ProductSpecificationID, sid.WarehouseID } into tempSID
                              from tsid in tempSID.DefaultIfEmpty()
                              join wh in DB.Warehouse on tsid.WarehouseID equals wh.ID into tempWH
                              from twh in tempWH.DefaultIfEmpty()
                              select new UISalesOrderAppDetail()
                              {
                                  ID = q.ID,
                                  SalesOrderApplicationID = q.SalesOrderApplicationID,
                                  ProductID = q.ProductID,
                                  ProductSpecificationID = q.ProductSpecificationID,
                                  WarehouseID = tsid == null ? 0 : tsid.WarehouseID,
                                  Warehouse = twh == null ? string.Empty : twh.Name,
                                  OrderCode = soa.OrderCode,
                                  ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  FactoryName = ts == null ? string.Empty : ts.FactoryName,
                                  SalesQty = q.Count,
                                  SalesPrice = q.SalesPrice,
                                  InvoicePrice = q.InvoicePrice,
                                  NumberInLargePackage = ps.NumberInLargePackage,
                                  NumberOfPackages = q.Count / (ps.NumberInLargePackage.HasValue ? ps.NumberInLargePackage.Value : 1),
                                  OutQty = DB.StockOutDetail.Any(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID)
                                   ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID).Sum(x => x.OutQty) : 0,
                                  ToBeOutQty = q.Count - (DB.StockOutDetail.Any(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID)
                                  ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID).Sum(x => x.OutQty) : 0),
                                  BalanceQty = (DB.StockInDetail.Any(x => x.IsDeleted == false && x.WarehouseID == uiSearchObj.WarehouseID && x.ProductID == q.ProductID
                                      && x.ProductSpecificationID == q.ProductSpecificationID) ? DB.StockInDetail.Where(x => x.IsDeleted == false && x.WarehouseID == uiSearchObj.WarehouseID
                                          && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID).Sum(x => x.InQty) : 0) -
                                    (DB.StockOutDetail.Any(x => x.IsDeleted == false && x.WarehouseID == uiSearchObj.WarehouseID && x.ProductID == q.ProductID
                                          && x.ProductSpecificationID == q.ProductSpecificationID) ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.WarehouseID == uiSearchObj.WarehouseID
                                              && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID).Sum(x => x.OutQty) : 0),
                                  TotalSalesAmount = q.TotalSalesAmount,
                                  BatchNumber = tsid == null ? string.Empty : tsid.BatchNumber,
                                  ExpirationDate = tsid == null ? null : tsid.ExpirationDate,
                                  LicenseNumber = tsid == null ? string.Empty : tsid.LicenseNumber
                              }).ToList();

            }

            return uiEntities;
        }

        public IList<UISalesOrderAppDetail> GetCanOutUIList(UISearchSalesOrderAppDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISalesOrderAppDetail> uiEntities = new List<UISalesOrderAppDetail>();

            int total = 0;

            IQueryable<SalesOrderAppDetail> query = null;

            List<Expression<Func<SalesOrderAppDetail, bool>>> whereFuncs = new List<Expression<Func<SalesOrderAppDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.SalesOrderApplicationID > 0)
                    whereFuncs.Add(x => x.SalesOrderApplicationID.Equals(uiSearchObj.SalesOrderApplicationID));

                if (uiSearchObj.WarehouseID != 0)
                    whereFuncs.Add(x => x.Product.StockInDetail.Any(y => y.WarehouseID == uiSearchObj.WarehouseID));

                if (uiSearchObj.SaleOrderTypeIDs != null
                    && uiSearchObj.SaleOrderTypeIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.SaleOrderTypeIDs.Contains(x.SalesOrderApplication.SaleOrderTypeID));

                if (uiSearchObj.DistributionCompanyID > 0)
                    whereFuncs.Add(x => x.SalesOrderApplication.DaBaoApplication.Any(y => y.DistributionCompanyID == uiSearchObj.DistributionCompanyID));

                if (uiSearchObj.ExcludeIDs != null
                    && uiSearchObj.ExcludeIDs.Count() > 0)
                    whereFuncs.Add(x => !uiSearchObj.ExcludeIDs.Contains(x.ID));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join soa in DB.SalesOrderApplication on q.SalesOrderApplicationID equals soa.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join s in DB.Supplier on p.SupplierID equals s.ID into tempS
                              from ts in tempS.DefaultIfEmpty()
                              join um in DB.UnitOfMeasurement on ps.UnitOfMeasurementID equals um.ID into tempUM
                              from tum in tempUM.DefaultIfEmpty()
                              join sid in DB.StockInDetail on new { q.ProductID, q.ProductSpecificationID, uiSearchObj.WarehouseID }
                              equals new { sid.ProductID, sid.ProductSpecificationID, sid.WarehouseID } into tempSID
                              from tsid in tempSID.DefaultIfEmpty()
                              join wh in DB.Warehouse on tsid.WarehouseID equals wh.ID into tempWH
                              from twh in tempWH.DefaultIfEmpty()
                              select new UISalesOrderAppDetail()
                              {
                                  ID = q.ID,
                                  SalesOrderApplicationID = q.SalesOrderApplicationID,
                                  ProductID = q.ProductID,
                                  ProductSpecificationID = q.ProductSpecificationID,
                                  WarehouseID = tsid == null ? 0 : tsid.WarehouseID,
                                  Warehouse = twh == null ? string.Empty : twh.Name,
                                  OrderCode = soa.OrderCode,
                                  ProductCode = p.ProductCode,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  UnitOfMeasurement = tum == null ? string.Empty : tum.UnitName,
                                  FactoryName = ts == null ? string.Empty : ts.FactoryName,
                                  SalesQty = q.Count,
                                  SalesPrice = q.SalesPrice,
                                  InvoicePrice = q.InvoicePrice,
                                  NumberInLargePackage = ps.NumberInLargePackage,
                                  NumberOfPackages = q.Count / (ps.NumberInLargePackage.HasValue ? ps.NumberInLargePackage.Value : 1),
                                  OutQty = DB.StockOutDetail.Any(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID)
                                   ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID).Sum(x => x.OutQty) : 0,
                                  ToBeOutQty = q.Count - (DB.StockOutDetail.Any(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID)
                                  ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID).Sum(x => x.OutQty) : 0),
                                  //BalanceQty = (DB.StockInDetail.Any(x => x.IsDeleted == false && x.WarehouseID == uiSearchObj.WarehouseID && x.ProductID == q.ProductID
                                  //    && x.ProductSpecificationID == q.ProductSpecificationID) ? DB.StockInDetail.Where(x => x.IsDeleted == false && x.WarehouseID == uiSearchObj.WarehouseID
                                  //        && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID).Sum(x => x.InQty) : 0) -
                                  //  (DB.StockOutDetail.Any(x => x.IsDeleted == false && x.WarehouseID == uiSearchObj.WarehouseID && x.ProductID == q.ProductID
                                  //        && x.ProductSpecificationID == q.ProductSpecificationID) ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.WarehouseID == uiSearchObj.WarehouseID
                                  //            && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID).Sum(x => x.OutQty) : 0),

                                  //BalanceQty = (from sid in DB.StockInDetail.Where(x => x.IsDeleted == false && x.ID == tsid.ID)
                                  //                   join sod in DB.StockOutDetail.Where(x => x.IsDeleted == false && x.WarehouseID == uiSearchObj.WarehouseID && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID)
                                  //                   on new { sid.ProductID, sid.ProductSpecificationID, sid.BatchNumber, sid.LicenseNumber }
                                  //                   equals new { sod.ProductID, sod.ProductSpecificationID, sod.BatchNumber, sod.LicenseNumber } into tempSOD
                                  //                   from tsod in tempSOD.DefaultIfEmpty()
                                  //                   select new { sid.InQty, OutQty = (tsod == null ? 0 : tsod.OutQty) })
                                  //                   .Sum(x => x.InQty - x.OutQty),

                                  BalanceQty = tsid == null ? 0 : tsid.InQty - (DB.StockOutDetail.Any(x => x.IsDeleted == false && x.WarehouseID == tsid.WarehouseID && x.ProductID == tsid.ProductID
                                      && x.ProductSpecificationID == tsid.ProductSpecificationID && x.BatchNumber == tsid.BatchNumber && x.LicenseNumber == tsid.LicenseNumber)
                                      ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.WarehouseID == tsid.WarehouseID && x.ProductID == tsid.ProductID
                                      && x.ProductSpecificationID == tsid.ProductSpecificationID && x.BatchNumber == tsid.BatchNumber && x.LicenseNumber == tsid.LicenseNumber).Sum(x => x.OutQty) : 0),

                                  TotalSalesAmount = q.TotalSalesAmount,
                                  BatchNumber = tsid == null ? string.Empty : tsid.BatchNumber,
                                  ExpirationDate = tsid == null ? null : tsid.ExpirationDate,
                                  LicenseNumber = tsid == null ? string.Empty : tsid.LicenseNumber
                              }).ToList();

            }

            totalRecords = total;

            return uiEntities;
        }


        public IList<UIToBeOutSalesOrderDetail> GetToBeOutUIList(UISearchToBeOutSalesOrderDetail uiSearchObj)
        {
            IList<UIToBeOutSalesOrderDetail> uiEntities = new List<UIToBeOutSalesOrderDetail>();

            IQueryable<SalesOrderAppDetail> query = null;

            List<Expression<Func<SalesOrderAppDetail, bool>>> whereFuncs = new List<Expression<Func<SalesOrderAppDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.SalesOrderApplicationID > 0)
                    whereFuncs.Add(x => x.SalesOrderApplicationID.Equals(uiSearchObj.SalesOrderApplicationID));

                if (uiSearchObj.SaleOrderTypeIDs != null
                    && uiSearchObj.SaleOrderTypeIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.SaleOrderTypeIDs.Contains(x.SalesOrderApplication.SaleOrderTypeID));

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.SalesOrderApplication.ClientSaleApplication.Any(y => y.ClientUserID == uiSearchObj.ClientUserID));

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.SalesOrderApplication.ClientSaleApplication.Any(y => y.ClientCompanyID == uiSearchObj.ClientCompanyID));

                if (uiSearchObj.DistributionCompanyID > 0)
                    whereFuncs.Add(x => x.SalesOrderApplication.DaBaoApplication.Any(y => y.DistributionCompanyID == uiSearchObj.DistributionCompanyID));

                if (uiSearchObj.ExcludeIDs != null
                    && uiSearchObj.ExcludeIDs.Count() > 0)
                    whereFuncs.Add(x => !uiSearchObj.ExcludeIDs.Contains(x.ID));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                //var statDate = DateTime.Now.Date.AddDays(-1);
                //var curBeginDate = DateTime.Now.Date;
                //var curEndDate = DateTime.Now.Date.AddDays(1);

                uiEntities = (from q in query
                              join soa in DB.SalesOrderApplication.Where(x => x.IsDeleted == false && x.IsStop == false)
                                on q.SalesOrderApplicationID equals soa.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              select new UIToBeOutSalesOrderDetail()
                              {
                                  ID = q.ID,
                                  SalesOrderApplicationID = q.SalesOrderApplicationID,
                                  ProductID = q.ProductID,
                                  ProductSpecificationID = q.ProductSpecificationID,
                                  OrderCode = soa.OrderCode,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  OutQty = DB.StockOutDetail.Any(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID)
                                   ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID).Sum(x => x.OutQty) : 0,

                                  ToBeOutQty = q.Count - (DB.StockOutDetail.Any(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID)
                                  ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID).Sum(x => x.OutQty) : 0),

                                  //BalanceQty = (DB.StockInDetail.Any(x => x.IsDeleted == false && x.ProductID == q.ProductID
                                  //    && x.ProductSpecificationID == q.ProductSpecificationID && x.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse)
                                  //    ? DB.StockInDetail.Where(x => x.IsDeleted == false && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID
                                  //        && x.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse).Sum(x => x.InQty) : 0) -
                                  //  (DB.StockOutDetail.Any(x => x.IsDeleted == false && x.ProductID == q.ProductID
                                  //        && x.ProductSpecificationID == q.ProductSpecificationID) ? DB.StockOutDetail.Where(x => x.IsDeleted == false
                                  //            && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID).Sum(x => x.OutQty) : 0),

                                  WarehouseData = ((from sid in DB.StockInDetail
                                                    join si in DB.StockIn on sid.StockInID equals si.ID
                                                    join w in DB.Warehouse on sid.WarehouseID equals w.ID
                                                    where si.IsDeleted == false && si.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse
                                                    && sid.IsDeleted == false && sid.ProductID == q.ProductID
                                                    && sid.ProductSpecificationID == q.ProductSpecificationID
                                                    && sid.ExpirationDate > DateTime.Now //过期的货品不能出库
                                                    select new
                                                    {
                                                        sid.ProductID,
                                                        sid.ProductSpecificationID,
                                                        sid.WarehouseID,
                                                        WarehouseName = w.Name,
                                                        sid.InQty
                                                    })
                                                   .GroupBy(x => new { x.ProductID, x.ProductSpecificationID, x.WarehouseID, x.WarehouseName })
                                                   .Select(g => new
                                                   {
                                                       WarehouseKey = g.Key,
                                                       BalanceQty = g.Sum(x => x.InQty) -
                                                           (DB.StockOutDetail.Any(x => x.IsDeleted == false && x.ProductID == g.Key.ProductID
                                                               && x.ProductSpecificationID == g.Key.ProductSpecificationID
                                                               && x.WarehouseID == g.Key.WarehouseID) ?
                                                           DB.StockOutDetail.Where(x => x.IsDeleted == false && x.ProductID == g.Key.ProductID
                                                               && x.ProductSpecificationID == g.Key.ProductSpecificationID
                                                               && x.WarehouseID == g.Key.WarehouseID).Sum(x => x.OutQty) : 0)
                                                   })
                                                   .Where(x => x.BalanceQty > 0))
                                                   .Select(x => new UIDropdownItem
                                                   {
                                                       ItemValue = x.WarehouseKey.WarehouseID,
                                                       ItemText = x.WarehouseKey.WarehouseName,
                                                       Extension = new { BalanceQty = x.BalanceQty }
                                                   }).ToList()
                              })
                              .Where(x => x.WarehouseData.Count > 0 && x.ToBeOutQty > 0)
                              .ToList();
            }

            return uiEntities;
        }

        public IList<UIToBeOutSalesOrderDetail> GetToBeOutUIList(UISearchToBeOutSalesOrderDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIToBeOutSalesOrderDetail> uiEntities = new List<UIToBeOutSalesOrderDetail>();

            int total = 0;

            IQueryable<SalesOrderAppDetail> query = null;

            List<Expression<Func<SalesOrderAppDetail, bool>>> whereFuncs = new List<Expression<Func<SalesOrderAppDetail, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.SalesOrderApplicationID > 0)
                    whereFuncs.Add(x => x.SalesOrderApplicationID.Equals(uiSearchObj.SalesOrderApplicationID));

                if (uiSearchObj.SaleOrderTypeIDs != null
                    && uiSearchObj.SaleOrderTypeIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.SaleOrderTypeIDs.Contains(x.SalesOrderApplication.SaleOrderTypeID));

                if (uiSearchObj.DistributionCompanyID > 0)
                    whereFuncs.Add(x => x.SalesOrderApplication.DaBaoApplication.Any(y => y.DistributionCompanyID == uiSearchObj.DistributionCompanyID));

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.SalesOrderApplication.ClientSaleApplication.Any(y => y.ClientUserID == uiSearchObj.ClientUserID));

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.SalesOrderApplication.ClientSaleApplication.Any(y => y.ClientCompanyID == uiSearchObj.ClientCompanyID));

                if (uiSearchObj.ExcludeIDs != null
                    && uiSearchObj.ExcludeIDs.Count() > 0)
                    whereFuncs.Add(x => !uiSearchObj.ExcludeIDs.Contains(x.ID));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            totalRecords = total;

            if (query != null)
            {
                //var statDate = DateTime.Now.Date.AddDays(-1);
                //var curBeginDate = DateTime.Now.Date;
                //var curEndDate = DateTime.Now.Date.AddDays(1);

                uiEntities = (from q in query
                              join soa in DB.SalesOrderApplication.Where(x => x.IsDeleted == false && x.IsStop == false)
                                on q.SalesOrderApplicationID equals soa.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              select new UIToBeOutSalesOrderDetail()
                              {
                                  ID = q.ID,
                                  SalesOrderApplicationID = q.SalesOrderApplicationID,
                                  ProductID = q.ProductID,
                                  ProductSpecificationID = q.ProductSpecificationID,
                                  OrderCode = soa.OrderCode,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  OutQty = DB.StockOutDetail.Any(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID)
                                   ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID).Sum(x => x.OutQty) : 0,

                                  ToBeOutQty = (q.Count + (q.GiftCount.HasValue ? q.GiftCount.Value : 0)) - (DB.StockOutDetail.Any(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID)
                                  ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.SalesOrderAppDetailID == q.ID).Sum(x => x.OutQty) : 0),

                                  //BalanceQty = (DB.StockInDetail.Any(x => x.IsDeleted == false && x.ProductID == q.ProductID 
                                  //    && x.ProductSpecificationID == q.ProductSpecificationID && x.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse)
                                  //    ? DB.StockInDetail.Where(x => x.IsDeleted == false && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID
                                  //        && x.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse).Sum(x => x.InQty) : 0) -
                                  //        (DB.StockOutDetail.Any(x => x.IsDeleted == false && x.ProductID == q.ProductID
                                  //            && x.ProductSpecificationID == q.ProductSpecificationID) ? DB.StockOutDetail.Where(x => x.IsDeleted == false
                                  //                && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID).Sum(x => x.OutQty) : 0),

                                  //BalanceQty = DB.InventoryHistory.Any(x => x.StatDate == statDate && x.ProductID == q.ProductID
                                  //    && x.ProductSpecificationID == q.ProductSpecificationID)
                                  //    ? (DB.InventoryHistory.Where(x => x.StatDate == statDate && x.ProductID == q.ProductID
                                  //        && x.ProductSpecificationID == q.ProductSpecificationID).Sum(x => x.BalanceQty)
                                  //        +
                                  //        (DB.StockInDetail.Any(x => x.IsDeleted == false && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID
                                  //            && x.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse && x.StockIn.EntryDate >= curBeginDate && x.StockIn.EntryDate < curEndDate)
                                  //            ? DB.StockInDetail.Where(x => x.IsDeleted == false && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID
                                  //                && x.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse && x.StockIn.EntryDate >= curBeginDate && x.StockIn.EntryDate < curEndDate)
                                  //                .Sum(x => x.InQty) : 0)
                                  //                -
                                  //                (DB.StockOutDetail.Any(x => x.IsDeleted == false && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID
                                  //                    && x.CreatedOn >= curBeginDate && x.CreatedOn < curEndDate)
                                  //                    ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID
                                  //                        && x.CreatedOn >= curBeginDate && x.CreatedOn < curEndDate).Sum(x => x.OutQty) : 0)
                                  //        )
                                  //    : ((DB.StockInDetail.Any(x => x.IsDeleted == false && x.ProductID == q.ProductID
                                  //        && x.ProductSpecificationID == q.ProductSpecificationID && x.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse)
                                  //        ? DB.StockInDetail.Where(x => x.IsDeleted == false && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID
                                  //            && x.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse).Sum(x => x.InQty) : 0) -
                                  //            (DB.StockOutDetail.Any(x => x.IsDeleted == false && x.ProductID == q.ProductID
                                  //                && x.ProductSpecificationID == q.ProductSpecificationID) ? DB.StockOutDetail.Where(x => x.IsDeleted == false
                                  //                    && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID).Sum(x => x.OutQty) : 0)),

                                  WarehouseData = (from sid in DB.StockInDetail
                                                   join si in DB.StockIn on sid.StockInID equals si.ID
                                                   join w in DB.Warehouse on sid.WarehouseID equals w.ID
                                                   where si.IsDeleted == false && si.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse
                                                   && sid.IsDeleted == false && sid.ProductID == q.ProductID
                                                   && sid.ProductSpecificationID == q.ProductSpecificationID
                                                   && sid.ExpirationDate > DateTime.Now //过期的货品不能出库
                                                   select new
                                                   {
                                                       sid.ProductID,
                                                       sid.ProductSpecificationID,
                                                       sid.WarehouseID,
                                                       WarehouseName = w.Name,
                                                       sid.InQty
                                                   })
                                                   .GroupBy(x => new { x.ProductID, x.ProductSpecificationID, x.WarehouseID, x.WarehouseName })
                                                   .Select(g => new
                                                   {
                                                       WarehouseKey = g.Key,
                                                       BalanceQty = g.Sum(x => x.InQty) -
                                                           (DB.StockOutDetail.Any(x => x.IsDeleted == false && x.ProductID == g.Key.ProductID
                                                               && x.ProductSpecificationID == g.Key.ProductSpecificationID
                                                               && x.WarehouseID == g.Key.WarehouseID) ?
                                                           DB.StockOutDetail.Where(x => x.IsDeleted == false && x.ProductID == g.Key.ProductID
                                                               && x.ProductSpecificationID == g.Key.ProductSpecificationID
                                                               && x.WarehouseID == g.Key.WarehouseID).Sum(x => x.OutQty) : 0)
                                                   })
                                                   .Where(x => x.BalanceQty > 0)
                                                   .Select(x => new UIDropdownItem
                                                   {
                                                       ItemValue = x.WarehouseKey.WarehouseID,
                                                       ItemText = x.WarehouseKey.WarehouseName,
                                                       Extension = new { BalanceQty = x.BalanceQty }
                                                   }).ToList()
                              })
                              .Where(x => x.WarehouseData.Count > 0 && x.ToBeOutQty > 0)
                              .ToList();

                totalRecords = uiEntities.Count;
            }

            return uiEntities;
        }
    }
}
