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
    public interface ISupplierContractFileRepository: IBaseRepository<SupplierContractFile>
    {
        /// <summary>
        /// 获取供应商的合同文件，不分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>IList{UISupplierContractFile}.</returns>
        IList<UISupplierContractFile> GetUIList(UISearchSupplierContractFile uiSearchObj = null);

        /// <summary>
        /// 获取供应商的合同文件，分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <returns>IList{UISupplierContractFile}.</returns>
        IList<UISupplierContractFile> GetUIList(UISearchSupplierContractFile uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

    }
}
