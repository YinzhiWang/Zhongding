using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

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

        #endregion

    }
}
