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

        #region Reports

        public IList<UICompany> GetCompanyReport()
        {
            return this.DB.Database.SqlQuery<UICompany>("exec GetCompanyList").ToList();
        }

        public IList<UIProcureOrderReport> GetProcureOrderReport(Domain.UISearchObjects.UISearchProcureOrderReport uiSearchObj)
        {
            SqlParameter totalRecordSqlParameter = new SqlParameter() { ParameterName = "@totalRecord", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.Output };
            List<SqlParameter> parameters = new List<SqlParameter>() {
                new SqlParameter(){ ParameterName="@pageSize",Value=10000000,Size=4},
                new SqlParameter(){ ParameterName="@pageIndex", Value=0,Size=4},
                totalRecordSqlParameter
            };

            string sql = "exec GetProcureOrderReport @pageSize,@pageIndex,@orderDateStart,@orderDateEnd,@supplierId,@productId,@totalRecord out";
            parameters.Add(new SqlParameter() { ParameterName = "@orderDateStart", SqlDbType = SqlDbType.DateTime, Size = 256, Value = uiSearchObj.BeginDate.HasValue ? (object)uiSearchObj.BeginDate.Value : DBNull.Value });

            parameters.Add(new SqlParameter() { ParameterName = "@orderDateEnd", SqlDbType = SqlDbType.DateTime, Size = 256, Value = uiSearchObj.EndDate.HasValue ? (object)uiSearchObj.EndDate.Value : DBNull.Value });

            parameters.Add(new SqlParameter() { ParameterName = "@supplierId", Size = 4, Value = uiSearchObj.SupplierId.HasValue ? (object)uiSearchObj.SupplierId.Value : DBNull.Value });

            parameters.Add(new SqlParameter() { ParameterName = "@productId", Size = 4, Value = uiSearchObj.ProductId.HasValue ? (object)uiSearchObj.ProductId.Value : DBNull.Value });
            var result = this.DB.Database.SqlQuery<UIProcureOrderReport>(sql, parameters.ToArray()).ToList();
            return result;
        }

        public IList<UIProcureOrderReport> GetProcureOrderReport(Domain.UISearchObjects.UISearchProcureOrderReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {

            SqlParameter totalRecordSqlParameter = new SqlParameter() { ParameterName = "@totalRecord", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.Output };
            List<SqlParameter> parameters = new List<SqlParameter>() {
                new SqlParameter(){ ParameterName="@pageSize",Value=pageSize,Size=4},
                new SqlParameter(){ ParameterName="@pageIndex", Value=pageIndex,Size=4},
                totalRecordSqlParameter
            };

            string sql = "exec GetProcureOrderReport @pageSize,@pageIndex,@orderDateStart,@orderDateEnd,@supplierId,@productId,@totalRecord out";
            parameters.Add(new SqlParameter() { ParameterName = "@orderDateStart", SqlDbType = SqlDbType.DateTime, Size = 256, Value = uiSearchObj.BeginDate.HasValue ? (object)uiSearchObj.BeginDate.Value : DBNull.Value });

            parameters.Add(new SqlParameter() { ParameterName = "@orderDateEnd", SqlDbType = SqlDbType.DateTime, Size = 256, Value = uiSearchObj.EndDate.HasValue ? (object)uiSearchObj.EndDate.Value : DBNull.Value });

            parameters.Add(new SqlParameter() { ParameterName = "@supplierId", Size = 4, Value = uiSearchObj.SupplierId.HasValue ? (object)uiSearchObj.SupplierId.Value : DBNull.Value });

            parameters.Add(new SqlParameter() { ParameterName = "@productId", Size = 4, Value = uiSearchObj.ProductId.HasValue ? (object)uiSearchObj.ProductId.Value : DBNull.Value });

            var result = this.DB.Database.SqlQuery<UIProcureOrderReport>(sql, parameters.ToArray()).ToList();
            totalRecords = totalRecordSqlParameter.Value.ToInt();
            return result;
        }
        #endregion



    }
}
