using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientNeedRefundOrder : UIBase
    {
        public int CompanyID { get; set; }

        public string CompanyName { get; set; }

        public int ClientSaleAppID { get; set; }

        public string OrderCode { get; set; }

        public string ClientUserName { get; set; }

        public string ClientCompanyName { get; set; }

        //public string ProductName { get; set; }

        //public string Specification { get; set; }

        public decimal? RefundAmount { get; set; }

        public bool IsGuaranteed { get; set; }

        /// <summary>
        /// 担保金额是否收回
        /// </summary>
        public bool IsReceiptedGuaranteeAmount { get; set; }

        /// <summary>
        /// 担保图标的Url
        /// </summary>
        public string IconUrlOfGuarantee { get; set; }
    }
}
