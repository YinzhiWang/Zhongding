using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 枚举：所有者类型
    /// </summary>
    public enum EOwnerType : int
    {
        /// <summary>
        /// 账套
        /// </summary>
        Company = 1,
        /// <summary>
        /// 供应商
        /// </summary>
        Supplier,
        /// <summary>
        /// 客户
        /// </summary>
        Client,
        /// <summary>
        /// 生产企业
        /// </summary>
        Producer,
        /// <summary>
        /// 货品
        /// </summary>
        Product
    }
}
