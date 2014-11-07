using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;

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
    }
}
