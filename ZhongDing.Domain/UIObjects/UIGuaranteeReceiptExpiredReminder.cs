using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIGuaranteeReceiptExpiredReminder : UIBase
    {
        public string OrderCode { get; set; }

        public DateTime OrderDate { get; set; }

        public string SaleOrderTypeName { get; set; }

        public string ClientUserName { get; set; }

        public string ClientCompanyName { get; set; }

        public bool IsGuaranteed { get; set; }

        /// <summary>
        /// 担保金额是否收回
        /// </summary>
        /// <value><c>true</c> if this instance is receipted guarantee amount; otherwise, <c>false</c>.</value>
        public bool IsReceiptedGuaranteeAmount { get; set; }

        public DateTime? GuaranteeExpirationDate { get; set; }

        public decimal GuaranteeAmount { get; set; }

        public string GuaranteebyFullName { get; set; }
    }
}
