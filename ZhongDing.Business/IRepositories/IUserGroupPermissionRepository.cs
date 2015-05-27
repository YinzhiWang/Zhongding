using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IUserGroupPermissionRepository : IBaseRepository<UserGroupPermission>
    {
        IList<UIUserGroupPermission> GetUIList(Domain.UISearchObjects.UISearchUserGroupPermission uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        UserGroupPermission GetByUserGroupIDAndPermissionID(int userGroupID, int permissionID);
    }
}
