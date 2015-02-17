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
    public interface ISupplierRefundApplicationRepository : IBaseRepository<SupplierRefundApplication>
    {
        /// <summary>
        /// 获取UI List，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UISupplierRefundApp}.</returns>
        IList<UISupplierRefundApp> GetUIList(UISearchSupplierRefundApp uiSearchObj, int pageIndex, int pageSize, out int totalRecords);


        /// <summary>
        /// 获取Details UI List，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UISupplierRefundAppDetail}.</returns>
        IList<UISupplierRefundAppDetail> GetDetails(UISearchSupplierRefundApp uiSearchObj, int pageIndex, int pageSize, out int totalRecords);


        /// <summary>
        /// 获取任务返款list，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UISupplierRefundApp}.</returns>
        IList<UISupplierRefundApp> GetTaskRefunds(UISearchSupplierRefundApp uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

    }
}
