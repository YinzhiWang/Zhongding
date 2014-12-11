using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class UsersRepository : BaseRepository<Users>, IUsersRepository
    {
        public Users GetByProviderUserKey(Guid providerUserKey)
        {
            return this.BaseList().Where(x => x.AspnetUserID.HasValue && x.AspnetUserID == providerUserKey).FirstOrDefault();
        }

        public Users GetByUserName(string userName)
        {
            var query = (from u in DB.Users
                         join au in DB.aspnet_Users on u.AspnetUserID equals au.UserId
                         join am in DB.aspnet_Membership on au.UserId equals am.UserId
                         where u.IsDeleted == false
                         && au.UserName.ToLower().Equals(userName.ToLower())
                         select u).FirstOrDefault();

            return query;
        }

        public string GetUserNameByID(int userID)
        {
            var query = (from u in DB.Users
                         join au in DB.aspnet_Users on u.AspnetUserID equals au.UserId
                         join am in DB.aspnet_Membership on au.UserId equals am.UserId
                         where u.UserID == userID && u.IsDeleted == false && am.IsApproved
                         select au.UserName).FirstOrDefault();

            return query;
        }

        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<Users, bool>>> whereFuncs = new List<Expression<Func<Users, bool>>>();

            whereFuncs.Add(x => x.UserID != GlobalConst.DEFAULT_SYSTEM_ADMIN_USERID);//不显示系统内置的管理员账号

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.UserID));

                if (uiSearchObj.ExcludeItemValues != null
                    && uiSearchObj.ExcludeItemValues.Count > 0)
                    whereFuncs.Add(x => !uiSearchObj.ExcludeItemValues.Contains(x.UserID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.FullName.Contains(uiSearchObj.ItemText));

                if (uiSearchObj.Extension != null)
                {
                    if (uiSearchObj.Extension.DepartmentID > 0)
                        whereFuncs.Add(x => x.DepartmentID == uiSearchObj.Extension.DepartmentID);
                }
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.UserID,
                ItemText = x.FullName
            }).ToList();

            return uiDropdownItems;
        }

        public IList<UIUser> GetUIList(UISearchUser uiSearchObj = null)
        {
            IList<UIUser> uiEntities = new List<UIUser>();

            IQueryable<Users> query = null;

            List<Expression<Func<Users, bool>>> whereFuncs = new List<Expression<Func<Users, bool>>>();

            whereFuncs.Add(x => x.UserID != GlobalConst.DEFAULT_SYSTEM_ADMIN_USERID);//不显示系统内置的管理员账号

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.UserID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DepartmentID > 0)
                    whereFuncs.Add(x => x.DepartmentID == uiSearchObj.DepartmentID);

                if (!string.IsNullOrEmpty(uiSearchObj.UserName))
                    whereFuncs.Add(x => x.UserName.Contains(uiSearchObj.UserName));

                if (!string.IsNullOrEmpty(uiSearchObj.FullName))
                    whereFuncs.Add(x => x.FullName.Contains(uiSearchObj.FullName));

                if (uiSearchObj.AspnetUserID.HasValue)
                    whereFuncs.Add(x => x.AspnetUserID == uiSearchObj.AspnetUserID);
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join d in DB.Department on q.DepartmentID equals d.ID into tempD
                              from td in tempD.DefaultIfEmpty()
                              select new UIUser()
                              {
                                  ID = q.UserID,
                                  AspnetUserID = q.AspnetUserID,
                                  UserName = q.UserName,
                                  FullName = q.FullName,
                                  DepartmentName = td == null ? string.Empty : td.DepartmentName,
                                  Position = q.Position,
                                  MobilePhone = q.MobilePhone,
                                  EnrollDate = q.EnrollDate
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIUser> GetUIList(UISearchUser uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIUser> uiEntities = new List<UIUser>();

            int total = 0;

            IQueryable<Users> query = null;

            List<Expression<Func<Users, bool>>> whereFuncs = new List<Expression<Func<Users, bool>>>();

            whereFuncs.Add(x => x.UserID != GlobalConst.DEFAULT_SYSTEM_ADMIN_USERID);//不显示系统内置的管理员账号

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.UserID.Equals(uiSearchObj.ID));

                if (uiSearchObj.DepartmentID > 0)
                    whereFuncs.Add(x => x.DepartmentID == uiSearchObj.DepartmentID);

                if (!string.IsNullOrEmpty(uiSearchObj.UserName))
                    whereFuncs.Add(x => x.UserName.Contains(uiSearchObj.UserName));

                if (!string.IsNullOrEmpty(uiSearchObj.FullName))
                    whereFuncs.Add(x => x.FullName.Contains(uiSearchObj.FullName));

                if (uiSearchObj.AspnetUserID.HasValue)
                    whereFuncs.Add(x => x.AspnetUserID == uiSearchObj.AspnetUserID);
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join d in DB.Department on q.DepartmentID equals d.ID into tempD
                              from td in tempD.DefaultIfEmpty()
                              select new UIUser()
                              {
                                  ID = q.UserID,
                                  AspnetUserID = q.AspnetUserID,
                                  UserName = q.UserName,
                                  FullName = q.FullName,
                                  DepartmentName = td == null ? string.Empty : td.DepartmentName,
                                  Position = q.Position,
                                  MobilePhone = q.MobilePhone,
                                  EnrollDate = q.EnrollDate
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
