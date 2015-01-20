using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientSaleApplication : UIWorkflowBase
    {
        public string OrderCode { get; set; }

        public DateTime OrderDate { get; set; }

        public string SalesModel { get; set; }

        public string ClientUserName { get; set; }

        public string ClientCompanyName { get; set; }

        public bool IsGuaranteeTransaction { get; set; }

        public bool IsReturnedGuaranteeAmount { get; set; }

        public bool IsStop { get; set; }

        /// <summary>
        /// 担保图标的Url
        /// </summary>
        public string IconUrlOfGuarantee { get; set; }

    }
}
