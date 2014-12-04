using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 枚举：销售计划类型
    /// </summary>
    public enum ESalesPlanType : int
    {
        /// <summary>
        /// 基础提成比例
        /// </summary>
        Inside = 1,

        /// <summary>
        /// 超出提成比例
        /// </summary>
        Outside
    }
}
