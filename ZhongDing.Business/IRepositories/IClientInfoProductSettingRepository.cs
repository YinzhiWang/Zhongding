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
    public interface IClientInfoProductSettingRepository : IBaseRepository<ClientInfoProductSetting>
    {
        /// <summary>
        /// 获取UI List，不分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>IList{UIClientInfoProductSetting}.</returns>
        IList<UIClientInfoProductSetting> GetUIList(UISearchClientInfoProductSetting uiSearchObj = null);

        /// <summary>
        /// 获取UI List，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UIClientInfoProductSetting}.</returns>
        IList<UIClientInfoProductSetting> GetUIList(UISearchClientInfoProductSetting uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 根据查询条件获取一个实体
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>ClientInfoProductSetting.</returns>
        ClientInfoProductSetting GetOneByCondistions(UISearchClientInfoProductSetting uiSearchObj);
    }
}
