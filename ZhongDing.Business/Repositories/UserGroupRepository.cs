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
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Common;

namespace ZhongDing.Business.Repositories
{
    public class UserGroupRepository : BaseRepository<UserGroup>, IUserGroupRepository
    {
        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<UserGroup, bool>>> whereFuncs = new List<Expression<Func<UserGroup, bool>>>();

            //whereFuncs.Add(x => x.UserID != GlobalConst.DEFAULT_SYSTEM_ADMIN_USERID);//不显示系统内置的管理员账号

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (uiSearchObj.ExcludeItemValues != null
                    && uiSearchObj.ExcludeItemValues.Count > 0)
                    whereFuncs.Add(x => !uiSearchObj.ExcludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.GroupName.Contains(uiSearchObj.ItemText));

                //if (uiSearchObj.Extension != null)
                //{
                //    if (uiSearchObj.Extension.DepartmentID > 0)
                //        whereFuncs.Add(x => x.DepartmentID == uiSearchObj.Extension.DepartmentID);
                //}
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.GroupName
            }).ToList();

            return uiDropdownItems;
        }

        public IList<Domain.UIObjects.UIUserGroup> GetUIList(Domain.UISearchObjects.UISearchUserGroup uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIUserGroup> uiEntities = new List<UIUserGroup>();
            int total = 0;

            IQueryable<UserGroup> query = null;

            List<Expression<Func<UserGroup, bool>>> whereFuncs = new List<Expression<Func<UserGroup, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.GroupName.IsNotNullOrEmpty())
                {
                    whereFuncs.Add(x => x.GroupName.Contains(uiSearchObj.GroupName));
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              select new UIUserGroup()
                              {
                                  ID = q.ID,
                                  Comment = q.Comment,
                                  GroupName = q.GroupName,
                                  UserIDs = q.UserGroupUser.Where(x => x.IsDeleted == false).Select(x => x.UserID)
                              }).ToList();

                foreach (var entity in uiEntities)
                {
                    entity.UserNames = string.Join(", ", DB.Users.Where(x => entity.UserIDs.Contains(x.UserID)).Select(x => x.FullName).ToList());
                }
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
