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
    public interface IClientSaleApplicationRepository : IBaseRepository<ClientSaleApplication>, IAutoSerialNo
    {
        /// <summary>
        /// 获取UI List，不分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>IList{UIClientSaleApplication}.</returns>
        IList<UIClientSaleApplication> GetUIList(UISearchClientSaleApplication uiSearchObj = null);

        /// <summary>
        /// 获取UI List，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UIClientSaleApplication}.</returns>
        IList<UIClientSaleApplication> GetUIList(UISearchClientSaleApplication uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 获取收款数据，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UIClientSaleAppPayment}.</returns>
        IList<UIClientSaleAppPayment> GetPayments(UISearchApplicationPayment uiSearchObj, int pageIndex, int pageSize, out int totalRecords);


    }
}
