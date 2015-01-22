using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 发货模式
    /// </summary>
    public enum EDeliveryMode : int
    {
        /// <summary>
        /// 查款发货
        /// </summary>
        ReceiptedDelivery = 1,

        /// <summary>
        /// 担保发货
        /// </summary>
        GuaranteeDelivery,
    }
}
