using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IHospitalCodeRepository : IBaseRepository<HospitalCode>
    {
        IList<UIHospitalCode> GetUIList(Domain.UISearchObjects.UISearchHospitalCode uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
    }
}
