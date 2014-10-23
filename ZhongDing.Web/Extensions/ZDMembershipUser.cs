using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ZhongDing.Web.Extensions
{
    public class ZDMembershipUser : MembershipUser
    {
        
        #region 自定义字段 属性

        private int _UserID;
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        private int _DepartmentID;

        public int DepartmentID
        {
            get { return _DepartmentID; }
            set { _DepartmentID = value; }
        }

        private string _MobilePhone;
        public string MobilePhone
        {
            get { return _MobilePhone; }
            set { _MobilePhone = value; }
        }

        private string _FullName;
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }

        private string[] _userRoles;

        /// <summary>
        /// 用户角色
        /// </summary>
        /// <value>The user roles.</value>
        public string[] UserRoles
        {
            get { return _userRoles; }
            set { _userRoles = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ZDMembershipUser"/> class.
        /// </summary>
        public ZDMembershipUser() { }

        public ZDMembershipUser(string providername,
            string username,
            object providerUserKey,
            string email,
            string passwordQuestion,
            string comment,
            bool isApproved,
            bool isLockedOut,
            DateTime creationDate,
            DateTime lastLoginDate,
            DateTime lastActivityDate,
            DateTime lastPasswordChangedDate,
            DateTime lastLockedOutDate,
            int userID,
            int departmentID,
            string mobilePhone,
            string fullName,
            string[] userRoles) :
            base(providername,
            username,
            providerUserKey,
            email,
            passwordQuestion,
            comment,
            isApproved,
            isLockedOut,
            creationDate,
            lastLoginDate,
            lastActivityDate,
            lastPasswordChangedDate,
            lastLockedOutDate)
        {
            this.UserID = userID;
            this.DepartmentID = departmentID;
            this.MobilePhone = mobilePhone;
            this.FullName = fullName;
            this.UserRoles = userRoles;
        }
    }
}