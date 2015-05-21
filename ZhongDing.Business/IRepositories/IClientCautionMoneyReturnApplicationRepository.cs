using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IClientCautionMoneyReturnApplicationRepository : IBaseRepository<ClientCautionMoneyReturnApplication>
    {
        IList<UIClientCautionMoneyReturnApplication> GetUIList(Domain.UISearchObjects.UISearchClientCautionMoneyReturnApplication uiSearchObj, int pageIndex, int pageSize, out int totalRecords);


        IList<Domain.UIObjects.UIClientCautionMoneyReturnApplication> GetUIListForClientCautionMoneyReturnApplyManagement(Domain.UISearchObjects.UISearchClientCautionMoneyReturnApplication uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
 
        
    }
}
