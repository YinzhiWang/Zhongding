using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IClientSaleApplicationImportFileLogRepository : IBaseRepository<ClientSaleApplicationImportFileLog>
    {
        IList<UIClientSaleApplicationImportFileLog> GetUIList(Domain.UISearchObjects.UISearchClientSaleApplicationImportFileLog uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
    }
}
