using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZhongDing.Common;

namespace ZhongDing.Web.Extensions
{
    public class SiteUser
    {
        HttpSessionState m_currentSession;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteUser"/> class.
        /// </summary>
        public SiteUser()
        {
            m_currentSession = HttpContext.Current.Session;

            if (this.UserID <= 0)
            {
                ZDMembershipUser mUser = ZDMembership.GetUser();

                if (mUser != null)
                {
                    this.UserID = mUser.UserID;
                    this.UserName = mUser.UserName;
                    this.DepartmentID = mUser.DepartmentID;
                    this.Email = mUser.Email;
                    this.FullName = mUser.FullName;
                    this.MobilePhone = mUser.MobilePhone;
                }
            }
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public static SiteUser GetCurrentSiteUser()
        {
            return new SiteUser();
        }

        /// <summary>
        /// 用户User ID
        /// </summary>
        public int UserID
        {
            get
            {
                if (m_currentSession != null && m_currentSession["UserID"] != null)
                {
                    return (int)(m_currentSession["UserID"]);
                }
                return GlobalConst.INVALID_INT;
            }
            set
            {
                m_currentSession.Add("UserID", value);
            }
        }

        /// <summary>
        /// 用户所属部门ID
        /// </summary>
        /// <value>部门ID.</value>
        public int DepartmentID
        {
            get
            {
                if (m_currentSession != null && m_currentSession["DepartmentID"] != null)
                {
                    return (int)(m_currentSession["DepartmentID"]);
                }
                return GlobalConst.INVALID_INT;
            }
            set
            {
                m_currentSession.Add("DepartmentID", value);
            }
        }

        /// <summary>
        /// 用户手机号
        /// </summary>
        /// <value>The mobile phone.</value>
        public string MobilePhone
        {
            get
            {
                if (m_currentSession != null && m_currentSession["MobilePhone"] != null)
                {
                    return m_currentSession["MobilePhone"].ToString();
                }

                return null;
            }
            set
            {
                m_currentSession.Add("MobilePhone", value);
            }
        }

        /// <summary>
        /// 登录用户名
        /// </summary>
        public string UserName
        {
            get
            {
                if (m_currentSession != null && m_currentSession["UserName"] != null)
                {
                    return m_currentSession["UserName"].ToString();
                }
                return null;
            }
            set
            {
                m_currentSession.Add("UserName", value);
            }
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string FullName
        {
            get
            {
                if (m_currentSession != null && m_currentSession["FullName"] != null)
                {
                    return m_currentSession["FullName"].ToString();
                }
                return null;
            }
            set
            {
                m_currentSession.Add("FullName", value);
            }
        }

        /// <summary>
        /// 用户email
        /// </summary>
        public string Email
        {
            get
            {
                if (m_currentSession != null && m_currentSession["Email"] != null)
                {
                    return m_currentSession["Email"].ToString();
                }

                return null;
            }
            set
            {
                m_currentSession.Add("Email", value);
            }
        }
    }
}