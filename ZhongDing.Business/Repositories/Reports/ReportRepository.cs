using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Common.Extension;
using System.Data.Entity.Core.Objects;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Common.Enums;

namespace ZhongDing.Business.Repositories.Reports
{
    public class ReportRepository : IReportRepository
    {
        #region Members

        private DbModelContainer db = null;
        protected internal DbModelContainer DB
        {
            get
            {
                if (db == null)
                    db = new DbModelContainer();

                return db;
            }
        }

        #endregion

        #region 构造函数



        static ReportRepository()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{TEntity}"/> class.
        /// </summary>
        public ReportRepository()
        {
            db = new DbModelContainer();
        }

        #endregion

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        #region GetCompanyReport

        public IList<UICompany> GetCompanyReport()
        {
            return this.DB.Database.SqlQuery<UICompany>("exec GetCompanyList").ToList();
        }
        #endregion

        #region GetProcureOrderReport

        public IList<UIProcureOrderReport> GetProcureOrderReport(Domain.UISearchObjects.UISearchProcureOrderReport uiSearchObj)
        {
            List<UIProcureOrderReport> result = null;
            int totalRecords = 0;
            BuildProcureOrderReport(uiSearchObj, 0, 10000000, out totalRecords, out result);
            return result;
        }

        public IList<UIProcureOrderReport> GetProcureOrderReport(Domain.UISearchObjects.UISearchProcureOrderReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {

            List<UIProcureOrderReport> result = null;
            BuildProcureOrderReport(uiSearchObj, pageIndex, pageSize, out totalRecords, out result);
            return result;
        }

        private void BuildProcureOrderReport(Domain.UISearchObjects.UISearchProcureOrderReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords, out List<UIProcureOrderReport> result)
        {
            SqlParameter totalRecordSqlParameter = new SqlParameter() { ParameterName = "@totalRecord", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.Output };
            List<SqlParameter> parameters = new List<SqlParameter>() {
                new SqlParameter(){ ParameterName="@pageSize",Value=pageSize,Size=4},
                new SqlParameter(){ ParameterName="@pageIndex", Value=pageIndex,Size=4},
                totalRecordSqlParameter
            };

            string sql = "exec GetProcureOrderReport @pageSize,@pageIndex,@beginDate,@endDate,@supplierId,@productId,@totalRecord out";
            parameters.Add(new SqlParameter() { ParameterName = "@beginDate", SqlDbType = SqlDbType.DateTime, Size = 256, Value = uiSearchObj.BeginDate.HasValue ? (object)uiSearchObj.BeginDate.Value : DBNull.Value });

            parameters.Add(new SqlParameter() { ParameterName = "@endDate", SqlDbType = SqlDbType.DateTime, Size = 256, Value = uiSearchObj.EndDate.HasValue ? (object)uiSearchObj.EndDate.Value : DBNull.Value });

            parameters.Add(new SqlParameter() { ParameterName = "@supplierId", Size = 4, Value = uiSearchObj.SupplierId.HasValue ? (object)uiSearchObj.SupplierId.Value : DBNull.Value });

            parameters.Add(new SqlParameter() { ParameterName = "@productId", Size = 4, Value = uiSearchObj.ProductId.HasValue ? (object)uiSearchObj.ProductId.Value : DBNull.Value });

            result = this.DB.Database.SqlQuery<UIProcureOrderReport>(sql, parameters.ToArray()).ToList();


            result.ForEach(x =>
            {
                x.AlreadyInNumberOfPackages = (decimal)x.AlreadyInQty / (decimal)x.NumberInLargePackage;
                if (x.ProcureOrderApplicationIsStop)
                {
                    x.StopInQty = x.ProcureCount - x.AlreadyInQty;
                    x.StopInQtyProcurePrice = x.StopInQty * x.ProcurePrice;
                    x.StopInNumberOfPackages = (decimal)x.StopInQty / (decimal)x.NumberInLargePackage;
                }
                else
                {
                    x.NotInQty = x.ProcureCount - x.AlreadyInQty;
                    x.NotInQtyProcurePrice = x.NotInQty * x.ProcurePrice;
                    x.NotInNumberOfPackages = (decimal)x.NotInQty / (decimal)x.NumberInLargePackage;

                }

            });

            totalRecords = totalRecordSqlParameter.Value.ToInt();
        }
        #endregion

        #region GetProcureOrderApplicationPaymentReport
        public IList<UIProcureOrderApplicationPaymentReport> GetProcureOrderApplicationPaymentReport(UISearchProcureOrderApplicationPaymentReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIProcureOrderApplicationPaymentReport> result = null;
            BuildProcureOrderApplicationPaymentReport(uiSearchObj, pageIndex, pageSize, out totalRecords, out result);
            return result;
        }

        public IList<UIProcureOrderApplicationPaymentReport> GetProcureOrderApplicationPaymentReport(Domain.UISearchObjects.UISearchProcureOrderApplicationPaymentReport uiSearchObj)
        {
            List<UIProcureOrderApplicationPaymentReport> result = null;
            int totalRecords = 0;
            BuildProcureOrderApplicationPaymentReport(uiSearchObj, 0, 10000000, out totalRecords, out result);
            return result;
        }
        private void BuildProcureOrderApplicationPaymentReport(UISearchProcureOrderApplicationPaymentReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords, out List<UIProcureOrderApplicationPaymentReport> result)
        {
            SqlParameter totalRecordSqlParameter = new SqlParameter() { ParameterName = "@totalRecord", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.Output };
            List<SqlParameter> parameters = new List<SqlParameter>() {
                new SqlParameter(){ ParameterName="@pageSize",Value=pageSize,Size=4},
                new SqlParameter(){ ParameterName="@pageIndex", Value=pageIndex,Size=4},
                totalRecordSqlParameter
            };

            string sql = "exec GetProcureOrderApplicationPaymentReport @pageSize,@pageIndex,@beginDate,@endDate,@supplierId,@totalRecord out";
            parameters.Add(new SqlParameter() { ParameterName = "@beginDate", SqlDbType = SqlDbType.DateTime, Size = 256, Value = uiSearchObj.BeginDate.HasValue ? (object)uiSearchObj.BeginDate.Value : DBNull.Value });
            parameters.Add(new SqlParameter() { ParameterName = "@endDate", SqlDbType = SqlDbType.DateTime, Size = 256, Value = uiSearchObj.EndDate.HasValue ? (object)uiSearchObj.EndDate.Value : DBNull.Value });
            parameters.Add(new SqlParameter() { ParameterName = "@supplierId", Size = 4, Value = uiSearchObj.SupplierId.HasValue ? (object)uiSearchObj.SupplierId.Value : DBNull.Value });

            result = this.DB.Database.SqlQuery<UIProcureOrderApplicationPaymentReport>(sql, parameters.ToArray()).ToList();
            totalRecords = totalRecordSqlParameter.Value.ToInt();
        }
        #endregion

        #region GetClientSaleAppReport
        public IList<UIClientSaleAppReport> GetClientSaleAppReport(UISearchClientSaleAppReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIClientSaleAppReport> result = null;
            BuildClientSaleAppReport(uiSearchObj, pageIndex, pageSize, out totalRecords, out result);
            return result;
        }

        public IList<UIClientSaleAppReport> GetClientSaleAppReport(UISearchClientSaleAppReport uiSearchObj)
        {

            List<UIClientSaleAppReport> result = null;
            int totalRecords = 0;
            BuildClientSaleAppReport(uiSearchObj, 0, 10000000, out totalRecords, out result);
            return result;
        }
        private void BuildClientSaleAppReport(UISearchClientSaleAppReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords, out List<UIClientSaleAppReport> result)
        {
            SqlParameter totalRecordSqlParameter = new SqlParameter() { ParameterName = "@totalRecord", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.Output };
            List<SqlParameter> parameters = new List<SqlParameter>() {
                new SqlParameter(){ ParameterName="@pageSize",Value=pageSize,Size=4},
                new SqlParameter(){ ParameterName="@pageIndex", Value=pageIndex,Size=4},
                totalRecordSqlParameter
            };

            string sql = "exec GetClientSaleAppReport @pageSize,@pageIndex,@beginDate,@endDate,@clientUserId,@clientCompanyId,@productId,@totalRecord out";
            parameters.Add(new SqlParameter() { ParameterName = "@beginDate", SqlDbType = SqlDbType.DateTime, Size = 256, Value = uiSearchObj.BeginDate.HasValue ? (object)uiSearchObj.BeginDate.Value : DBNull.Value });
            parameters.Add(new SqlParameter() { ParameterName = "@endDate", SqlDbType = SqlDbType.DateTime, Size = 256, Value = uiSearchObj.EndDate.HasValue ? (object)uiSearchObj.EndDate.Value : DBNull.Value });
            parameters.Add(new SqlParameter() { ParameterName = "@clientUserId", Size = 4, Value = uiSearchObj.ClientUserId.HasValue ? (object)uiSearchObj.ClientUserId.Value : DBNull.Value });
            parameters.Add(new SqlParameter() { ParameterName = "@clientCompanyId", Size = 4, Value = uiSearchObj.ClientCompanyId.HasValue ? (object)uiSearchObj.ClientCompanyId.Value : DBNull.Value });
            parameters.Add(new SqlParameter() { ParameterName = "@productId", Size = 4, Value = uiSearchObj.ProductId.HasValue ? (object)uiSearchObj.ProductId.Value : DBNull.Value });

            result = this.DB.Database.SqlQuery<UIClientSaleAppReport>(sql, parameters.ToArray()).ToList();

            result.ForEach(x =>
            {
                x.AlreadyOutNumberOfPackages = (decimal)x.AlreadyOutQty / (decimal)x.NumberInLargePackage;
                if (x.SalesOrderApplicationIsStop)
                {
                    x.StopOutQty = x.Count - x.AlreadyOutQty;
                    x.StopOutQtySalesPricePrice = x.StopOutQty * x.SalesPrice;
                    x.StopOutNumberOfPackages = (decimal)x.StopOutQty / (decimal)x.NumberInLargePackage;
                }
                else
                {
                    x.NotOutQty = x.Count - x.AlreadyOutQty;
                    x.NotOutQtySalesPricePrice = x.NotOutQty * x.SalesPrice;
                    x.NotOutNumberOfPackages = (decimal)x.NotOutQty / (decimal)x.NumberInLargePackage;

                }

            });

            totalRecords = totalRecordSqlParameter.Value.ToInt();
        }
        #endregion


        public IList<UIStockOutDetailReport> GetStockOutDetailReport(UISearchStockOutDetailReport uiSearchObj)
        {
            List<UIStockOutDetailReport> result = null;
            int totalRecords = 0;
            BuildStockOutDetailReport(uiSearchObj, 0, 10000000, out totalRecords, out result);
            return result;
        }


        public IList<UIStockOutDetailReport> GetStockOutDetailReport(UISearchStockOutDetailReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIStockOutDetailReport> result = null;
            BuildStockOutDetailReport(uiSearchObj, pageIndex, pageSize, out totalRecords, out result);
            return result;
        }

        private void BuildStockOutDetailReport(UISearchStockOutDetailReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords, out List<UIStockOutDetailReport> result)
        {
            SqlParameter totalRecordSqlParameter = new SqlParameter() { ParameterName = "@totalRecord", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.Output };
            List<SqlParameter> parameters = new List<SqlParameter>() {
                new SqlParameter(){ ParameterName="@pageSize",Value=pageSize,Size=4},
                new SqlParameter(){ ParameterName="@pageIndex", Value=pageIndex,Size=4},
                totalRecordSqlParameter
            };

            string sql = "exec GetStockOutDetailReport @pageSize,@pageIndex,@beginDate,@endDate,@clientUserId,@clientCompanyId,@productId,@totalRecord out";
            parameters.Add(new SqlParameter() { ParameterName = "@beginDate", SqlDbType = SqlDbType.DateTime, Size = 256, Value = uiSearchObj.BeginDate.HasValue ? (object)uiSearchObj.BeginDate.Value : DBNull.Value });
            parameters.Add(new SqlParameter() { ParameterName = "@endDate", SqlDbType = SqlDbType.DateTime, Size = 256, Value = uiSearchObj.EndDate.HasValue ? (object)uiSearchObj.EndDate.Value : DBNull.Value });
            parameters.Add(new SqlParameter() { ParameterName = "@clientUserId", Size = 4, Value = uiSearchObj.ClientUserId.HasValue ? (object)uiSearchObj.ClientUserId.Value : DBNull.Value });
            parameters.Add(new SqlParameter() { ParameterName = "@clientCompanyId", Size = 4, Value = uiSearchObj.ClientCompanyId.HasValue ? (object)uiSearchObj.ClientCompanyId.Value : DBNull.Value });
            parameters.Add(new SqlParameter() { ParameterName = "@productId", Size = 4, Value = uiSearchObj.ProductId.HasValue ? (object)uiSearchObj.ProductId.Value : DBNull.Value });

            result = this.DB.Database.SqlQuery<UIStockOutDetailReport>(sql, parameters.ToArray()).ToList();
            totalRecords = totalRecordSqlParameter.Value.ToInt();
        }


        public IList<UIStockInDetailReport> GetStockInDetailReport(UISearchStockInDetailReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIStockInDetailReport> result = null;
            BuildStockInDetailReport(uiSearchObj, pageIndex, pageSize, out totalRecords, out result);
            return result;
        }

        public IList<UIStockInDetailReport> GetStockInDetailReport(UISearchStockInDetailReport uiSearchObj)
        {
            List<UIStockInDetailReport> result = null;
            int totalRecords = 0;
            BuildStockInDetailReport(uiSearchObj, 0, 10000000, out totalRecords, out result);
            return result;
        }
        private void BuildStockInDetailReport(UISearchStockInDetailReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords, out List<UIStockInDetailReport> result)
        {
            SqlParameter totalRecordSqlParameter = new SqlParameter() { ParameterName = "@totalRecord", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.Output };
            List<SqlParameter> parameters = new List<SqlParameter>() {
                new SqlParameter(){ ParameterName="@pageSize",Value=pageSize,Size=4},
                new SqlParameter(){ ParameterName="@pageIndex", Value=pageIndex,Size=4},
                totalRecordSqlParameter
            };

            string sql = "exec GetStockInDetailReport @pageSize,@pageIndex,@beginDate,@endDate,@supplierId,@productId,@batchNumber,@totalRecord out";
            parameters.Add(new SqlParameter() { ParameterName = "@beginDate", SqlDbType = SqlDbType.DateTime, Size = 256, Value = uiSearchObj.BeginDate.HasValue ? (object)uiSearchObj.BeginDate.Value : DBNull.Value });
            parameters.Add(new SqlParameter() { ParameterName = "@endDate", SqlDbType = SqlDbType.DateTime, Size = 256, Value = uiSearchObj.EndDate.HasValue ? (object)uiSearchObj.EndDate.Value : DBNull.Value });
            parameters.Add(new SqlParameter() { ParameterName = "@supplierId", Size = 4, Value = uiSearchObj.SupplierId.HasValue ? (object)uiSearchObj.SupplierId.Value : DBNull.Value });
            parameters.Add(new SqlParameter() { ParameterName = "@productId", Size = 4, Value = uiSearchObj.ProductId.HasValue ? (object)uiSearchObj.ProductId.Value : DBNull.Value });
            parameters.Add(new SqlParameter() { ParameterName = "@batchNumber", SqlDbType = SqlDbType.NVarChar, Size = 256, Value = uiSearchObj.BatchNumber.HasValue() ? (object)uiSearchObj.BatchNumber : DBNull.Value });

            result = this.DB.Database.SqlQuery<UIStockInDetailReport>(sql, parameters.ToArray()).ToList();
            totalRecords = totalRecordSqlParameter.Value.ToInt();
        }



        //
        public IList<UIProcurePlanReport> GetProcurePlanReport(UISearchProcurePlanReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIProcurePlanReport> result = null;
            BuildProcurePlanReport(uiSearchObj, pageIndex, pageSize, out totalRecords, out result);
            return result;
        }

        public IList<UIProcurePlanReport> GetProcurePlanReport(UISearchProcurePlanReport uiSearchObj)
        {
            List<UIProcurePlanReport> result = null;
            int totalRecords = 0;
            BuildProcurePlanReport(uiSearchObj, 0, 10000000, out totalRecords, out result);
            return result;
        }
        private void BuildProcurePlanReport(UISearchProcurePlanReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords, out List<UIProcurePlanReport> result)
        {
            SqlParameter totalRecordSqlParameter = new SqlParameter() { ParameterName = "@totalRecord", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.Output };
            List<SqlParameter> parameters = new List<SqlParameter>() {
                new SqlParameter(){ ParameterName="@pageSize",Value=pageSize,Size=4},
                new SqlParameter(){ ParameterName="@pageIndex", Value=pageIndex,Size=4},
                totalRecordSqlParameter
            };

            string sql = "exec GetProcurePlanReport @pageSize,@pageIndex,@warehouseId,@productName,@totalRecord out";
            parameters.Add(new SqlParameter() { ParameterName = "@warehouseId", SqlDbType = SqlDbType.Int, Size = 256, Value = uiSearchObj.WarehouseID.HasValue ? (object)uiSearchObj.WarehouseID.Value : DBNull.Value });
            parameters.Add(new SqlParameter() { ParameterName = "@productName", SqlDbType = SqlDbType.NVarChar, Size = 256, Value = uiSearchObj.ProductName.HasValue() ? (object)uiSearchObj.ProductName : DBNull.Value });

            result = this.DB.Database.SqlQuery<UIProcurePlanReport>(sql, parameters.ToArray()).ToList();
            totalRecords = totalRecordSqlParameter.Value.ToInt();
            //x.ProductID, x.ProductSpecificationID, x.WarehouseID, x.WarehouseName
            result.ForEach(x =>
            {
                x.ToBeOutNumberOfPackages = x.TotalToBeOutQty / x.NumberInLargePackage;
                //x
                IInventoryHistoryRepository inventoryHistoryRepository = new InventoryHistoryRepository();
                int balanceQty = inventoryHistoryRepository.GetProductBalanceQty(x.WarehouseID, x.ProductID, x.ProductSpecificationID);
                x.WarehouseQty = balanceQty;
                x.WarehouseNumberOfPackages = balanceQty / x.NumberInLargePackage;

            });

        }


        public IList<UIInventorySummaryReport> GetInventorySummaryReport(UISearchInventorySummaryReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIInventorySummaryReport> result = null;
            BuildInventorySummaryReport(uiSearchObj, pageIndex, pageSize, out totalRecords, out result);
            //test start
            //totalRecords = 12;
            //result = new List<UIInventorySummaryReport>();
            //test end
            return result;
        }
        private void BuildInventorySummaryReport(UISearchInventorySummaryReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords, out List<UIInventorySummaryReport> result)
        {
            totalRecords = 0;
            result = new List<UIInventorySummaryReport>();


            IQueryable<UIInventoryHistory> queryHasToday = null;//包含今天或者说 Service最后一次计算之后的最新库存
            IQueryable<UIInventoryHistory> queryBaseInfo = null;
            InventoryHistory lastInventoryHistory = null;
            //Need Today  如果包含今天，那么需要计算今天实时数据，如果今天添加了新货品，那么依靠上边的功能无法获取到新的货品，那么下边计划采用 如果发现新货品，那么将新货品追加到追后，同时更新Total
            if (uiSearchObj.EndDate == null || uiSearchObj.EndDate >= DateTime.Now.Date)
            {
                lastInventoryHistory = DB.InventoryHistory.OrderByDescending(x => x.ID).FirstOrDefault();

                DateTime? lastStatDate = null;
                if (lastInventoryHistory != null)
                    lastStatDate = lastInventoryHistory.StatDate;
                DateTime? beginDate = lastStatDate.HasValue ? lastStatDate.Value.AddDays(1) : lastStatDate;
                DateTime endDate = DateTime.Now;

                queryHasToday = (from sid in DB.StockInDetail
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
                             NewInQty = (DB.StockInDetail.Any(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                     && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                     && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                     && y.ExpirationDate == x.Key.ExpirationDate && (beginDate == null || y.StockIn.EntryDate >= beginDate) && y.StockIn.EntryDate < endDate
                                     && y.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse)
                                    ? DB.StockInDetail.Where(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
                                        && y.ProductID == x.Key.ProductID && y.ProductSpecificationID == x.Key.ProductSpecificationID
                                        && y.BatchNumber == x.Key.BatchNumber && y.LicenseNumber == x.Key.LicenseNumber
                                        && y.ExpirationDate == x.Key.ExpirationDate && (beginDate == null || y.StockIn.EntryDate >= beginDate) && y.StockIn.EntryDate < endDate
                                        && y.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse).Sum(y => y.InQty) : 0),
                             NewOutQty = (DB.StockOutDetail.Any(y => y.IsDeleted == false && y.WarehouseID == x.Key.WarehouseID
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
                             InQty = DB.InventoryHistory.Where(m => (uiSearchObj.BeginDate == null || m.StatDate >= uiSearchObj.BeginDate)
                                    && (uiSearchObj.EndDate == null || m.StatDate <= uiSearchObj.EndDate)
                                    && m.WarehouseID == x.Key.WarehouseID
                                    && m.ProductID == x.Key.ProductID && m.ProductSpecificationID == x.Key.ProductSpecificationID
                                    && m.BatchNumber == x.Key.BatchNumber && m.LicenseNumber == x.Key.LicenseNumber
                                    && m.ExpirationDate == x.Key.ExpirationDate).Any() ?
                                    DB.InventoryHistory.Where(m => (uiSearchObj.BeginDate == null || m.StatDate >= uiSearchObj.BeginDate)
                                    && (uiSearchObj.EndDate == null || m.StatDate <= uiSearchObj.EndDate)
                                    && m.WarehouseID == x.Key.WarehouseID
                                    && m.ProductID == x.Key.ProductID && m.ProductSpecificationID == x.Key.ProductSpecificationID
                                    && m.BatchNumber == x.Key.BatchNumber && m.LicenseNumber == x.Key.LicenseNumber
                                    && m.ExpirationDate == x.Key.ExpirationDate).Sum(m => m.InQty) : 0,
                             OutQty = DB.InventoryHistory.Where(m => (uiSearchObj.BeginDate == null || m.StatDate >= uiSearchObj.BeginDate)
                                    && (uiSearchObj.EndDate == null || m.StatDate <= uiSearchObj.EndDate)
                                    && m.WarehouseID == x.Key.WarehouseID
                                    && m.ProductID == x.Key.ProductID && m.ProductSpecificationID == x.Key.ProductSpecificationID
                                    && m.BatchNumber == x.Key.BatchNumber && m.LicenseNumber == x.Key.LicenseNumber
                                    && m.ExpirationDate == x.Key.ExpirationDate).Any() ?
                                    DB.InventoryHistory.Where(m => (uiSearchObj.BeginDate == null || m.StatDate >= uiSearchObj.BeginDate)
                                    && (uiSearchObj.EndDate == null || m.StatDate <= uiSearchObj.EndDate)
                                    && m.WarehouseID == x.Key.WarehouseID
                                    && m.ProductID == x.Key.ProductID && m.ProductSpecificationID == x.Key.ProductSpecificationID
                                    && m.BatchNumber == x.Key.BatchNumber && m.LicenseNumber == x.Key.LicenseNumber
                                    && m.ExpirationDate == x.Key.ExpirationDate).Sum(m => m.OutQty) : 0,
                         });

            }
            else
            {
                //WarehouseID, x.ProductID, x.ProductSpecificationID, x.BatchNumber, x.LicenseNumber, x.ExpirationDate
                queryBaseInfo = from inventoryHistory in DB.InventoryHistory
                                where (uiSearchObj.BeginDate == null || inventoryHistory.StatDate >= uiSearchObj.BeginDate)
                                && (uiSearchObj.EndDate == null || inventoryHistory.StatDate <= uiSearchObj.EndDate)
                                group inventoryHistory by new
                                {
                                    inventoryHistory.WarehouseID,
                                    inventoryHistory.ProductID,
                                    inventoryHistory.ProductSpecificationID,
                                    inventoryHistory.BatchNumber,
                                    inventoryHistory.LicenseNumber,
                                    inventoryHistory.ExpirationDate,
                                    inventoryHistory.ProcurePrice

                                } into g
                                select new UIInventoryHistory()
                                {
                                    WarehouseID = g.Key.WarehouseID,
                                    ProductID = g.Key.ProductID,
                                    ProductSpecificationID = g.Key.ProductSpecificationID,
                                    BatchNumber = g.Key.BatchNumber,
                                    LicenseNumber = g.Key.LicenseNumber,
                                    ExpirationDate = g.Key.ExpirationDate,
                                    InQty = g.Sum(x => x.InQty),
                                    OutQty = g.Sum(x => x.OutQty),
                                    ProcurePrice = g.Key.ProcurePrice,
                                    NewInQty = 0,
                                    NewOutQty = 0
                                };
            }

            IQueryable<UIInventoryHistory> queryTemp = queryHasToday == null ? queryBaseInfo : queryHasToday;

            if (queryTemp != null)
            {
                DateTime? preStatDate = uiSearchObj.BeginDate.HasValue ? uiSearchObj.BeginDate.Value.AddDays(-1) : uiSearchObj.BeginDate;
                DateTime? currentStatDate = uiSearchObj.EndDate.HasValue ? uiSearchObj.EndDate.Value : uiSearchObj.EndDate;
                if (currentStatDate == null || currentStatDate >= DateTime.Now)
                {
                    currentStatDate = DateTime.Now.AddDays(-1).Date;
                    if (lastInventoryHistory != null && lastInventoryHistory.StatDate != currentStatDate)
                    {
                        currentStatDate = lastInventoryHistory.StatDate;
                    }
                }
                //var lastInventoryHistory = DB.InventoryHistory.OrderByDescending(x => x.ID).FirstOrDefault();

                var queryExInfo = from q in queryTemp
                                  join warehouse in DB.Warehouse on q.WarehouseID equals warehouse.ID
                                  join product in DB.Product on q.ProductID equals product.ID
                                  join productSpecification in DB.ProductSpecification on q.ProductSpecificationID equals productSpecification.ID
                                  join unitName in DB.UnitOfMeasurement on productSpecification.UnitOfMeasurementID equals unitName.ID
                                  select new UIInventorySummaryReport()
                                  {
                                      WarehouseID = q.WarehouseID,
                                      ProductID = q.ProductID,
                                      ProductSpecificationID = q.ProductSpecificationID,
                                      BatchNumber = q.BatchNumber,
                                      LicenseNumber = q.LicenseNumber,
                                      ExpirationDate = q.ExpirationDate,
                                      InQty = q.InQty,
                                      OutQty = q.OutQty,
                                      ProcurePrice = q.ProcurePrice,
                                      WarehouseName = warehouse.Name,
                                      ProductName = product.ProductName,
                                      ProductCode = product.ProductCode,
                                      Specification = productSpecification.Specification,
                                      UnitName = unitName.UnitName,
                                      NumberInLargePackage = productSpecification.NumberInLargePackage.HasValue ? productSpecification.NumberInLargePackage.Value : 1,
                                      PreBalanceQty = (preStatDate == null ? 0 :
                                          (DB.InventoryHistory.Where(x => x.StatDate == preStatDate && x.WarehouseID == q.WarehouseID && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID
                                          && x.BatchNumber == q.BatchNumber && x.LicenseNumber == q.LicenseNumber && x.ExpirationDate == q.ExpirationDate).Any() ?
                                          DB.InventoryHistory.Where(x => x.StatDate == preStatDate && x.WarehouseID == q.WarehouseID && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID
                                          && x.BatchNumber == q.BatchNumber && x.LicenseNumber == q.LicenseNumber && x.ExpirationDate == q.ExpirationDate).FirstOrDefault().BalanceQty : 0)),
                                      CurrentBalanceQty = (currentStatDate == null ? 0 :
                                          (DB.InventoryHistory.Where(x => x.StatDate == currentStatDate && x.WarehouseID == q.WarehouseID && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID
                                          && x.BatchNumber == q.BatchNumber && x.LicenseNumber == q.LicenseNumber && x.ExpirationDate == q.ExpirationDate).Any() ?
                                          DB.InventoryHistory.Where(x => x.StatDate == currentStatDate && x.WarehouseID == q.WarehouseID && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID
                                          && x.BatchNumber == q.BatchNumber && x.LicenseNumber == q.LicenseNumber && x.ExpirationDate == q.ExpirationDate).FirstOrDefault().BalanceQty : 0)),
                                      NewInQty = q.NewInQty,
                                      NewOutQty = q.NewOutQty
                                  };
                if (uiSearchObj.ProductID.BiggerThanZero())
                {
                    queryExInfo = queryExInfo.Where(x => x.ProductID == uiSearchObj.ProductID);
                }
                totalRecords = queryExInfo.Count();
                queryExInfo = queryExInfo.OrderBy(x => x.WarehouseID).Skip(pageSize * pageIndex).Take(pageSize);
                result = queryExInfo.ToList();

            }

            result.ForEach(x =>
            {
                x.InQty += x.NewInQty;
                x.OutQty += x.NewOutQty;

                x.CurrentBalanceQty += (x.NewInQty - x.NewOutQty);

                x.InQtyPackages = x.InQty.ToDecimal() / x.NumberInLargePackage.ToDecimal();
                x.OutQtyPackages = x.OutQty.ToDecimal() / x.NumberInLargePackage.ToDecimal();
                x.PreBalanceQtyPackages = x.PreBalanceQty.ToDecimal() / x.NumberInLargePackage.ToDecimal();
                x.CurrentBalanceQtyPackages = x.CurrentBalanceQty.ToDecimal() / x.NumberInLargePackage.ToDecimal();
                x.Amount = x.CurrentBalanceQty * x.ProcurePrice;

            });

        }


        public IList<UIInventorySummaryDetailReport> GetInventorySummaryDetailReport(UISearchInventorySummaryDetailReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIInventorySummaryDetailReport> result = null;
            BuildInventorySummaryDetailReport(uiSearchObj, pageIndex, pageSize, out totalRecords, out result);
            return result;
        }
        private void BuildInventorySummaryDetailReport(UISearchInventorySummaryDetailReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords, out List<UIInventorySummaryDetailReport> result)
        {
            totalRecords = 0;
            result = new List<UIInventorySummaryDetailReport>();

            var queryBaseInfoIn = from stockInDetail in DB.StockInDetail
                                  join stockIn in DB.StockIn on stockInDetail.StockInID equals stockIn.ID
                                  join procureOrderAppDetail in DB.ProcureOrderAppDetail on stockInDetail.ProcureOrderAppDetailID equals procureOrderAppDetail.ID
                                  join procureOrderApplication in DB.ProcureOrderApplication on procureOrderAppDetail.ProcureOrderApplicationID equals procureOrderApplication.ID
                                  join warehouse in DB.Warehouse on stockInDetail.WarehouseID equals warehouse.ID
                                  join product in DB.Product on stockInDetail.ProductID equals product.ID
                                  join productSpecification in DB.ProductSpecification on stockInDetail.ProductSpecificationID equals productSpecification.ID
                                  join unitName in DB.UnitOfMeasurement on productSpecification.UnitOfMeasurementID equals unitName.ID
                                  where stockIn.IsDeleted == false && stockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse && stockInDetail.IsDeleted == false
                                  select new UIInventorySummaryDetailReport()
                                  {
                                      ID = stockInDetail.ID,
                                      EntryOrOutDate = stockIn.EntryDate,
                                      Type = "入库",
                                      OrderCode = procureOrderApplication.OrderCode,
                                      StockInOrOutCode = stockIn.Code,
                                      ProductCode = product.ProductCode,
                                      ProductName = product.ProductName,
                                      Specification = productSpecification.Specification,
                                      UnitName = unitName.UnitName,
                                      BatchNumber = stockInDetail.BatchNumber,
                                      ExpirationDate = stockInDetail.ExpirationDate,
                                      Price = stockInDetail.ProcurePrice,
                                      InQty = stockInDetail.InQty,
                                      OutQty = null,

                                      NumberInLargePackage = productSpecification.NumberInLargePackage.HasValue ? productSpecification.NumberInLargePackage.Value : 1,
                                      WarehouseID = stockInDetail.WarehouseID,
                                      ProductID = stockInDetail.ProductID,
                                      ProductSpecificationID = stockInDetail.ProductSpecificationID,
                                      LicenseNumber = stockInDetail.LicenseNumber

                                  };

            var queryBaseInfoOut = from stockOutDetail in DB.StockOutDetail
                                   join stockOut in DB.StockOut on stockOutDetail.StockOutID equals stockOut.ID
                                   join salesOrderAppDetail in DB.SalesOrderAppDetail on stockOutDetail.SalesOrderAppDetailID equals salesOrderAppDetail.ID
                                   join salesOrderApplication in DB.SalesOrderApplication on salesOrderAppDetail.SalesOrderApplicationID equals salesOrderApplication.ID
                                   join warehouse in DB.Warehouse on stockOutDetail.WarehouseID equals warehouse.ID
                                   join product in DB.Product on stockOutDetail.ProductID equals product.ID
                                   join productSpecification in DB.ProductSpecification on stockOutDetail.ProductSpecificationID equals productSpecification.ID
                                   join unitName in DB.UnitOfMeasurement on productSpecification.UnitOfMeasurementID equals unitName.ID
                                   where stockOut.IsDeleted == false && stockOut.WorkflowStatusID == (int)EWorkflowStatus.OutWarehouse && stockOutDetail.IsDeleted == false
                                   select new UIInventorySummaryDetailReport()
                                   {
                                       ID = stockOutDetail.ID,
                                       EntryOrOutDate = stockOut.OutDate,
                                       Type = " 出库",
                                       OrderCode = salesOrderApplication.OrderCode,
                                       StockInOrOutCode = stockOut.Code,
                                       ProductCode = product.ProductCode,
                                       ProductName = product.ProductName,
                                       Specification = productSpecification.Specification,
                                       UnitName = unitName.UnitName,
                                       BatchNumber = stockOutDetail.BatchNumber,
                                       ExpirationDate = stockOutDetail.ExpirationDate,
                                       Price = stockOutDetail.SalesPrice,
                                       InQty = null,
                                       OutQty = stockOutDetail.OutQty,

                                       NumberInLargePackage = productSpecification.NumberInLargePackage.HasValue ? productSpecification.NumberInLargePackage.Value : 1,
                                       WarehouseID = stockOutDetail.WarehouseID,
                                       ProductID = stockOutDetail.ProductID,
                                       ProductSpecificationID = stockOutDetail.ProductSpecificationID,
                                       LicenseNumber = stockOutDetail.LicenseNumber

                                   };

            if (uiSearchObj.WarehouseID.BiggerThanZero())
            {
                queryBaseInfoIn = queryBaseInfoIn.Where(x => x.WarehouseID == uiSearchObj.WarehouseID);
                queryBaseInfoOut = queryBaseInfoOut.Where(x => x.WarehouseID == uiSearchObj.WarehouseID);
            }
            if (uiSearchObj.ProductID.BiggerThanZero())
            {
                queryBaseInfoIn = queryBaseInfoIn.Where(x => x.ProductID == uiSearchObj.ProductID);
                queryBaseInfoOut = queryBaseInfoOut.Where(x => x.ProductID == uiSearchObj.ProductID);
            }
            if (uiSearchObj.ProductSpecificationID.BiggerThanZero())
            {
                queryBaseInfoIn = queryBaseInfoIn.Where(x => x.ProductSpecificationID == uiSearchObj.ProductSpecificationID);
                queryBaseInfoOut = queryBaseInfoOut.Where(x => x.ProductSpecificationID == uiSearchObj.ProductSpecificationID);
            }
            if (uiSearchObj.BatchNumber.HasValue())
            {
                queryBaseInfoIn = queryBaseInfoIn.Where(x => x.BatchNumber == uiSearchObj.BatchNumber);
                queryBaseInfoOut = queryBaseInfoOut.Where(x => x.BatchNumber == uiSearchObj.BatchNumber);
            }
            if (uiSearchObj.LicenseNumber.HasValue())
            {
                queryBaseInfoIn = queryBaseInfoIn.Where(x => x.LicenseNumber == uiSearchObj.LicenseNumber);
                queryBaseInfoOut = queryBaseInfoOut.Where(x => x.LicenseNumber == uiSearchObj.LicenseNumber);
            }
            if (uiSearchObj.ExpirationDate.HasValue)
            {
                queryBaseInfoIn = queryBaseInfoIn.Where(x => x.ExpirationDate == uiSearchObj.ExpirationDate);
                queryBaseInfoOut = queryBaseInfoOut.Where(x => x.ExpirationDate == uiSearchObj.ExpirationDate);
            }
            if (uiSearchObj.BeginDate.HasValue)
            {
                queryBaseInfoIn = queryBaseInfoIn.Where(x => x.EntryOrOutDate >= uiSearchObj.BeginDate);
                queryBaseInfoOut = queryBaseInfoOut.Where(x => x.EntryOrOutDate >= uiSearchObj.BeginDate);
            }
            if (uiSearchObj.EndDate.HasValue)
            {
                uiSearchObj.EndDate=uiSearchObj.EndDate.Value.AddDays(1);
                queryBaseInfoIn = queryBaseInfoIn.Where(x => x.EntryOrOutDate < uiSearchObj.EndDate);
                queryBaseInfoOut = queryBaseInfoOut.Where(x => x.EntryOrOutDate < uiSearchObj.EndDate);
            }

            var queryResult = queryBaseInfoIn.Union(queryBaseInfoOut);

            totalRecords = queryResult.Count();
            queryResult = queryResult.OrderBy(x => x.EntryOrOutDate).Skip(pageSize * pageIndex).Take(pageSize);

            result = queryResult.ToList();
            result.ForEach(x =>
            {
                decimal? decimalNull = null;
                x.InNumberOfPackages = x.InQty.HasValue ? (x.InQty.Value.ToDecimal() / x.NumberInLargePackage.ToDecimal()) : decimalNull;
                x.OutNumberOfPackages = x.OutQty.HasValue ? (x.OutQty.Value.ToDecimal() / x.NumberInLargePackage.ToDecimal()) : decimalNull;
            });
        }


        public IList<UIInventorySummaryReport> GetInventorySummaryReport(UISearchInventorySummaryReport uiSearchObj)
        {
            List<UIInventorySummaryReport> result = null;
            int totalRecords = 0;
            BuildInventorySummaryReport(uiSearchObj, 0, 10000000, out totalRecords, out result);
            return result;
        }

        public IList<UIInventorySummaryDetailReport> GetInventorySummaryDetailReport(UISearchInventorySummaryDetailReport uiSearchObj)
        {
            List<UIInventorySummaryDetailReport> result = null;
            int totalRecords = 0;
            BuildInventorySummaryDetailReport(uiSearchObj, 0, 10000000, out totalRecords, out result);
            return result;
        }
    }
}
