using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 导入数据类型
    /// </summary>
    public enum EImportDataType : int
    {
        /// <summary>
        /// 配送公司流向数据
        /// </summary>
        DCFlowData = 1,

        /// <summary>
        /// 配送公司库存数据
        /// </summary>
        DCInventoryData,

        /// <summary>
        /// 商业客户流向数据
        /// </summary>
        ClientFlowData,

        /// <summary>
        /// 采购订单数据
        /// </summary>
        ProcureOrderData,

        /// <summary>
        /// 入库单数据
        /// </summary>
        StockInData,
    }
}
