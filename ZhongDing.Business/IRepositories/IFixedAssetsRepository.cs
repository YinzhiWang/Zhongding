using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IFixedAssetsRepository : IBaseRepository<FixedAssets>
    {
        IList<UIFixedAssets> GetUIList(Domain.UISearchObjects.UISearchFixedAssets uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
    }
}
