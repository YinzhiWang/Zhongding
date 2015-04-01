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
    }
}
