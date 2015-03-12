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
    public interface IDepartmentRepository : IBaseRepository<Department>, IGenerateDropdownItems
    {
        /// <summary>
        /// 获取UI List，不分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <returns>IList{UIDepartment}.</returns>
        IList<UIDepartment> GetUIList(UISearchDepartment uiSearchObj = null);

        /// <summary>
        /// 获取UI List，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UIDepartment}.</returns>
        IList<UIDepartment> GetUIList(UISearchDepartment uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 特定产品是否与该部门有关联
        /// </summary>
        /// <param name="departmentID">The department ID.</param>
        /// <param name="productID">The product ID.</param>
        /// <returns><c>true</c> if [is dept related with product] [the specified department ID]; otherwise, <c>false</c>.</returns>
        bool IsDeptRelatedWithProduct(int departmentID, int productID);

    }
}
