using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    public enum EReimbursementType : int
    {
        /// <summary>
        /// 话费
        /// </summary>
        Telephone = 1,
        /// <summary>
        /// 出差
        /// </summary>
        Evection = 2,
        /// <summary>
        /// 物流费用
        /// </summary>
        TransportFee = 3,
        /// <summary>
        /// 托管配送费
        /// </summary>
        ManagedDistributionFee = 4,
        /// <summary>
        /// 杂项
        /// </summary>
        Other = 5
    }
}
