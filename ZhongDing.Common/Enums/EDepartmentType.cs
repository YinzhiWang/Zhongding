using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 枚举：部门类型
    /// </summary>
    public enum EDepartmentType : int
    {
        /// <summary>
        /// 基药
        /// </summary>
        BaseMedicine = 1,
        /// <summary>
        /// 招商
        /// </summary>
        BusinessMedicine,

        /// <summary>
        /// 其他（如：人事部，行政部等）
        /// </summary>
        Other,
    }
}
