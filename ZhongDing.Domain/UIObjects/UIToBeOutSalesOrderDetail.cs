using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    /// <summary>
    /// 待出库销售订单明细
    /// </summary>
    public class UIToBeOutSalesOrderDetail : UIBase
    {
        public int SalesOrderApplicationID { get; set; }

        public int ProductID { get; set; }

        public int ProductSpecificationID { get; set; }

        public string OrderCode { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        /// <summary>
        /// 已出仓数量（或出库数量）
        /// </summary>
        public int OutQty { get; set; }

        /// <summary>
        /// 未出仓数量
        /// </summary>
        public int ToBeOutQty { get; set; }

        /// <summary>
        /// 本次出库数量
        /// </summary>
        public int CurrentOutQty { get; set; }

        /// <summary>
        /// 本次需开票数量
        /// </summary>
        /// <value>The current fax qty.</value>
        public int CurrentFaxQty { get; set; }

        /// <summary>
        /// 库存余量
        /// </summary>
        public int BalanceQty { get; set; }

        /// <summary>
        /// 有货的仓库
        /// </summary>
        public IList<UIDropdownItem> WarehouseData { get; set; }

    }
}
