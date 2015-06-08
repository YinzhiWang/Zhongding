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
using System.Linq.Expressions;

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

        #region 大包客户季度考核表

        public IList<UIDBClientQuarterlyAssessmentReport> GetDBClientQuarterlyAssessmentReport(UISearchDBClientQuarterlyAssessmentReport uiSearchObj)
        {
            IList<UIDBClientQuarterlyAssessmentReport> uiEntities = new List<UIDBClientQuarterlyAssessmentReport>();

            if (uiSearchObj.Year > 0 && uiSearchObj.Quarter > 0)
            {
                int beginMonth = 1;
                int endMonth = 3;

                EQuarter quarter = (EQuarter)uiSearchObj.Quarter;

                switch (quarter)
                {
                    case EQuarter.Quarter1:
                        beginMonth = (int)EMonthOfYear.January;
                        endMonth = (int)EMonthOfYear.March;
                        break;
                    case EQuarter.Quarter2:
                        beginMonth = (int)EMonthOfYear.April;
                        endMonth = (int)EMonthOfYear.June;
                        break;
                    case EQuarter.Quarter3:
                        beginMonth = (int)EMonthOfYear.July;
                        endMonth = (int)EMonthOfYear.September;
                        break;
                    case EQuarter.Quarter4:
                        beginMonth = (int)EMonthOfYear.October;
                        endMonth = (int)EMonthOfYear.December;
                        break;
                }

                DateTime firstMonth = new DateTime(uiSearchObj.Year, beginMonth, 1);
                DateTime secondMonth = firstMonth.AddMonths(1);
                DateTime thirdMonth = firstMonth.AddMonths(2);
                DateTime endOfQuarter = thirdMonth.AddMonths(1);

                uiEntities = (from dbc in DB.DBContract
                              join dbch in DB.DBContractHospital on dbc.ID equals dbch.DBContractID
                              join p in DB.Product on dbc.ProductID equals p.ID
                              join ps in DB.ProductSpecification on dbc.ProductSpecificationID equals ps.ID
                              join cu in DB.ClientUser on dbc.ClientUserID equals cu.ID
                              join ht in DB.HospitalType on dbc.HospitalTypeID equals ht.ID
                              join h in DB.Hospital on dbch.HospitalID equals h.ID
                              where dbc.IsDeleted == false && dbc.IsTempContract != true
                              && (!uiSearchObj.ClientUserID.HasValue || dbc.ClientUserID == uiSearchObj.ClientUserID)
                              select new UIDBClientQuarterlyAssessmentReport
                              {
                                  ClientUserName = cu.ClientName,
                                  HospitalType = ht.TypeName,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  HospitalName = h.HospitalName,
                                  PromotionExpense = dbc.PromotionExpense,
                                  QuarterTaskAssignment = DB.DBContractTaskAssignment.Any(x => x.DBContractID == dbc.ID && x.MonthOfTask >= beginMonth && x.MonthOfTask <= endMonth)
                                  ? DB.DBContractTaskAssignment.Where(x => x.DBContractID == dbc.ID && x.MonthOfTask >= beginMonth && x.MonthOfTask <= endMonth)
                                     .Sum(x => x.Quantity ?? 0) : 0,
                                  FirstMonthSalesQty = (from fdd in DB.DCFlowDataDetail
                                                        join fd in DB.DCFlowData on fdd.DCFlowDataID equals fd.ID
                                                        where fd.IsCorrectlyFlow == true && fdd.DBContractID == dbc.ID
                                                        && fd.ProductID == dbc.ProductID && fd.ProductSpecificationID == dbc.ProductSpecificationID
                                                        && fdd.ClientUserID == dbc.ClientUserID
                                                        && fdd.HospitalID == dbch.HospitalID && fdd.SaleDate >= firstMonth
                                                        && fdd.SaleDate < secondMonth
                                                        select new { fdd.SaleQty }).Any(x => x.SaleQty > 0) ?
                                                        (from fdd in DB.DCFlowDataDetail
                                                         join fd in DB.DCFlowData on fdd.DCFlowDataID equals fd.ID
                                                         where fd.IsCorrectlyFlow == true && fdd.DBContractID == dbc.ID
                                                         && fd.ProductID == dbc.ProductID && fd.ProductSpecificationID == dbc.ProductSpecificationID
                                                         && fdd.ClientUserID == dbc.ClientUserID
                                                         && fdd.HospitalID == dbch.HospitalID && fdd.SaleDate >= firstMonth
                                                         && fdd.SaleDate < secondMonth
                                                         select new { fdd.SaleQty }).Sum(x => x.SaleQty) : 0,

                                  SecondMonthSalesQty = (from fdd in DB.DCFlowDataDetail
                                                         join fd in DB.DCFlowData on fdd.DCFlowDataID equals fd.ID
                                                         where fd.IsCorrectlyFlow == true && fdd.DBContractID == dbc.ID
                                                         && fd.ProductID == dbc.ProductID && fd.ProductSpecificationID == dbc.ProductSpecificationID
                                                         && fdd.ClientUserID == dbc.ClientUserID
                                                         && fdd.HospitalID == dbch.HospitalID && fdd.SaleDate >= secondMonth
                                                         && fdd.SaleDate < thirdMonth
                                                         select new { fdd.SaleQty }).Any(x => x.SaleQty > 0) ?
                                                        (from fdd in DB.DCFlowDataDetail
                                                         join fd in DB.DCFlowData on fdd.DCFlowDataID equals fd.ID
                                                         where fd.IsCorrectlyFlow == true && fdd.DBContractID == dbc.ID
                                                         && fd.ProductID == dbc.ProductID && fd.ProductSpecificationID == dbc.ProductSpecificationID
                                                         && fdd.ClientUserID == dbc.ClientUserID
                                                         && fdd.HospitalID == dbch.HospitalID && fdd.SaleDate >= secondMonth
                                                         && fdd.SaleDate < thirdMonth
                                                         select new { fdd.SaleQty }).Sum(x => x.SaleQty) : 0,
                                  ThirdMonthSalesQty = (from fdd in DB.DCFlowDataDetail
                                                        join fd in DB.DCFlowData on fdd.DCFlowDataID equals fd.ID
                                                        where fd.IsCorrectlyFlow == true && fdd.DBContractID == dbc.ID
                                                        && fd.ProductID == dbc.ProductID && fd.ProductSpecificationID == dbc.ProductSpecificationID
                                                        && fdd.ClientUserID == dbc.ClientUserID
                                                        && fdd.HospitalID == dbch.HospitalID && fdd.SaleDate >= thirdMonth
                                                        && fdd.SaleDate < endOfQuarter
                                                        select new { fdd.SaleQty }).Any(x => x.SaleQty > 0) ?
                                                        (from fdd in DB.DCFlowDataDetail
                                                         join fd in DB.DCFlowData on fdd.DCFlowDataID equals fd.ID
                                                         where fd.IsCorrectlyFlow == true && fdd.DBContractID == dbc.ID
                                                         && fd.ProductID == dbc.ProductID && fd.ProductSpecificationID == dbc.ProductSpecificationID
                                                         && fdd.ClientUserID == dbc.ClientUserID
                                                         && fdd.HospitalID == dbch.HospitalID && fdd.SaleDate >= thirdMonth
                                                         && fdd.SaleDate < endOfQuarter
                                                         select new { fdd.SaleQty }).Sum(x => x.SaleQty) : 0,


                              }).ToList();

                foreach (var uiEntity in uiEntities)
                {
                    var totalQuarterQty = uiEntity.FirstMonthSalesQty + uiEntity.SecondMonthSalesQty + uiEntity.ThirdMonthSalesQty;
                    uiEntity.QuarterAmount = totalQuarterQty * uiEntity.PromotionExpense ?? 0M;

                    if (totalQuarterQty > 0
                        && totalQuarterQty < uiEntity.QuarterTaskAssignment)
                    {
                        uiEntity.RewardRate = -0.05M;

                        uiEntity.RewardAmount = uiEntity.QuarterAmount * uiEntity.RewardRate;
                    }
                }
            }

            return uiEntities;
        }

        public IList<UIDBClientQuarterlyAssessmentReport> GetDBClientQuarterlyAssessmentReport(UISearchDBClientQuarterlyAssessmentReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDBClientQuarterlyAssessmentReport> uiEntities = new List<UIDBClientQuarterlyAssessmentReport>();

            int total = 0;

            if (uiSearchObj.Year > 0 && uiSearchObj.Quarter > 0)
            {
                int beginMonth = 1;
                int endMonth = 3;

                EQuarter quarter = (EQuarter)uiSearchObj.Quarter;

                switch (quarter)
                {
                    case EQuarter.Quarter1:
                        beginMonth = (int)EMonthOfYear.January;
                        endMonth = (int)EMonthOfYear.March;
                        break;
                    case EQuarter.Quarter2:
                        beginMonth = (int)EMonthOfYear.April;
                        endMonth = (int)EMonthOfYear.June;
                        break;
                    case EQuarter.Quarter3:
                        beginMonth = (int)EMonthOfYear.July;
                        endMonth = (int)EMonthOfYear.September;
                        break;
                    case EQuarter.Quarter4:
                        beginMonth = (int)EMonthOfYear.October;
                        endMonth = (int)EMonthOfYear.December;
                        break;
                }

                DateTime firstMonth = new DateTime(uiSearchObj.Year, beginMonth, 1);
                DateTime secondMonth = firstMonth.AddMonths(1);
                DateTime thirdMonth = firstMonth.AddMonths(2);
                DateTime endOfQuarter = thirdMonth.AddMonths(1);

                var query = (from dbc in DB.DBContract
                             join dbch in DB.DBContractHospital on dbc.ID equals dbch.DBContractID
                             join p in DB.Product on dbc.ProductID equals p.ID
                             join ps in DB.ProductSpecification on dbc.ProductSpecificationID equals ps.ID
                             join cu in DB.ClientUser on dbc.ClientUserID equals cu.ID
                             join ht in DB.HospitalType on dbc.HospitalTypeID equals ht.ID
                             join h in DB.Hospital on dbch.HospitalID equals h.ID
                             where dbc.IsDeleted == false && dbc.IsTempContract != true
                             && (!uiSearchObj.ClientUserID.HasValue || dbc.ClientUserID == uiSearchObj.ClientUserID)
                             select new UIDBClientQuarterlyAssessmentReport
                             {
                                 ClientUserName = cu.ClientName,
                                 HospitalType = ht.TypeName,
                                 ProductName = p.ProductName,
                                 Specification = ps.Specification,
                                 HospitalName = h.HospitalName,
                                 PromotionExpense = dbc.PromotionExpense,
                                 QuarterTaskAssignment = DB.DBContractTaskAssignment.Any(x => x.DBContractID == dbc.ID && x.MonthOfTask >= beginMonth && x.MonthOfTask <= endMonth)
                                 ? DB.DBContractTaskAssignment.Where(x => x.DBContractID == dbc.ID && x.MonthOfTask >= beginMonth && x.MonthOfTask <= endMonth)
                                    .Sum(x => x.Quantity ?? 0) : 0,
                                 FirstMonthSalesQty = (from fdd in DB.DCFlowDataDetail
                                                       join fd in DB.DCFlowData on fdd.DCFlowDataID equals fd.ID
                                                       where fd.IsCorrectlyFlow == true && fdd.DBContractID == dbc.ID
                                                       && fd.ProductID == dbc.ProductID && fd.ProductSpecificationID == dbc.ProductSpecificationID
                                                       && fdd.ClientUserID == dbc.ClientUserID
                                                       && fdd.HospitalID == dbch.HospitalID && fdd.SaleDate >= firstMonth
                                                       && fdd.SaleDate < secondMonth
                                                       select new { fdd.SaleQty }).Any(x => x.SaleQty > 0) ?
                                                       (from fdd in DB.DCFlowDataDetail
                                                        join fd in DB.DCFlowData on fdd.DCFlowDataID equals fd.ID
                                                        where fd.IsCorrectlyFlow == true && fdd.DBContractID == dbc.ID
                                                        && fd.ProductID == dbc.ProductID && fd.ProductSpecificationID == dbc.ProductSpecificationID
                                                        && fdd.ClientUserID == dbc.ClientUserID
                                                        && fdd.HospitalID == dbch.HospitalID && fdd.SaleDate >= firstMonth
                                                        && fdd.SaleDate < secondMonth
                                                        select new { fdd.SaleQty }).Sum(x => x.SaleQty) : 0,

                                 SecondMonthSalesQty = (from fdd in DB.DCFlowDataDetail
                                                        join fd in DB.DCFlowData on fdd.DCFlowDataID equals fd.ID
                                                        where fd.IsCorrectlyFlow == true && fdd.DBContractID == dbc.ID
                                                        && fd.ProductID == dbc.ProductID && fd.ProductSpecificationID == dbc.ProductSpecificationID
                                                        && fdd.ClientUserID == dbc.ClientUserID
                                                        && fdd.HospitalID == dbch.HospitalID && fdd.SaleDate >= secondMonth
                                                        && fdd.SaleDate < thirdMonth
                                                        select new { fdd.SaleQty }).Any(x => x.SaleQty > 0) ?
                                                       (from fdd in DB.DCFlowDataDetail
                                                        join fd in DB.DCFlowData on fdd.DCFlowDataID equals fd.ID
                                                        where fd.IsCorrectlyFlow == true && fdd.DBContractID == dbc.ID
                                                        && fd.ProductID == dbc.ProductID && fd.ProductSpecificationID == dbc.ProductSpecificationID
                                                        && fdd.ClientUserID == dbc.ClientUserID
                                                        && fdd.HospitalID == dbch.HospitalID && fdd.SaleDate >= secondMonth
                                                        && fdd.SaleDate < thirdMonth
                                                        select new { fdd.SaleQty }).Sum(x => x.SaleQty) : 0,
                                 ThirdMonthSalesQty = (from fdd in DB.DCFlowDataDetail
                                                       join fd in DB.DCFlowData on fdd.DCFlowDataID equals fd.ID
                                                       where fd.IsCorrectlyFlow == true && fdd.DBContractID == dbc.ID
                                                       && fd.ProductID == dbc.ProductID && fd.ProductSpecificationID == dbc.ProductSpecificationID
                                                       && fdd.ClientUserID == dbc.ClientUserID
                                                       && fdd.HospitalID == dbch.HospitalID && fdd.SaleDate >= thirdMonth
                                                       && fdd.SaleDate < endOfQuarter
                                                       select new { fdd.SaleQty }).Any(x => x.SaleQty > 0) ?
                                                       (from fdd in DB.DCFlowDataDetail
                                                        join fd in DB.DCFlowData on fdd.DCFlowDataID equals fd.ID
                                                        where fd.IsCorrectlyFlow == true && fdd.DBContractID == dbc.ID
                                                        && fd.ProductID == dbc.ProductID && fd.ProductSpecificationID == dbc.ProductSpecificationID
                                                        && fdd.ClientUserID == dbc.ClientUserID
                                                        && fdd.HospitalID == dbch.HospitalID && fdd.SaleDate >= thirdMonth
                                                        && fdd.SaleDate < endOfQuarter
                                                        select new { fdd.SaleQty }).Sum(x => x.SaleQty) : 0,


                             });

                total = query.Count();

                uiEntities = query.OrderByDescending(x => x.QuarterTaskAssignment)
                    .Skip(pageIndex * pageSize).Take(pageSize).ToList();

                foreach (var uiEntity in uiEntities)
                {
                    var totalQuarterQty = uiEntity.FirstMonthSalesQty + uiEntity.SecondMonthSalesQty + uiEntity.ThirdMonthSalesQty;
                    uiEntity.QuarterAmount = totalQuarterQty * uiEntity.PromotionExpense ?? 0M;

                    if (totalQuarterQty > 0
                        && totalQuarterQty < uiEntity.QuarterTaskAssignment)
                    {
                        uiEntity.RewardRate = -0.05M;

                        uiEntity.RewardAmount = uiEntity.QuarterAmount * uiEntity.RewardRate;
                    }
                }
            }

            totalRecords = total;

            return uiEntities;
        }

        #endregion


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
                var queryResult = from q in queryExInfo
                                  select new UIInventorySummaryReport()
                                  {
                                      WarehouseID = q.WarehouseID,
                                      ProductID = q.ProductID,
                                      ProductSpecificationID = q.ProductSpecificationID,
                                      BatchNumber = q.BatchNumber,
                                      LicenseNumber = q.LicenseNumber,
                                      ExpirationDate = q.ExpirationDate,
                                      InQty = q.InQty + q.NewInQty,
                                      OutQty = q.OutQty + q.NewOutQty,
                                      ProcurePrice = q.ProcurePrice,
                                      WarehouseName = q.WarehouseName,
                                      ProductName = q.ProductName,
                                      ProductCode = q.ProductCode,
                                      Specification = q.Specification,
                                      UnitName = q.UnitName,
                                      NumberInLargePackage = q.NumberInLargePackage,
                                      PreBalanceQty = q.PreBalanceQty,
                                      CurrentBalanceQty = q.NewInQty - q.NewOutQty,
                                      NewInQty = q.NewInQty,
                                      NewOutQty = q.NewOutQty
                                  };
                queryResult = queryResult.Where(x => !(x.PreBalanceQty == 0 && x.InQty == 0));
                totalRecords = queryResult.Count();
                queryExInfo = queryResult.OrderBy(x => x.ProductCode).Skip(pageSize * pageIndex).Take(pageSize);
                result = queryExInfo.ToList();

            }

            result.ForEach(x =>
            {
                //这里的 计算转移到上边的 linq中了
                //x.InQty += x.NewInQty;
                //x.OutQty += x.NewOutQty;
                //x.CurrentBalanceQty += (x.NewInQty - x.NewOutQty);

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
                uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
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

        public IList<UIDCFlowSettlementReport> GetDCFlowSettlementReport(UISearchDCFlowSettlementReport uiSearchObj)
        {
            IList<UIDCFlowSettlementReport> uiEntities = new List<UIDCFlowSettlementReport>();

            DateTime startDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            DateTime endDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;

            if (uiSearchObj.SaleDate.HasValue)
            {
                startDate = uiSearchObj.SaleDate.Value;
                endDate = startDate.AddMonths(1);
            }

            var query = (from fdd in DB.DCFlowDataDetail
                         join fd in DB.DCFlowData on fdd.DCFlowDataID equals fd.ID
                         join dbc in DB.DBContract on fdd.DBContractID equals dbc.ID
                         join dc in DB.DistributionCompany on fd.DistributionCompanyID equals dc.ID
                         join p in DB.Product on fd.ProductID equals p.ID
                         join ps in DB.ProductSpecification on fd.ProductSpecificationID equals ps.ID
                         join cu in DB.ClientUser on dbc.ClientUserID equals cu.ID
                         join h in DB.Hospital on fdd.HospitalID equals h.ID
                         join ht in DB.HospitalType on dbc.HospitalTypeID equals ht.ID
                         where fd.IsCorrectlyFlow == true && fdd.SaleDate >= startDate
                         && fdd.SaleDate < endDate
                         select new
                         {
                             ClientUserName = cu.ClientName,
                             HospitalTypeName = ht.TypeName,
                             ProductName = p.ProductName,
                             Specification = ps.Specification,
                             HospitalName = h.HospitalName,
                             SaleQty = fdd.SaleQty,
                             SaleDate = fdd.SaleDate,
                             DistributionCompanyName = dc.Name,
                             PromotionExpense = fdd.SaleQty * (dbc.PromotionExpense ?? 0)
                         })
                        .GroupBy(x => new { x.ClientUserName, x.HospitalTypeName, x.ProductName, x.Specification, x.HospitalName, x.SaleDate, x.DistributionCompanyName })
                        .Select(g => new UIDCFlowSettlementReport
                        {
                            ClientUserName = g.Key.ClientUserName,
                            HospitalTypeName = g.Key.HospitalTypeName,
                            ProductName = g.Key.ProductName,
                            Specification = g.Key.Specification,
                            HospitalName = g.Key.HospitalName,
                            SaleQty = g.Sum(x => x.SaleQty),
                            SaleDate = g.Key.SaleDate,
                            DistributionCompanyName = g.Key.DistributionCompanyName,
                            TotalPromotionExpense = g.Sum(x => x.PromotionExpense)
                        });


            uiEntities = query.ToList();

            return uiEntities;
        }

        public IList<UIDCFlowSettlementReport> GetDCFlowSettlementReport(UISearchDCFlowSettlementReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDCFlowSettlementReport> uiEntities = new List<UIDCFlowSettlementReport>();

            DateTime startDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            DateTime endDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;

            if (uiSearchObj.SaleDate.HasValue)
            {
                startDate = uiSearchObj.SaleDate.Value;
                endDate = startDate.AddMonths(1);
            }

            var query = (from fdd in DB.DCFlowDataDetail
                         join fd in DB.DCFlowData on fdd.DCFlowDataID equals fd.ID
                         join dbc in DB.DBContract on fdd.DBContractID equals dbc.ID
                         join dc in DB.DistributionCompany on fd.DistributionCompanyID equals dc.ID
                         join p in DB.Product on fd.ProductID equals p.ID
                         join ps in DB.ProductSpecification on fd.ProductSpecificationID equals ps.ID
                         join cu in DB.ClientUser on dbc.ClientUserID equals cu.ID
                         join h in DB.Hospital on fdd.HospitalID equals h.ID
                         join ht in DB.HospitalType on dbc.HospitalTypeID equals ht.ID
                         where fd.IsCorrectlyFlow == true && fdd.SaleDate >= startDate
                         && fdd.SaleDate < endDate
                         select new
                         {
                             ClientUserName = cu.ClientName,
                             HospitalTypeName = ht.TypeName,
                             ProductName = p.ProductName,
                             Specification = ps.Specification,
                             HospitalName = h.HospitalName,
                             SaleQty = fdd.SaleQty,
                             SaleDate = fdd.SaleDate,
                             DistributionCompanyName = dc.Name,
                             PromotionExpense = fdd.SaleQty * (dbc.PromotionExpense ?? 0)
                         })
                        .GroupBy(x => new { x.ClientUserName, x.HospitalTypeName, x.ProductName, x.Specification, x.HospitalName, x.SaleDate, x.DistributionCompanyName })
                        .Select(g => new UIDCFlowSettlementReport
                        {
                            ClientUserName = g.Key.ClientUserName,
                            HospitalTypeName = g.Key.HospitalTypeName,
                            ProductName = g.Key.ProductName,
                            Specification = g.Key.Specification,
                            HospitalName = g.Key.HospitalName,
                            SaleQty = g.Sum(x => x.SaleQty),
                            SaleDate = g.Key.SaleDate,
                            DistributionCompanyName = g.Key.DistributionCompanyName,
                            TotalPromotionExpense = g.Sum(x => x.PromotionExpense)
                        });

            totalRecords = query.Count();

            uiEntities = query.OrderBy(x => x.SaleDate)
                .Skip(pageSize * pageIndex).Take(pageSize).ToList();

            return uiEntities;
        }



        /// <summary>
        /// 大包客户结算
        /// </summary>
        /// <param name="uiSearchObj"></param>
        /// <returns></returns>
        public IList<UIDBClientSettlementReport> GetDBClientSettlementReport(UISearchDBClientSettlementReport uiSearchObj)
        {
            List<UIDBClientSettlementReport> result = null;
            int totalRecords = 0;
            BuildDBClientSettlementReport(uiSearchObj, 0, 10000000, out totalRecords, out result);
            return result;
        }

        private void BuildDBClientSettlementReport(UISearchDBClientSettlementReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords, out List<UIDBClientSettlementReport> result)
        {
            totalRecords = 0;
            result = new List<UIDBClientSettlementReport>();
            IList<UIDBClientSettlementReport> uiEntities = new List<UIDBClientSettlementReport>();
            int total = 0;
            IQueryable<DBClientSettleBonus> query = DB.Set<DBClientSettleBonus>(); ;
            var whereFuncs = new List<Expression<Func<DBClientSettleBonus, bool>>>();

            if (uiSearchObj != null)
            {
                whereFuncs.Add(x => x.IsDeleted == false);

                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DBClientSettlementID > 0)
                    whereFuncs.Add(x => x.DBClientSettlementID.Equals(uiSearchObj.DBClientSettlementID));


                if (uiSearchObj.ClientUserID.BiggerThanZero())
                {
                    whereFuncs.Add(x => x.DBClientBonus.ClientUserID == uiSearchObj.ClientUserID);
                }
                if (uiSearchObj.SettlementDate.HasValue)
                {
                    whereFuncs.Add(x => x.DBClientSettlement.SettlementDate == uiSearchObj.SettlementDate);
                }
                if (uiSearchObj.IncludeWorkflowStatusIDs != null)
                {
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.DBClientSettlement.WorkflowStatusID));
                }
            }
            foreach (var item in whereFuncs)
            {
                query = query.Where(item);
            }
            total = query.Count();

            if (query != null)
            {
                uiEntities = (from q in query
                              join cb in DB.DBClientBonus on q.DBClientBonusID equals cb.ID
                              join p in DB.Product on cb.ProductID equals p.ID
                              join ps in DB.ProductSpecification on cb.ProductSpecificationID equals ps.ID
                              join cu in DB.ClientUser on cb.ClientUserID equals cu.ID
                              join ba in DB.BankAccount on cu.DBBankAccountID equals ba.ID into tempBA
                              from tba in tempBA.DefaultIfEmpty()
                              select new UIDBClientSettlementReport()
                              {
                                  ID = q.ID,
                                  ClientUserName = cu.ClientName,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  SettlementDate = q.DBClientSettlement.SettlementDate,
                                  IsNeedSettlement = q.IsNeedSettlement,
                                  BonusAmount = cb.BonusAmount,
                                  PerformanceAmount = cb.PerformanceAmount,
                                  TotalPayAmount = q.TotalPayAmount,
                                  IsSettled = cb.IsSettled,
                                  IsManualSettled = q.IsManualSettled,
                                  ClientDBBankAccount = tba != null
                                    ? (tba.AccountName + " " + tba.BankBranchName + " " + tba.Account)
                                    : string.Empty,
                                  HospitalType = q.DBClientSettlement.HospitalType.TypeName,
                                  SaleQty = cb.SaleQty,
                                  PromotionExpense = cb.PromotionExpense,
                              }).ToList();

                result = uiEntities.ToList();
            }

            totalRecords = total;


        }



        public IList<UIDBClientSettlementReport> GetDBClientSettlementReport(UISearchDBClientSettlementReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIDBClientSettlementReport> result = null;
            BuildDBClientSettlementReport(uiSearchObj, pageIndex, pageSize, out totalRecords, out result);
            return result;
        }


        public IList<UISupplierTaskReport> GetSupplierTaskReport(UISearchSupplierTaskReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UISupplierTaskReport> result = null;
            BuildSupplierTaskReport(uiSearchObj, pageIndex, pageSize, out totalRecords, out result);
            return result;
        }
        public IList<UISupplierTaskReport> GetSupplierTaskReport(UISearchSupplierTaskReport uiSearchObj)
        {
            List<UISupplierTaskReport> result = null;
            int totalRecords = 0;
            BuildSupplierTaskReport(uiSearchObj, 0, 10000000, out totalRecords, out result);
            return result;
        }
        private void BuildSupplierTaskReport(UISearchSupplierTaskReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords, out List<UISupplierTaskReport> result)
        {
            totalRecords = 0;
            result = new List<UISupplierTaskReport>();
            List<UISupplierTaskReport> uiEntities = new List<UISupplierTaskReport>();
            int total = 0;
            DateTime? date = uiSearchObj.Date;
            DateTime? now = DateTime.Now;
            int yearOfTask = date.Value.Year;
            int monthOfTask = date.Value.Month;
            DateTime validDate = new DateTime(yearOfTask, monthOfTask, 1);
            DateTime validDateEnd = validDate.AddMonths(1);
            var query = from supplierContract in DB.SupplierContract
                        join supplier in DB.Supplier on supplierContract.SupplierID equals supplier.ID
                        join p in DB.Product on supplierContract.ProductID equals p.ID
                        join ps in DB.ProductSpecification on supplierContract.ProductSpecificationID equals ps.ID
                        where supplierContract.IsDeleted == false
                        && supplier.IsDeleted == false
                        && supplier.CompanyID == uiSearchObj.CompanyID
                        && supplierContract.IsNeedTaskAssignment == true
                        && supplierContract.ExpirationDate > now && supplierContract.ExpirationDate > validDate && supplierContract.StartDate <= now && supplierContract.StartDate < validDateEnd
                        select new UISupplierTaskReport()
                        {
                            ID = supplierContract.ID,
                            ProductID = p.ID,
                            ProductName = p.ProductName,
                            SupplierID = supplierContract.SupplierID,
                            SupplierName = supplier.SupplierName,
                            ProductSpecificationID = ps.ID,
                            Specification = ps.Specification,
                            TaskQuantity =
                                DB.SupplierTaskAssignment.Where(x => x.IsDeleted == false
                                && x.SupplierID == supplierContract.SupplierID && x.ContractID == supplierContract.ID
                                && x.YearOfTask == yearOfTask && x.MonthOfTask == monthOfTask).Any() ?
                                DB.SupplierTaskAssignment.Where(x => x.IsDeleted == false
                                && x.SupplierID == supplierContract.SupplierID && x.ContractID == supplierContract.ID
                                && x.YearOfTask == yearOfTask && x.MonthOfTask == monthOfTask).FirstOrDefault().Quantity : 0,
                            AlreadyProcureCount = 0
                        };
            if (uiSearchObj.SupplierID.BiggerThanZero())
            {
                query = query.Where(x => x.SupplierID == uiSearchObj.SupplierID);
            }
            //query = query.Where(x => x.TaskQuantity > 0);


            var queryResult = from q in query
                              select new UISupplierTaskReport()
                              {
                                  ID = q.ID,
                                  ProductID = q.ProductID,
                                  ProductName = q.ProductName,
                                  SupplierID = q.SupplierID,
                                  SupplierName = q.SupplierName,
                                  ProductSpecificationID = q.ProductSpecificationID,
                                  Specification = q.Specification,
                                  TaskQuantity = q.TaskQuantity,
                                  AlreadyProcureCount = 0
                                  //AlreadyProcureCount =
                                  //    DB.ProcureOrderAppDetail.Where(x => x.IsDeleted == false && x.ProcureOrderApplication.IsDeleted == false
                                  //    && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID
                                  //    && x.ProcureOrderApplication.WorkflowStatusID == (int)EWorkflowStatus.Completed
                                  //    && x.ProcureOrderApplication.EstDeliveryDate >= start && x.ProcureOrderApplication.EstDeliveryDate < end).Any() ?
                                  //    DB.ProcureOrderAppDetail.Where(x => x.IsDeleted == false && x.ProcureOrderApplication.IsDeleted == false
                                  //    && x.ProductID == q.ProductID && x.ProductSpecificationID == q.ProductSpecificationID
                                  //    && x.ProcureOrderApplication.WorkflowStatusID == (int)EWorkflowStatus.Completed
                                  //    && x.ProcureOrderApplication.EstDeliveryDate >= start && x.ProcureOrderApplication.EstDeliveryDate < end).Sum(x => x.ProcureCount) : 0M
                              };

            total = queryResult.Count();
            uiEntities = queryResult.OrderBy(x => x.ID).Skip(pageSize * pageIndex).Take(pageSize).ToList();
            uiEntities.ForEach(x =>
            {
                x.Date = uiSearchObj.Date;
                x.AlreadyProcureCount = GetAlreadyProcureCountForSupplierTaskReport(x.ProductID, x.ProductSpecificationID, uiSearchObj.Date.Value);
            });
            result = uiEntities;
            totalRecords = total;
        }

        private int GetAlreadyProcureCountForSupplierTaskReport(int productID, int productSpecificationID, DateTime date)
        {
            //仅仅计算 交货日期是 当前月的
            DateTime start = new DateTime(date.Year, date.Month, 1);
            DateTime end = start.AddMonths(1);
            List<int> needStatus = new List<int>() { (int)EWorkflowStatus.Paid, (int)EWorkflowStatus.Shipping, (int)EWorkflowStatus.Completed };
            var query = from procureOrderAppDetail in DB.ProcureOrderAppDetail
                        join procureOrderApplication in DB.ProcureOrderApplication on procureOrderAppDetail.ProcureOrderApplicationID equals procureOrderApplication.ID
                        where procureOrderAppDetail.IsDeleted == false
                        && procureOrderApplication.IsDeleted == false
                        && procureOrderAppDetail.ProductID == productID
                        && procureOrderAppDetail.ProductSpecificationID == productSpecificationID
                        && needStatus.Contains(procureOrderApplication.WorkflowStatusID)
                        && procureOrderApplication.OrderDate >= start && procureOrderApplication.OrderDate < end
                        select new
                        {
                            ProcureOrderApplicationID = procureOrderApplication.ID,
                            ProcureOrderAppDetailID = procureOrderAppDetail.ID,
                            ProcureCount = procureOrderAppDetail.ProcureCount,
                            IsStop = procureOrderApplication.IsStop,
                            AlreadyInQty = procureOrderApplication.IsStop ? (
                                DB.StockInDetail.Where(x => x.IsDeleted == false
                                && x.StockIn.IsDeleted == false
                                && x.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse
                                && x.ProcureOrderAppDetailID == procureOrderAppDetail.ID).Any() ?
                                DB.StockInDetail.Where(x => x.IsDeleted == false
                                && x.StockIn.IsDeleted == false
                                && x.StockIn.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse
                                && x.ProcureOrderAppDetailID == procureOrderAppDetail.ID).Sum(x => x.InQty)
                                : 0) : 0
                        };
            var tempResult = query.ToList();
            int count = 0;
            tempResult.ForEach(x =>
            {
                if (x.IsStop)
                {
                    count += x.AlreadyInQty;
                }
                else
                {
                    count += x.ProcureCount;
                }
            });

            return count;
        }


    }
}
