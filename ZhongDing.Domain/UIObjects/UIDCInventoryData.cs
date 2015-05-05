using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDCInventoryData : UIBase
    {
        public string DistributionCompanyName { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public DateTime SettlementDate { get; set; }

        /// <summary>
        /// 配送公司库存
        /// </summary>
        public int? DCBalanceQty { get; set; }

        /// <summary>
        /// 公司账面库存
        /// </summary>
        public int? BookBalanceQty { get; set; }
    }
}
