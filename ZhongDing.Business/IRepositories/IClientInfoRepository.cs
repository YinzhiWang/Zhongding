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
    public interface IClientInfoRepository : IBaseRepository<ClientInfo>, IAutoSerialNo, IGenerateDropdownItems
    {
        /// <summary>
        /// 获取UI List，不分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>IList{UIClientInfo}.</returns>
        IList<UIClientInfo> GetUIList(UISearchClientInfo uiSearchObj = null);

        /// <summary>
        /// 获取UI List，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UIClientInfo}.</returns>
        IList<UIClientInfo> GetUIList(UISearchClientInfo uiSearchObj, int pageIndex, int pageSize, out int totalRecords);


        /// <summary>
        /// 获取客户的银行账号
        /// </summary>
        /// <param name="clientInfoID">客户ID.</param>
        /// <returns>IList{UIClientInfoBankAccount}.</returns>
        IList<UIClientInfoBankAccount> GetBankAccounts(int? clientInfoID);

        /// <summary>
        /// 获取客户的联系人
        /// </summary>
        /// <param name="clientInfoID">客户ID.</param>
        /// <returns>IList{UIClientInfoContact}.</returns>
        IList<UIClientInfoContact> GetContacts(int? clientInfoID);

        /// <summary>
        /// 获取客户,根据client user id 和 client company id
        /// </summary>
        /// <param name="clientUserID">The client user ID.</param>
        /// <param name="clientCompanyID">The client company ID.</param>
        /// <returns>ClientInfo.</returns>
        ClientInfo GetByConditions(int clientUserID, int clientCompanyID);

    }
}
