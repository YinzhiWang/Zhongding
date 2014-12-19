using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;

namespace ZhongDing.Business.IRepositories
{
    public interface IWorkflowStatusRepository : IBaseRepository<WorkflowStatus>, IGenerateDropdownItems
    {
        /// <summary>
        /// 根据工作流状态ID获取可访问的用户IDs
        /// </summary>
        /// <param name="statusID">The status ID.</param>
        /// <returns>IList{System.Int32}.</returns>
        IList<int> GetCanAccessUserIDsByID(int statusID);

        /// <summary>
        /// 根据用户ID获取该用户可以访问的状态列表
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns>IList{System.Int32}.</returns>
        IList<int> GetCanAccessIDsByUserID(int userID);
    }
}
