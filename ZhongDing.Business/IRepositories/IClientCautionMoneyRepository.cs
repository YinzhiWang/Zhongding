using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IClientCautionMoneyRepository : IBaseRepository<ClientCautionMoney>
    {
        IList<UIClientCautionMoney> GetUIList(Domain.UISearchObjects.UISearchClientCautionMoney uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        UIClientCautionMoney GetUIClientCautionMoneyByID(int id);
   }
}
