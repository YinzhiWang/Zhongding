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
    public interface IUsersRepository : IBaseRepository<Users>, IGenerateDropdownItems
    {
        /// <summary>
        /// 获取UI List，不分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>IList{UIUser}.</returns>
        IList<UIUser> GetUIList(UISearchUser uiSearchObj = null);

        /// <summary>
        /// 获取UI List，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UIUser}.</returns>
        IList<UIUser> GetUIList(UISearchUser uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 根据ProviderUserKey获取对应的User
        /// </summary>
        /// <param name="providerUserKey">The provider user key.</param>
        /// <returns>Users.</returns>
        Users GetByProviderUserKey(Guid providerUserKey);

        /// <summary>
        /// 根据UserName获取对应的User
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Users.</returns>
        Users GetByUserName(string userName);

        /// <summary>
        /// 根据UserID获取对应的UserName
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns>System.String.</returns>
        string GetUserNameByID(int userID);
    }
}
