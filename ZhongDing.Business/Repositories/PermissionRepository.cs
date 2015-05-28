using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;

namespace ZhongDing.Business.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public IDictionary<int, int> GetPermossionIDAndValues(int userID)
        {
            var query = from userGroupUser in DB.UserGroupUser
                        join userGroupPermission in DB.UserGroupPermission on userGroupUser.UserGroupID equals userGroupPermission.UserGroupID
                        where userGroupUser.IsDeleted == false && userGroupPermission.IsDeleted == false && userGroupUser.UserID == userID
                        group userGroupPermission by new { userGroupPermission.PermissionID, userGroupPermission.Value } into g
                        select new { g.Key.PermissionID, g.Key.Value };
            var permissionIDs = query.ToDictionary(x => x.PermissionID, x => x.Value);
            return permissionIDs;
        }
    }
}
