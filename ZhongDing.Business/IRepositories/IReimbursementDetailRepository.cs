using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IReimbursementDetailRepository : IBaseRepository<ReimbursementDetail>
    {
        IList<UIReimbursementDetail> GetUIList(Domain.UISearchObjects.UISearchReimbursementDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
    }
}
