﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IProcureOrderAppDetailRepository : IBaseRepository<ProcureOrderAppDetail>
    {
        /// <summary>
        /// 获取UI List，不分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>IList{UIProcureOrderAppDetail}.</returns>
        IList<UIProcureOrderAppDetail> GetUIList(UISearchProcureOrderAppDetail uiSearchObj = null);

        /// <summary>
        /// 获取UI List，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UIProcureOrderAppDetail}.</returns>
        IList<UIProcureOrderAppDetail> GetUIList(UISearchProcureOrderAppDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 获取可入库的列表，不分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UIProcureOrderAppDetail}.</returns>
        IList<UIProcureOrderAppDetail> GetToBeInUIList(UISearchProcureOrderAppDetail uiSearchObj);

        /// <summary>
        /// 获取可入库的列表，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UIProcureOrderAppDetail}.</returns>
        IList<UIProcureOrderAppDetail> GetToBeInUIList(UISearchProcureOrderAppDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
        /// <summary>
        /// 获取供应商发票 新增 选择货品的 List
        /// </summary>
        /// <param name="uiSearchObj"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        IList<UIProcureOrderAppDetail> GetSupplierInvoiceChooseProcureOrderAppDetailUIList(UISearchProcureOrderAppDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

    }
}
