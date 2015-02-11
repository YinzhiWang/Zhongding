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
    public interface IClientRefundApplicationRepository : IBaseRepository<ClientRefundApplication>
    {
        /// <summary>
        /// 获取UI List，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UIClientRefundApplication}.</returns>
        IList<UIClientRefundApplication> GetUIList(UISearchClientRefundApplication uiSearchObj, int pageIndex, int pageSize, out int totalRecords);


        /// <summary>
        /// 获取UI List，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UIClientNeedRefundOrder}.</returns>
        IList<UIClientNeedRefundOrder> GetNeedRefundOrders(UISearchClientNeedRefundOrder uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

    }
}
