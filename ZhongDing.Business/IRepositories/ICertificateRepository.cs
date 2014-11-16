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
    public interface ICertificateRepository : IBaseRepository<Certificate>
    {
        /// <summary>
        /// 获取证照，不分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>IList{UICertificate}.</returns>
        IList<UICertificate> GetUIList(UISearchCertificate uiSearchObj);

        /// <summary>
        /// 获取证照，分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <returns>IList{UICertificate}.</returns>
        IList<UICertificate> GetUIList(UISearchCertificate uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
    }
}
