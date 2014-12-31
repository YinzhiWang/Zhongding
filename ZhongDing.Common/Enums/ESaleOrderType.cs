using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 枚举：销售订单类型
    /// </summary>
    public enum ESaleOrderType : int
    {
        /// <summary>
        /// 大包配送模式
        /// </summary>
        DaBaoMode = 1,
        /// <summary>
        /// 招商模式
        /// </summary>
        AttractBusinessMode,
        /// <summary>
        /// 挂靠模式
        /// </summary>
        AttachedMode,

    }
}
