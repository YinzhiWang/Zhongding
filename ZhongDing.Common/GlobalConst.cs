using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common
{
    /// <summary>
    /// 公共常量类
    /// </summary>
    public class GlobalConst
    {
        /// <summary>
        /// 无效的Int:-1
        /// </summary>
        public static readonly int INVALID_INT = -1;

        /// <summary>
        /// 系统母版页主标题
        /// </summary>
        public static readonly string WEBSITE_MASTER_PAGE_MAIN_TITLE = "众鼎医药咨询信息系统";

        /// <summary>
        /// 是否有某种证照：有
        /// </summary>
        public static readonly string GOTTEN_DESC_HAVE = "有";
        /// <summary>
        /// 是否有某种证照：无
        /// </summary>
        public static readonly string GOTTEN_DESC_NONHAVE = "无";


        #region System Security

        /// <summary>
        /// 密码恢复时格式默认长度：8
        /// </summary>
        public static readonly int DEFAULT_PASSWORD_RESET_LENGTH = 8;

        /// <summary>
        /// 密码恢复时格式中特殊字符的默认个数：1
        /// </summary>
        public static readonly int DEFAULT_PASSWORD_RESET_NONALPHANUMERIC_COUNT = 1;

        /// <summary>
        /// 默认锁定用户超时时间：20分钟
        /// </summary>
        public static readonly int DEFAULT_LOCKEDOUT_TIMEOUT = 20;

        /// <summary>
        /// 系统管理员默认用户名ID
        /// </summary>
        public static readonly int DEFAULT_SYSTEM_ADMIN_USERID = 1;

        #endregion

        #region Rad Notification提示框 属性设置

        public class NotificationSettings
        {
            /// <summary>
            /// Title:提示
            /// </summary>
            public static readonly string TITLE_SUCCESS = "提示";
            /// <summary>
            /// Title:错误
            /// </summary>
            public static readonly string TITLE_ERROR = "错误";

            /// <summary>
            /// ContentIcon：成功的
            /// </summary>
            public static readonly string CONTENT_ICON_SUCCESS = "~/Content/icons/32/tick.png";
            /// <summary>
            /// ContentIcon：错误的
            /// </summary>
            public static readonly string CONTENT_ICON_ERROR = "~/Content/icons/32/cross.png";

        }


        #endregion
    }
}
