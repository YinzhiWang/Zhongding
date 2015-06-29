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
    public interface ISupplierRepository : IBaseRepository<Supplier>, IAutoSerialNo, IGenerateDropdownItems
    {
        /// <summary>
        /// 获取UI List，不分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>IList{UISupplier}.</returns>
        IList<UISupplier> GetUIList(UISearchSupplier uiSearchObj = null);

        /// <summary>
        /// 获取UI List，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UISupplier}.</returns>
        IList<UISupplier> GetUIList(UISearchSupplier uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 获取供应商的银行账号
        /// </summary>
        /// <param name="supplierID">供应商ID.</param>
        /// <returns>IList{UISupplierBankAccount}.</returns>
        IList<UISupplierBankAccount> GetBankAccounts(int? supplierID);
        /// <summary>
        /// Get By Name
        /// </summary>
        /// <param name="supplierName"></param>
        /// <returns></returns>
        Supplier GetBySupplierName(string supplierName);


        IList<UISupplierContact> GetContacts(int? supplyID);
    }
}
