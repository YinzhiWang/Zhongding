﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 发票税率类型
    /// </summary>
    public enum EInvoiceType : int
    {
        /// <summary>
        /// 高价税率
        /// </summary>
        HighRatio = 1,
        /// <summary>
        /// 低价税率
        /// </summary>
        LowRatio,
        /// <summary>
        /// 评价税率
        /// </summary>
        DeductionRatio
    }
}
