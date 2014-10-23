using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    /// <summary>
    /// 类：账套查询实体
    /// </summary>
    public class UISearchCompany : UISearchBase
    {
        /// <summary>
        /// 账套编号
        /// </summary>
        /// <value>The company code.</value>
        public string CompanyCode { get; set; }

        /// <summary>
        /// 账套名称
        /// </summary>
        /// <value>The name of the company.</value>
        public string CompanyName { get; set; }

    }
}
