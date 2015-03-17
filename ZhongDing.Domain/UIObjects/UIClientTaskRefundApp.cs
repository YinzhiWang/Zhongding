using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientTaskRefundApp : UIWorkflowBase
    {
        public int CompanyID { get; set; }

        public string CompanyName { get; set; }

        public string ClientUserName { get; set; }

        public string ClientCompanyName { get; set; }

        /// <summary>
        /// 货品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 货品规格
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 返款日期
        /// </summary>
        public DateTime? RefundDate { get; set; }

        /// <summary>
        /// 返款金额
        /// </summary>
        public decimal? RefundAmount { get; set; }
    }
}
