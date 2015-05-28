using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Common.Extension;
using ZhongDing.Common;
using ZhongDing.Common.Enums;

namespace ZhongDing.Business.Repositories
{
    public class UserGroupPermissionRepository : BaseRepository<UserGroupPermission>, IUserGroupPermissionRepository
    {
        public IList<Domain.UIObjects.UIUserGroupPermission> GetUIList(Domain.UISearchObjects.UISearchUserGroupPermission uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIUserGroupPermission> uiEntities = new List<UIUserGroupPermission>();
            int total = 0;

            List<Expression<Func<UserGroupPermission, bool>>> whereFuncs = new List<Expression<Func<UserGroupPermission, bool>>>();

            var query = from permission in DB.Permission
                        join userGroupPermission in DB.UserGroupPermission.Where(x => x.IsDeleted == false && x.UserGroupID == uiSearchObj.UserGroupID.Value) on permission.ID equals userGroupPermission.PermissionID into tempUserGroupPermissionList
                        from tempUserGroupPermission in tempUserGroupPermissionList.DefaultIfEmpty()
                        where permission.IsDeleted == false
                        select new UIUserGroupPermission()
                        {
                            UserGroupPermissionID = tempUserGroupPermission == null ? 0 : tempUserGroupPermission.ID,
                            PermissionID = permission.ID,
                            Value = tempUserGroupPermission == null ? 0 : tempUserGroupPermission.Value,
                            Name = permission.Name,
                            HasCreate = permission.HasCreate,
                            HasDelete = permission.HasDelete,
                            HasEdit = permission.HasEdit,
                            HasExport = permission.HasExport,
                            HasPrint = permission.HasPrint,
                            HasView = permission.HasView,
                        };
            total = query.Count();
            uiEntities = query.OrderBy(x => x.PermissionID).Skip(pageSize * pageIndex).Take(pageSize).ToList();
            totalRecords = total;

            uiEntities.ForEach(x =>
            {
                EPermissionOption permissionOption = PermissionManager.ConverEnum(x.Value);
                x.HasPermissionCreate = PermissionManager.HasRight(permissionOption, EPermissionOption.Create);
                x.HasPermissionDelete = PermissionManager.HasRight(permissionOption, EPermissionOption.Delete);
                x.HasPermissionEdit = PermissionManager.HasRight(permissionOption, EPermissionOption.Edit);
                x.HasPermissionExport = PermissionManager.HasRight(permissionOption, EPermissionOption.Export);
                x.HasPermissionPrint = PermissionManager.HasRight(permissionOption, EPermissionOption.Print);
                x.HasPermissionView = PermissionManager.HasRight(permissionOption, EPermissionOption.View);

            });

            return uiEntities;
        }


        public UserGroupPermission GetByUserGroupIDAndPermissionID(int userGroupID, int permissionID)
        {
            var entity = GetList(x => x.UserGroupID == userGroupID && x.PermissionID == permissionID).FirstOrDefault();
            return entity;
        }
    }
}
