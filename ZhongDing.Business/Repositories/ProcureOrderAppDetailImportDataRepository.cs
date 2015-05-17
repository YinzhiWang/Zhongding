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

namespace ZhongDing.Business.Repositories
{
    public class ProcureOrderAppDetailImportDataRepository : BaseRepository<ProcureOrderAppDetailImportData>, IProcureOrderAppDetailImportDataRepository
    {
        public IList<Domain.UIObjects.UIProcureOrderAppDetailImportData> GetUIList(Domain.UISearchObjects.UISearchProcureOrderAppDetailImportData uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIProcureOrderAppDetailImportData> uiEntities = new List<UIProcureOrderAppDetailImportData>();

            int total = 0;
            var query = from procureOrderApplicationImportFileLog in DB.ProcureOrderApplicationImportFileLog
                        join procureOrderApplicationImportData in DB.ProcureOrderApplicationImportData
                        on procureOrderApplicationImportFileLog.ID equals procureOrderApplicationImportData.ProcureOrderApplicationImportFileLogID
                        join procureOrderAppDetailImportData in DB.ProcureOrderAppDetailImportData
                        on procureOrderApplicationImportData.ID equals procureOrderAppDetailImportData.ProcureOrderApplicationImportDataID
                        where procureOrderApplicationImportFileLog.ImportFileLogID == uiSearchObj.ImportFileLogID
                        select new UIProcureOrderAppDetailImportData()
                        {
                            ID = procureOrderAppDetailImportData.ID,
                            EstDeliveryDate = procureOrderApplicationImportData.EstDeliveryDate,
                            OrderCode = procureOrderApplicationImportData.OrderCode,
                            OrderDate = procureOrderApplicationImportData.OrderDate,
                            ProcureCount = procureOrderAppDetailImportData.ProcureCount,
                            ProcurePrice = procureOrderAppDetailImportData.ProcurePrice,
                            ProductName = procureOrderAppDetailImportData.ProductName,
                            Specification = procureOrderAppDetailImportData.Specification,
                            SupplierName = procureOrderApplicationImportData.SupplierName,
                            TotalAmount = procureOrderAppDetailImportData.TotalAmount,
                            WarehouseName = procureOrderAppDetailImportData.WarehouseName
                        };
            total = query.Count();
            uiEntities = query.OrderByDescending(x => x.ID)
                     .Skip(pageIndex * pageSize).Take(pageSize).ToList();
            totalRecords = total;

            return uiEntities;
        }
    }
}
