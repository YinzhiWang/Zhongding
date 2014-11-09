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
    public interface ISupplierCertificateRepository : IBaseRepository<SupplierCertificate>
    {

        /// <summary>
        /// 获取供应商的证照，不分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>IList{UISupplierCertificate}.</returns>
        IList<UISupplierCertificate> GetUIList(UISearchCertificate uiSearchObj = null);

        /// <summary>
        /// 获取供应商的证照，分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <returns>IList{UISupplierCertificate}.</returns>
        IList<UISupplierCertificate> GetUIList(UISearchCertificate uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

    }
}
