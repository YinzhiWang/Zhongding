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
using ZhongDing.Common.Extension;

namespace ZhongDing.Business.Repositories
{
    public class InventoryHistoryRepository : BaseRepository<InventoryHistory>, IInventoryHistoryRepository
    {
        public int GetCount()
        {
            return DB.InventoryHistory.Count();
        }

        public List<UIInventoryHistory> CalculateInventoryByStatDate(DateTime statDate)
        {
            List<UIInventoryHistory> inventoryHistories = new List<UIInventoryHistory>();

            var isFirstCalculate = GetCount() == 0;

            DateTime yesterdayDate = statDate.Date.AddDays(-1);
            DateTime beginDate = statDate.Date;
            DateTime endDate = beginDate.AddDays(1);

            var query1 = (from sid in DB.StockInDetail
                          join si in DB.StockIn on sid.StockInID equals si.ID
                          where si.IsDeleted == false && si.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse
                                  && sid.IsDeleted == false && ((isFirstCalculate && si.EntryDate < endDate)
                                        || (!isFirstCalculate && si.EntryDate >= beginDate && si.EntryDate < endDate))
                          select new
                          {
                              sid.WarehouseID,
                              sid.ProductID,
                              sid.ProductSpecificationID,
                              sid.BatchNumber,
                              sid.LicenseNumber,
                              sid.ExpirationDate,
                          });

            var query2 = (from sod in DB.StockOutDetail
                          join so in DB.StockOut on sod.StockOutID equals so.ID
                          where so.IsDeleted == false && so.WorkflowStatusID == (int)EWorkflowStatus.OutWarehouse
                                && sod.IsDeleted == false && ((isFirstCalculate && so.OutDate < endDate)
                                    || (!isFirstCalculate && so.OutDate >= beginDate && so.OutDate < endDate))
                          select new
                          {
                              sod.WarehouseID,
                              sod.ProductID,
                              sod.ProductSpecificationID,
                              sod.BatchNumber,
                              sod.LicenseNumber,
                              sod.ExpirationDate,
                          });

            var query = query1.Union(query2);

            inventoryHistories = query.GroupBy(x => new { x.WarehouseID, x.ProductID, x.ProductSpecificationID, x.BatchNumber, x.LicenseNumber, x.ExpirationDate })
                                      .Select(g => new
                                      {
                                          Key = g.Key,
                                          TotalInQty = DB.StockInDetail.Any(y => y.IsDeleted == false && y.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse
                                          && ((isFirstCalculate && y.StockIn.EntryDate < endDate) || (!isFirstCalculate && y.StockIn.EntryDate >= beginDate && y.StockIn.EntryDate < endDate))
                                          && y.WarehouseID == g.Key.WarehouseID && y.ProductID == g.Key.ProductID && y.ProductSpecificationID == g.Key.ProductSpecificationID
                                          && y.BatchNumber == g.Key.BatchNumber && y.LicenseNumber == g.Key.LicenseNumber && y.ExpirationDate == g.Key.ExpirationDate)
                                          ? DB.StockInDetail.Where(y => y.IsDeleted == false && y.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse
                                          && ((isFirstCalculate && y.StockIn.EntryDate < endDate) || (!isFirstCalculate && y.StockIn.EntryDate >= beginDate && y.StockIn.EntryDate < endDate))
                                          && y.WarehouseID == g.Key.WarehouseID && y.ProductID == g.Key.ProductID && y.ProductSpecificationID == g.Key.ProductSpecificationID
                                          && y.BatchNumber == g.Key.BatchNumber && y.LicenseNumber == g.Key.LicenseNumber && y.ExpirationDate == g.Key.ExpirationDate)
                                          .Sum(y => y.InQty)
                                          : 0,
                                          TotalOutQty = DB.StockOutDetail.Any(x => x.IsDeleted == false && x.StockOut.WorkflowStatusID == (int)EWorkflowStatus.OutWarehouse
                                              && ((isFirstCalculate && x.StockOut.OutDate < endDate) || (!isFirstCalculate && x.StockOut.OutDate >= beginDate && x.StockOut.OutDate < endDate))
                                              && x.WarehouseID == g.Key.WarehouseID && x.ProductID == g.Key.ProductID && x.ProductSpecificationID == g.Key.ProductSpecificationID
                                              && x.BatchNumber == g.Key.BatchNumber && x.LicenseNumber == g.Key.LicenseNumber && x.ExpirationDate == g.Key.ExpirationDate)
                                              ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.StockOut.WorkflowStatusID == (int)EWorkflowStatus.OutWarehouse
                                              && ((isFirstCalculate && x.StockOut.OutDate < endDate) || (!isFirstCalculate && x.StockOut.OutDate >= beginDate && x.StockOut.OutDate < endDate))
                                              && x.WarehouseID == g.Key.WarehouseID && x.ProductID == g.Key.ProductID && x.ProductSpecificationID == g.Key.ProductSpecificationID
                                              && x.BatchNumber == g.Key.BatchNumber && x.LicenseNumber == g.Key.LicenseNumber && x.ExpirationDate == g.Key.ExpirationDate).Sum(x => x.OutQty)
                                              : 0,
                                      })
                                      .Select(x => new UIInventoryHistory
                                      {
                                          WarehouseID = x.Key.WarehouseID,
                                          ProductID = x.Key.ProductID,
                                          ProductSpecificationID = x.Key.ProductSpecificationID,
                                          BatchNumber = x.Key.BatchNumber,
                                          LicenseNumber = x.Key.LicenseNumber,
                                          ExpirationDate = x.Key.ExpirationDate,
                                          ProcurePrice = DB.StockInDetail.Any(y => y.WarehouseID == x.Key.WarehouseID
                                              && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                              && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                              && y.ExpirationDate == x.Key.ExpirationDate)
                                              ? DB.StockInDetail.FirstOrDefault(y => y.WarehouseID == x.Key.WarehouseID
                                              && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                              && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                              && y.ExpirationDate == x.Key.ExpirationDate).ProcurePrice : 0,
                                          InQty = x.TotalInQty,
                                          OutQty = x.TotalOutQty,
                                          BalanceQty = isFirstCalculate ? (x.TotalInQty - x.TotalOutQty)
                                          : (DB.InventoryHistory.Any(y => y.WarehouseID == x.Key.WarehouseID
                                              && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                              && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                              && y.ExpirationDate == x.Key.ExpirationDate && y.StatDate == yesterdayDate)
                                                ? DB.InventoryHistory.FirstOrDefault(y => y.WarehouseID == x.Key.WarehouseID
                                                    && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                                    && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                                    && y.ExpirationDate == x.Key.ExpirationDate && y.StatDate == yesterdayDate).BalanceQty + x.TotalInQty - x.TotalOutQty
                                                : x.TotalInQty - x.TotalOutQty),
                                          StatDate = statDate
                                      }).ToList();

