﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IInventoryHistoryRepository : IBaseRepository<InventoryHistory>
    {
        /// <summary>
        /// 获取InventoryHistory的数据条数
        /// </summary>
        int GetCount();

        /// <summary>
        /// 根据统计日期计算库存数据
        /// </summary>
        /// <param name="statDate">The stat date.</param>
        /// <returns>List{UIInventoryHistory}.</returns>
        List<UIInventoryHistory> CalculateInventoryByStatDate(DateTime statDate);

        IList<UIInventoryHistory> GetUIList(Domain.UISearchObjects.UISearchInventoryHistory uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
        /// <summary>
        /// 获取 库存数量
        /// </summary>
        /// <param name="warehouseID"></param>
        /// <param name="productID"></param>
        /// <param name="productSpecificationID"></param>
        /// <returns></returns>
        int GetProductBalanceQty(int warehouseID,int productID,int productSpecificationID);
    }
}
