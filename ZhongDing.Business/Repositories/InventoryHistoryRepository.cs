using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

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

            DateTime beginDate = statDate.Date;
            DateTime endDate = beginDate.AddDays(1);

            inventoryHistories = (from sid in DB.StockInDetail
                                  join si in DB.StockIn on sid.StockInID equals si.ID
                                  where si.IsDeleted == false && si.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse
                                  && sid.IsDeleted == false && si.EntryDate < endDate
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
                                  .Select(g => new
                                  {
                                      Key = g.Key,
                                      TotalInQty = g.Sum(x => x.InQty),
                                      TotalOutQty = DB.StockOutDetail.Any(x => x.IsDeleted == false && x.CreatedOn < endDate && x.WarehouseID == g.Key.WarehouseID
                                          && x.ProductID == g.Key.ProductID && x.ProductSpecificationID == g.Key.ProductSpecificationID
                                          && x.BatchNumber == g.Key.BatchNumber && x.LicenseNumber == g.Key.LicenseNumber
                                          && x.ExpirationDate == g.Key.ExpirationDate)
                                          ? DB.StockOutDetail.Where(x => x.IsDeleted == false && x.CreatedOn < endDate && x.WarehouseID == g.Key.WarehouseID
                                          && x.ProductID == g.Key.ProductID && x.ProductSpecificationID == g.Key.ProductSpecificationID
                                          && x.BatchNumber == g.Key.BatchNumber && x.LicenseNumber == g.Key.LicenseNumber
                                          && x.ExpirationDate == g.Key.ExpirationDate).Sum(x => x.OutQty) : 0,
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
                                      InQty = DB.InventoryHistory.Any(y => y.WarehouseID == x.Key.WarehouseID
                                          && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                          && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                          && y.ExpirationDate == x.Key.ExpirationDate)
                                          ? (DB.StockInDetail.Any(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                              && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                              && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                              && y.ExpirationDate == x.Key.ExpirationDate && y.StockIn.EntryDate >= beginDate && y.StockIn.EntryDate < endDate
                                              && y.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse)
                                             ? DB.StockInDetail.Where(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                                 && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                                 && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                                 && y.ExpirationDate == x.Key.ExpirationDate && y.StockIn.EntryDate >= beginDate && y.StockIn.EntryDate < endDate
                                                 && y.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse).Sum(y => y.InQty) : 0)
                                          : x.TotalInQty,
                                      OutQty = DB.InventoryHistory.Any(y => y.WarehouseID == x.Key.WarehouseID
                                          && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                          && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                          && y.ExpirationDate == x.Key.ExpirationDate)
                                          ? (DB.StockOutDetail.Any(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                              && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                              && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                              && y.ExpirationDate == x.Key.ExpirationDate && ((y.CreatedOn >= beginDate && y.CreatedOn < endDate)
                                                    || (y.LastModifiedOn >= beginDate && y.LastModifiedOn < endDate)))
                                             ? DB.StockOutDetail.Where(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                                 && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                                 && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                                 && y.ExpirationDate == x.Key.ExpirationDate
                                                 && ((y.CreatedOn >= beginDate && y.CreatedOn < endDate)
                                                    || (y.LastModifiedOn >= beginDate && y.LastModifiedOn < endDate))).Sum(y => y.OutQty) : 0)
                                          : x.TotalOutQty,
                                      BalanceQty = x.TotalInQty - x.TotalOutQty,
                                      StatDate = statDate
                                  }).ToList();

            return inventoryHistories;
        }

    }
}