            return inventoryHistories;

        }



        public IList<UIInventoryHistory> GetUIList(Domain.UISearchObjects.UISearchInventoryHistory uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIInventoryHistory> inventoryHistories = new List<UIInventoryHistory>();

            var lastInventoryHistory = DB.InventoryHistory.OrderByDescending(x => x.ID).FirstOrDefault();

            DateTime? lastStatDate = null;
            if (lastInventoryHistory != null)
                lastStatDate = lastInventoryHistory.StatDate;
            DateTime? beginDate = lastStatDate.HasValue ? lastStatDate.Value.AddDays(1) : lastStatDate;
            DateTime endDate = DateTime.Now;
            IQueryable<UIInventoryHistory> query = null;

            query = (from sid in DB.StockInDetail
                     join si in DB.StockIn on sid.StockInID equals si.ID
                     where si.IsDeleted == false && si.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse
                     && sid.IsDeleted == false
                     select new
                     {
                         sid.WarehouseID,
                         sid.ProductID,
                         sid.ProductSpecificationID,
                         sid.BatchNumber,
                         sid.LicenseNumber,
                         sid.ExpirationDate,
                         sid.ProcurePrice,
                         sid.InQty
                     })
                     .GroupBy(x => new { x.WarehouseID, x.ProductID, x.ProductSpecificationID, x.BatchNumber, x.LicenseNumber, x.ExpirationDate })
                     .Select(x => new UIInventoryHistory
                     {
                         WarehouseID = x.Key.WarehouseID,
                         ProductID = x.Key.ProductID,
                         ProductSpecificationID = x.Key.ProductSpecificationID,
                         BatchNumber = x.Key.BatchNumber,
                         LicenseNumber = x.Key.LicenseNumber,
                         ExpirationDate = x.Key.ExpirationDate,
                         ProcurePrice = DB.StockInDetail.Any(y => y.WarehouseID == x.Key.WarehouseID
                             && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                             && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                             && y.ExpirationDate == x.Key.ExpirationDate)
                             ? DB.StockInDetail.FirstOrDefault(y => y.WarehouseID == x.Key.WarehouseID
                             && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                             && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                             && y.ExpirationDate == x.Key.ExpirationDate).ProcurePrice : 0,
                         InQty = (DB.StockInDetail.Any(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                 && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                 && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                 && y.ExpirationDate == x.Key.ExpirationDate && (beginDate == null || y.StockIn.EntryDate >= beginDate) && y.StockIn.EntryDate < endDate
                                 && y.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse)
                                ? DB.StockInDetail.Where(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                    && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                    && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                    && y.ExpirationDate == x.Key.ExpirationDate && (beginDate == null || y.StockIn.EntryDate >= beginDate) && y.StockIn.EntryDate < endDate
                                    && y.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse).Sum(y => y.InQty) : 0),
                         OutQty = (DB.StockOutDetail.Any(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                 && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                 && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                 && y.ExpirationDate == x.Key.ExpirationDate
                                 && y.StockOut.WorkflowStatusID == (int)EWorkflowStatus.OutWarehouse &&
                                 (beginDate == null || y.StockOut.OutDate >= beginDate) && y.StockOut.OutDate < endDate)
                                ? DB.StockOutDetail.Where(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                    && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                    && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                    && y.ExpirationDate == x.Key.ExpirationDate
                                    && (((beginDate == null || y.StockOut.OutDate >= beginDate) && y.StockOut.OutDate < endDate)
                                       )).Sum(y => y.OutQty) : 0),
                         WarehouseName = null,
                         ProductName = null,
                         Specification = null,
                         UnitName = null,
                         NumberInLargePackage = 0,

                     });


            var queryEx = from q in query
                          join warehouse in DB.Warehouse on q.WarehouseID equals warehouse.ID
                          join product in DB.Product on q.ProductID equals product.ID
                          join productSpecification in DB.ProductSpecification on q.ProductSpecificationID equals productSpecification.ID
                          join unitName in DB.UnitOfMeasurement on productSpecification.UnitOfMeasurementID equals unitName.ID
                          select new UIInventoryHistory()
                                {
                                    WarehouseID = q.WarehouseID,
                                    ProductID = q.ProductID,
                                    ProductSpecificationID = q.ProductSpecificationID,
                                    BatchNumber = q.BatchNumber,
                                    LicenseNumber = q.LicenseNumber,
                                    ExpirationDate = q.ExpirationDate,
                                    ProcurePrice = q.ProcurePrice,
                                    InQty = q.InQty,
                                    OutQty = q.OutQty,
                                    WarehouseName = warehouse.Name,
                                    ProductName = product.ProductName,
                                    Specification = productSpecification.Specification,
                                    UnitName = unitName.UnitName,
                                    NumberInLargePackage = productSpecification.NumberInLargePackage.HasValue ? productSpecification.NumberInLargePackage.Value : 1,
                                };
            if (uiSearchObj.WarehouseName.IsNotNullOrEmpty())
            {
                queryEx = queryEx.Where(x => x.WarehouseName.Contains(uiSearchObj.WarehouseName));
            }
            if (uiSearchObj.ProductName.IsNotNullOrEmpty())
            {
                queryEx = queryEx.Where(x => x.ProductName.Contains(uiSearchObj.ProductName));
            }
            totalRecords = query.Count();
            query = query.OrderBy(x => x.WarehouseID).Skip(pageSize * pageIndex).Take(pageSize);
            inventoryHistories = queryEx.ToList();
            if (lastInventoryHistory != null)
            {
                inventoryHistories.ForEach(m =>
                {

                    int balanceQty = m.InQty - m.OutQty;
                    var oldInventoryHistory = DB.InventoryHistory.Where(x => x.StatDate == lastStatDate
                          && x.WarehouseID == m.WarehouseID && x.ProductID == m.ProductID && x.ProductSpecificationID == m.ProductSpecificationID
                          && x.BatchNumber == m.BatchNumber && x.LicenseNumber == m.LicenseNumber && x.ExpirationDate == m.ExpirationDate)
                          .FirstOrDefault();
                    if (oldInventoryHistory != null)
                    {
                        m.BalanceQty = oldInventoryHistory.BalanceQty;
                        m.BalanceQty += balanceQty;
                    }
                    else
                    {
                        m.BalanceQty += balanceQty;
                    }
                    m.Amount = m.BalanceQty * m.ProcurePrice;
                    m.NumberOfPackages = m.BalanceQty / m.NumberInLargePackage;
                });
            }

            return inventoryHistories;
        }



    }
}
