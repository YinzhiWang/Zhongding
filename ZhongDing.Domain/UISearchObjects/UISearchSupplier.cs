using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    /// <summary>
    /// 类：供应商查询实体
    /// </summary>
    public class UISearchSupplier : UISearchBase
    {
        /// <summary>
        ///  账套ID
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        /// <value>The supplier code.</value>
        public string SupplierCode { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        /// <value>The name of the supplier.</value>
        public string SupplierName { get; set; }
        /// <summary>
        /// 生产企业
        /// </summary>
        /// <value>The name of the factory.</value>
        public string FactoryName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        /// <value>The contact person.</value>
        public string ContactPerson { get; set; }
    }
}
