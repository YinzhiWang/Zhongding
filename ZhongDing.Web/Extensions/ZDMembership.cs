using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Domain.Models;

namespace ZhongDing.Web.Extensions
{
    public class ZDMembership
    {
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <returns>ZDMembershipUser.</returns>
        public static ZDMembershipUser GetUser()
        {
            MembershipUser mUser = Membership.GetUser();

            ZDMembershipUser zDMembershipUser = null;

            if (mUser != null)
            {
                IUsersRepository usersRepository = new UsersRepository();

                Users user = usersRepository.GetByProviderUserKey((Guid)mUser.ProviderUserKey);

                string[] userRoles = System.Web.Security.Roles.GetRolesForUser(mUser.UserName);

                zDMembershipUser = new ZDMembershipUser(mUser.ProviderName,
                mUser.UserName,
                mUser.ProviderUserKey,
                mUser.Email,
                mUser.PasswordQuestion,
                mUser.Comment,
                mUser.IsApproved,
                mUser.IsLockedOut,
                mUser.CreationDate,
                mUser.LastLoginDate,
                mUser.LastActivityDate,
                mUser.LastPasswordChangedDate,
                mUser.LastLockoutDate,
                user != null ? user.UserID : 0,
                user != null ? (user.DepartmentID.HasValue ? user.DepartmentID.Value : 0) : 0,
                user != null ? user.MobilePhone : string.Empty,
                user != null ? user.FullName : string.Empty,
                userRoles);
            }

            return zDMembershipUser;
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="provideruserkey">The provideruserkey.</param>
        /// <returns>ZDMembershipUser.</returns>
        public static ZDMembershipUser GetUser(Guid provideruserkey)
        {
            MembershipUser mUser = Membership.GetUser(provideruserkey);

            ZDMembershipUser zDMembershipUser = null;

            if (mUser != null)
            {
                IUsersRepository usersRepository = new UsersRepository();

                Users user = usersRepository.GetByProviderUserKey((Guid)mUser.ProviderUserKey);

                string[] userRoles = System.Web.Security.Roles.GetRolesForUser(mUser.UserName);

                zDMembershipUser = new ZDMembershipUser(mUser.ProviderName,
                mUser.UserName,
                mUser.ProviderUserKey,
                mUser.Email,
                mUser.PasswordQuestion,
                mUser.Comment,
                mUser.IsApproved,
                mUser.IsLockedOut,
                mUser.CreationDate,
                mUser.LastLoginDate,
                mUser.LastActivityDate,
                mUser.LastPasswordChangedDate,
                mUser.LastLockoutDate,
                user != null ? user.UserID : 0,
                user != null ? (user.DepartmentID.HasValue ? user.DepartmentID.Value : 0) : 0,
                user != null ? user.MobilePhone : string.Empty,
                user != null ? user.FullName : string.Empty,
                userRoles);
            }

            return zDMembershipUser;
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="userIsOnline">if set to <c>true</c> [user is online].</param>
        /// <returns>ZDMembershipUser.</returns>
        public static ZDMembershipUser GetUser(string username, bool userIsOnline)
        {
            MembershipUser mUser = Membership.GetUser(username, userIsOnline);

            ZDMembershipUser zDMembershipUser = null;

            if (mUser != null)
            {
                IUsersRepository usersRepository = new UsersRepository();

                Users user = usersRepository.GetByProviderUserKey((Guid)mUser.ProviderUserKey);

                string[] userRoles = System.Web.Security.Roles.GetRolesForUser(username);

                zDMembershipUser = new ZDMembershipUser(mUser.ProviderName,
                mUser.UserName,
                mUser.ProviderUserKey,
                mUser.Email,
                mUser.PasswordQuestion,
                mUser.Comment,
                mUser.IsApproved,
                mUser.IsLockedOut,
                mUser.CreationDate,
                mUser.LastLoginDate,
                mUser.LastActivityDate,
                mUser.LastPasswordChangedDate,
                mUser.LastLockoutDate,
                user != null ? user.UserID : 0,
                user != null ? (user.DepartmentID.HasValue ? user.DepartmentID.Value : 0) : 0,
                user != null ? user.MobilePhone : string.Empty,
                user != null ? user.FullName : string.Empty,
                userRoles);
            }

            return zDMembershipUser;

        }
    }
}