using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 手动结算动作类型
    /// </summary>
    public enum EManualSettleActionType : int
    {
        /// <summary>
        /// 加入结算
        /// </summary>
        IncludeSettlement = 1,

        /// <summary>
        /// 取消结算
        /// </summary>
        ExcludeSettlement
    }
}
