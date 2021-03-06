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
    public interface IStockOutDetailRepository : IBaseRepository<StockOutDetail>
    {
        /// <summary>
        /// 获取UI List，不分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>IList{UIStockOutDetail}.</returns>
        IList<UIStockOutDetail> GetUIList(UISearchStockOutDetail uiSearchObj = null);

        /// <summary>
        /// 获取UI List，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UIStockOutDetail}.</returns>
        IList<UIStockOutDetail> GetUIList(UISearchStockOutDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
        /// <summary>
        /// 获取短信提醒的数据
        /// </summary>
        /// <param name="uiSearchObj"></param>
        /// <returns></returns>
        IList<UIStockOutDetail> GetUIListForSmsReminder(UISearchStockOutDetail uiSearchObj);
        /// <summary>
        /// 获取 供新增客户发票管理的  出库单 货品列表
        /// </summary>
        /// <param name="uiSearchObj"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        IList<UIStockOutDetail> GetClientInvoiceChooseStockOutDetailUIList(UISearchStockOutDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
        /// <summary>
        /// 获取 供新增大包客户发票管理的  出库单 货品列表
        /// </summary>
        /// <param name="uiSearchObj"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        IList<UIStockOutDetail> GetDBClientInvoiceChooseStockOutDetailUIList(UISearchStockOutDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

    }
}
