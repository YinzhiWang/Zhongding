using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.Repositories
{
    public class SalesOrderAppDetailImportDataRepository : BaseRepository<SalesOrderAppDetailImportData>, ISalesOrderAppDetailImportDataRepository
    {
        public IList<Domain.UIObjects.UISalesOrderAppDetailImportData> GetUIList(Domain.UISearchObjects.UISearchSalesOrderAppDetailImportData uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISalesOrderAppDetailImportData> uiEntities = new List<UISalesOrderAppDetailImportData>();

            int total = 0;
            var query = from clientSaleApplicationImportFileLog in DB.ClientSaleApplicationImportFileLog
                        join salesOrderApplicationImportData in DB.SalesOrderApplicationImportData
                        on clientSaleApplicationImportFileLog.ID equals salesOrderApplicationImportData.SaleApplicationImportFileLogID
                        join salesOrderAppDetailImportData in DB.SalesOrderAppDetailImportData
                        on salesOrderApplicationImportData.ID equals salesOrderAppDetailImportData.SalesOrderApplicationImportDataID
                        join clientSaleApplicationImportData in DB.ClientSaleApplicationImportData on salesOrderApplicationImportData.ID equals clientSaleApplicationImportData.SalesOrderApplicationImportDataID
                        where clientSaleApplicationImportFileLog.ImportFileLogID == uiSearchObj.ImportFileLogID
                        select new UISalesOrderAppDetailImportData()
                        {
                            ID = salesOrderAppDetailImportData.ID,
                            ProductName = salesOrderAppDetailImportData.ProductName,
                            WarehouseName = salesOrderAppDetailImportData.WarehouseName,
                            Count = salesOrderAppDetailImportData.Count,
                            ProductSpecification = salesOrderAppDetailImportData.ProductSpecification,
                            SalesPrice = salesOrderAppDetailImportData.SalesPrice,
                            TotalSalesAmount = salesOrderAppDetailImportData.TotalSalesAmount,
                            UnitOfMeasurement = salesOrderAppDetailImportData.UnitOfMeasurement,
                            OrderCode = salesOrderApplicationImportData.OrderCode,
                            OrderDate = salesOrderApplicationImportData.OrderDate,
                            SaleOrderType = salesOrderApplicationImportData.SaleOrderType,
                            ClientUserName = clientSaleApplicationImportData.ClientUserName,
                            ClientCompanyName = clientSaleApplicationImportData.ClientCompanyName

                        };
            total = query.Count();
            uiEntities = query.OrderByDescending(x => x.ID)
                     .Skip(pageIndex * pageSize).Take(pageSize).ToList();
            totalRecords = total;

            return uiEntities;
        }
    }
}
