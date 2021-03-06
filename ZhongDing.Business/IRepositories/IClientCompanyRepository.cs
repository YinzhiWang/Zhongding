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
    public interface IClientCompanyRepository : IBaseRepository<ClientCompany>, IGenerateDropdownItems
    {
        /// <summary>
        /// 获取UI List，不分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>IList{UIClientCompany}.</returns>
        IList<UIClientCompany> GetUIList(UISearchClientCompany uiSearchObj = null);

        /// <summary>
        /// 获取UI List，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UIClientCompany}.</returns>
        IList<UIClientCompany> GetUIList(UISearchClientCompany uiSearchObj, int pageIndex, int pageSize, out int totalRecords);


        ClientCompany GetClientCompanyByClientCompanyNameAndClientUserID(string clientCompanyName, int clientUserID);
    }
}
