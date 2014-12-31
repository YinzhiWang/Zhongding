using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;

namespace ZhongDing.Business.Repositories
{
    public class SalesOrderApplicationRepository : BaseRepository<SalesOrderApplication>, ISalesOrderApplicationRepository
    {
        public int? GetMaxEntityID()
        {
            if (this.DB.SalesOrderApplication.Count() > 0)
                return this.DB.SalesOrderApplication.Max(x => x.ID);
            else
                return null;
        }
    }
}
