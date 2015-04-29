using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 保证金类别
    /// </summary>
    public enum ECautionMoneyType : int
    {
        /// <summary>
        /// 增补保证金
        /// </summary>
        AddCautionMoney = 1,
        /// <summary>
        /// 市场保证金
        /// </summary>
        MarketCautionMoney = 2

    }
}
