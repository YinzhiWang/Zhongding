using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 枚举：导入状态
    /// </summary>
    public enum EImportStatus : int
    {
        /// <summary>
        /// 待导入
        /// </summary>
        ToBeImport = 1,

        /// <summary>
        /// 正在导入
        /// </summary>
        Importing,

        /// <summary>
        /// 导入出错
        /// </summary>
        ImportError,

        /// <summary>
        /// 完成导入
        /// </summary>
        Completed
    }
}
